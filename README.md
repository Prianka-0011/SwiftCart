# SwiftCart

# Folder Structure

SwiftCart/
├── src/
│   ├── SwiftCart.Backend/
│   │   ├── SwiftCart.API/               # REST API project (Controllers, Middlewares, etc.) — Presentation Layer
│   │   ├── SwiftCart.Application/       # Application services, DTOs, business logic — Application Layer
│   │   ├── SwiftCart.Domain/            # Entities, Value Objects, Enums, Domain Events — Core Domain Layer
│   │   └── SwiftCart.Infrastructure/    # EF Core, Repositories, Configurations, External Services — Infrastructure Layer
│   │
│   └── Web-Spa/                         # Angular frontend project (SPA)
│       ├── src/
│       │   ├── app/
|       |   |   |── admin/
|       |   |   |── auth/
|       |   |   |── ecomerce    
│       |   |   | -n 
│       │   └── assets/                  # Images, icons, styles
│       └── angular.json
│
├── tests/
│   ├── SwiftCart.UnitTests/             # Unit tests for domain and application layers
│   └── SwiftCart.IntegrationTests/      # Integration tests for API and database
│
├── docker-compose.yml                   # Docker Compose for backend + frontend containers
└── SwiftCart.sln  

# ==============================================================
# 🏗️ SwiftCart Solution Setup — Clean Architecture
# ==============================================================

# 1️⃣ Create Solution
dotnet new sln -n SwiftCart

# ==============================================================
# 2️⃣ Backend Projects
# ==============================================================

# 🧱 Domain Layer
dotnet new classlib -n SwiftCart.Domain -o src/SwiftCart.Backend/SwiftCart.Domain

# ⚙️ Application Layer
dotnet new classlib -n SwiftCart.Application -o src/SwiftCart.Backend/SwiftCart.Application
cd src/SwiftCart.Backend/SwiftCart.Application
dotnet add package MediatR
dotnet add package AutoMapper
cd ../../..

# 🧩 Infrastructure Layer
dotnet new classlib -n SwiftCart.Infrastructure -o src/SwiftCart.Backend/SwiftCart.Infrastructure
cd src/SwiftCart.Backend/SwiftCart.Infrastructure
dotnet add package Microsoft.EntityFrameworkCore --version 8.0.10
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 8.0.10
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 8.0.10
dotnet add package Microsoft.Extensions.Configuration --version 8.0.0
dotnet add package Microsoft.Extensions.Configuration.Binder --version 8.0.2
dotnet add package Microsoft.Extensions.DependencyInjection --version 8.0.1
cd ../../..

# 🌐 API Layer
dotnet new webapi -n SwiftCart.API -o src/SwiftCart.Backend/SwiftCart.API
dotnet add package Microsoft.EntityFrameworkCore --version 8.0.10
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 8.0.10
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 8.0.10

# ==============================================================
# 3️⃣ Project References
# ==============================================================

# Infrastructure → Application + Domain
cd src/SwiftCart.Backend/SwiftCart.Infrastructure
dotnet add reference ../SwiftCart.Application/SwiftCart.Application.csproj
dotnet add reference ../SwiftCart.Domain/SwiftCart.Domain.csproj
cd ../../..

# API → Application + Infrastructure
cd src/SwiftCart.Backend/SwiftCart.API
dotnet add reference ../SwiftCart.Application/SwiftCart.Application.csproj
dotnet add reference ../SwiftCart.Infrastructure/SwiftCart.Infrastructure.csproj
cd ../../..

# ==============================================================
# 4️⃣ Add Projects to Solution
# ==============================================================

dotnet sln SwiftCart.sln add src/SwiftCart.Backend/SwiftCart.Domain/SwiftCart.Domain.csproj
dotnet sln SwiftCart.sln add src/SwiftCart.Backend/SwiftCart.Application/SwiftCart.Application.csproj
dotnet sln SwiftCart.sln add src/SwiftCart.Backend/SwiftCart.Infrastructure/SwiftCart.Infrastructure.csproj
dotnet sln SwiftCart.sln add src/SwiftCart.Backend/SwiftCart.API/SwiftCart.API.csproj

# ==============================================================
# ✅ Final Step
# ==============================================================

dotnet restore
