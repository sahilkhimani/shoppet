# Pet Shop API

Welcome to the **Pet Shop API** repository! This project is a backend implementation for managing a pet shop, providing APIs for user authentication, pet management, role-based access control, and more. The project is designed using **Onion Architecture** (Clean Architecture) to ensure separation of concerns, scalability, and maintainability.

---

## **Features**
- **User Management**: Includes signup, login, and role-based authorization.
- **Role Management**: Admin, Seller, and Buyer roles with specific access controls.
- **Pet Management**: CRUD operations for pets with detailed attributes like breed, species.
- **Order Management**: Place, update, and cancel orders with role-based access control.
- **Generic Repository Pattern**: Centralized and reusable data access logic.
- **Complete Abstraction**: Interfaces are implemented for all repositories and services.
- **Onion Architecture**: Layers include Domain, Repository, Service, and API.
- **JWT Authentication**: Secure login with token-based authorization.
- **Identity Framework Integration**: Provides robust authentication and role management, customized for the project's needs.
- **Swagger Integration**: API documentation for easier testing and exploration. you can also test endpoints on postman.
- **Database Configuration**: Supports SQL Server as the database.

---

## **Technologies and Packages Used**
### **Framework**
- ASP.NET Core Web API (.NET 8.0)

### **NuGet Packages**
- **Entity Framework Core**: `Microsoft.EntityFrameworkCore`, `Microsoft.EntityFrameworkCore.Design`
- **Database**: `Microsoft.EntityFrameworkCore.SqlServer`, `Microsoft.EntityFrameworkCore.Tools`
- **JWT Authentication**: `Microsoft.AspNetCore.Authentication.JwtBearer`
- **AutoMapper**: `AutoMapper`
- **Swagger**: `Swashbuckle.AspNetCore`
- **Identity Framework**: `Microsoft.AspNetCore.Identity.EntityFrameworkCore`
---

## **Project Structure**
The project follows the **Onion Architecture** (Clean Architecture), structured as follows:

### **1. Domain Layer**
- Contains entities like `User`, `Role`, `Pet`, `Breed`, `Species` and `Order`.
- Defines the core business logic and rules.

### **2. Repository Layer**
- Implements the **Generic Repository Pattern**.
- Handles database access using Entity Framework Core.
- Example repositories: `UserRepository`, `PetRepository`.

### **3. Service Layer**
- Contains service interfaces and implementations.
- Business logic is encapsulated in services like `UserService`, `PetService`.
- Uses Dependency Injection for repository access.

### **4. API Layer**
- Contains controllers for exposing API endpoints.
- Example controllers: `UserController`, `PetController`.
- Integrates Swagger for API documentation.

---

## **Setup and Configuration**

### **Clone the Repository**
```bash
git clone https://github.com/sahilkhimani/shoppet.git
cd shoppet
```

### **Database Configuration**
1. Ensure you have **SQL Server** installed and running.
2. Open the `appsettings.json` file and update the connection string:
3. Also add the key there create your own random key. The key should be lengthy otherwise you will get an error.
   ```json
   {
   "Key": "CREATEYOUROWNLENGTHYKEYTORUNTHISAPIPROJECT",
   }
   "ConnectionStrings": {
       "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=PetShopDB;Trusted_Connection=True;"
   }
   ```
4. Run the following command to apply migrations and update the database:
   Run this command in console manager to apply migrations
   ```bash
   Update-Database
   ```

## **Contributing**
Feel free to fork this repository and submit pull requests for any new features or bug fixes. Make sure to follow the project's coding standards and structure.

