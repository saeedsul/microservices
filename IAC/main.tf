
terraform {
  required_providers {
    docker = {
      source  = "kreuzwerker/docker"
      version = "~> 3.0.1"
    }
  }
}

provider "docker" {
  host = var.docker_host
} 

# Define React app image
resource "docker_image" "react_app" {
  name = var.react_image_name
  build {
    context    = "${path.module}/../ux/"   
    dockerfile = "Dockerfile"   
  }   
} 
 

# Define Docker api image
resource "docker_image" "api_image" {
  name = var.api_image_name
  build {
    context    = "${path.module}/../"   
    dockerfile = "Dockerfile"  
  }
} 
 
# Define Docker network
resource "docker_network" "app_network" {
  name = var.network_name
  depends_on   = [docker_image.api_image]
}

# Define persistent volume for SQL Server data
resource "docker_volume" "sqlserver_data" {
  name = var.db_volume_name
  depends_on   = [docker_image.api_image]
}

# Define SQL Server container with persistent storage
resource "docker_container" "sql_server_db" {
  name         = var.db_container_name
  image        = var.db_image_name
  ports {
    internal = 1433
    external = 8002
  }
  env = [
    "ACCEPT_EULA=Y",
    "MSSQL_SA_PASSWORD=myStong_Password123#"
  ]
  network_mode = docker_network.app_network.name
  depends_on   = [docker_image.api_image]
  # Mount the persistent volume for SQL Server data
  mounts {
    target = "/var/opt/mssql"
    source = docker_volume.sqlserver_data.name
    type   = "volume"
  }
}

# RabbitMQ Image
resource "docker_image" "rabbitmq" {
  name = "rabbitmq:3-management"
}

# RabbitMQ Container
resource "docker_container" "rabbitmq" {
  name  = "rabbitmq"
  image = docker_image.rabbitmq.name

  ports {
    internal = 5672   # AMQP
    external = 5672
  }

  ports {
    internal = 15672  # Management UI
    external = 15672
  }

  env = [
    "RABBITMQ_DEFAULT_USER=guest",
    "RABBITMQ_DEFAULT_PASS=guest"
  ]

  network_mode = docker_network.app_network.name
  depends_on   = [docker_image.rabbitmq]
}

# Define API container
resource "docker_container" "api_container" {
  name         = var.api_container_name
  image        = docker_image.api_image.name
  ports {
    internal = 80
    external = 8001
  }
  env = [
    "ConnectionStrings__dbConnectionString=Server=${docker_container.sql_server_db.name};Database=ShoppingDb;User Id=SA;Password=myStong_Password123#;Encrypt=False;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False",
    "ASPNETCORE_ENVIRONMENT=Development",
    "ASPNETCORE_URLS=http://+:80",
    "RabbitMQ__Host=rabbitmq",
    "RabbitMQ__Port=5672",
    "RabbitMQ__Username=guest",
    "RabbitMQ__Password=guest"

  ]
  network_mode = docker_network.app_network.name
  depends_on   = [docker_container.sql_server_db, docker_image.api_image]
  
  # Define volumes for API container
  volumes {
    host_path      = "${pathexpand("~")}/.microsoft/usersecrets"
    container_path = "/root/.microsoft/usersecrets"
    read_only      = true
  }

  volumes {
    host_path      = "${pathexpand("~")}/.aspnet/https"
    container_path = "/root/.aspnet/https"
    read_only      = true
  }
  healthcheck {
    test     = ["CMD", "curl", "-f", "http://localhost:80/health"]
    interval = "30s"
    timeout  = "10s"
    retries  = 3
  }
}  

# React App Container
resource "docker_container" "react_app_container" {
  name         = var.react_container_name
  image        = docker_image.react_app.name   

  ports {
    internal = 80
    external = 3000
  }

  networks_advanced {
    name         = docker_network.app_network.name
    aliases      = ["react-shopping-frontend"]
  }
  
  network_mode = docker_network.app_network.name
  depends_on   = [docker_container.api_container, docker_image.react_app]

  env = [
    "VITE_API_URL=http://localhost:8001",
    "VITE_API_INTERNAL_URL=http://${docker_container.api_container.name}:80",
    "VITE_API_LOCAL=http://localhost:5178"
  ]

   healthcheck {
    test     = ["CMD", "curl", "-f", "http://localhost:80"]
    interval = "30s"
    timeout  = "10s"
    retries  = 3
  }

  labels {
    label = "traefik.enable"
    value = "true"
  }

  labels {
    label = "traefik.http.routers.react.rule"
    value = "Host(`react.localhost`)"
  }

  mounts {
    target = "/app/node_modules"
    source = "react_node_modules"
    type   = "volume"
  }

  mounts {
    target = "/app/src"
    source = abspath("${path.module}/../ux/src")
    type   = "bind"
    read_only = true
  } 

  restart = "unless-stopped"
  must_run = true
}

