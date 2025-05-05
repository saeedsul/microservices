
# Designer  (Shopping Cart)

.Net core 9 application with React TSX front end, using microservice clean architecture.
RabbitMq and MassTransit with EF.

## Folder Structure
```
Shopping.sln 
.gitignore  
azure-pipeline.yml
launchSettings 
|-- Api/             # ASP.NET Core Web API project
|-- Test/            # Unit and Integration tests
|-- UX/              # Frontend application (React TSX.)
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
 
 <img width="313" alt="image" src="https://github.com/user-attachments/assets/0e72fff5-ed61-42ca-8039-154d67a95615" />
<img width="1312" alt="image-3" src="https://github.com/user-attachments/assets/e3a4d19d-1316-4ceb-8d6b-f3447bf8542f" />
<img width="1166" alt="image-2" src="https://github.com/user-attachments/assets/0cae1fb0-2f13-4b86-a37d-ee6aed237959" />
<img width="339" alt="image-1" src="https://github.com/user-attachments/assets/dbf630fb-629b-4334-bb7f-5c140b9bc335" />

## For HTTP File Execution
More about HTTP files: [https://aka.ms/vs/httpfile](https://aka.ms/vs/httpfile)

---

## License
This project is licensed under the MIT License.
