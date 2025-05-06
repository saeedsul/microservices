
# Shopping Cart

.Net core 9 application with React TSX front end, using microservice clean architecture.
RabbitMq and MassTransit with EF.
- Api .net 9 project using Repository pattern
- Use Sql server database Server=(localdb)\\mssqllocaldb  

## Folder Structure
```
Shopping.sln 
.gitignore  
azure-pipeline.yml
launchSettings 
|-- Api/             # ASP.NET Core Web API project
|-- IAC/             # Infrastructure as Code (Terraform scripts)
|-- Test/            # Unit and Integration tests
|-- UX/              # Frontend application (React TSX.)
```

---
 

## Infrastructure as Code (IAC)
- The `IAC/` folder contains Terraform scripts for provisioning cloud infrastructure (e.g., App, Database).
- cd IAC
- terraform init
- terraform plan
- terraform apply --auto-approve
- this will build api project and great the api contianer and the sql db container
- open a browser and go to http://localhost:80 

---

## Docker
- A `Dockerfile` is available to containerize and run the API.
- Build and run the Docker container:
  ```bash
  docker build -t movies-api .
  docker run -p 80:80 movies-api
  ```

---

## Base URL
```
@Api_HostAddress = http://localhost:5178/api

GET {{Api_HostAddress}}/product/
Accept: application/json

###
POST {{Api_HostAddress}}/order/create
Accept: application/json
Content-Type: application/json

{
    "customerId":123,
    "items":[
        {"id":3,
        "name":"Keyboard",
        "price":22.99,
        "quantity":1
        }],
    "totalAmount":22.99,
    "shippingAddress":{
        "street":"212",
        "city":"Leeds",
        "postalCode":"LS16 7AZ"
        }
}
   
---

## Run Locally
1. Build and run the solution using Visual Studio or .NET CLI.
2. API will be available at `http://localhost:5178/api`.

---

## React App 
1. make sure api is running
2. npm run dev

 
![place order](image.png)

![get products](image-1.png)

![react app1](image-2.png)

![react app2](image-3.png)

---