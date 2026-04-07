# 🧭 CourseCompass

> AI-powered academic planning platform for Montclair State University

CourseCompass is a backend REST API built with ASP.NET Core that helps Montclair State University students plan their academic journey. It leverages AI to assist with course selection, degree planning, and academic guidance.

---

## 🚀 Features

- AI-assisted academic planning and course recommendations
- RESTful API with full Swagger/OpenAPI documentation
- JWT-based authentication and authorization
- Entity Framework Core with SQL Server for persistent data storage
- Clean architecture with Controllers, Models, DTOs, and Services layers

---

## 🛠️ Tech Stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core 8 |
| Language | C# (.NET 8) |
| Database | SQL Server |
| ORM | Entity Framework Core 8 |
| Auth | JWT Bearer Tokens |
| API Docs | Swagger (Swashbuckle) |

---

## 📁 Project Structure

```
CourseCompass/
├── Controllers/        # API route handlers
├── Data/               # DbContext and database configuration
├── DTOs/               # Data Transfer Objects
├── Migrations/         # EF Core database migrations
├── Models/             # Entity models
├── Properties/         # Launch settings
├── Services/           # Business logic layer
├── Program.cs          # App entry point and DI configuration
├── appsettings.json    # App configuration
└── CourseCompass.csproj
```

---

## ⚙️ Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server (local or remote instance)

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/nurbu/CourseCompass.git
   cd CourseCompass
   ```

2. **Configure the database connection**

   Update `appsettings.json` with your SQL Server connection string:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=YOUR_SERVER;Database=CourseCompassDb;Trusted_Connection=True;"
     }
   }
   ```

3. **Apply database migrations**
   ```bash
   dotnet ef database update
   ```

4. **Run the application**
   ```bash
   dotnet run
   ```

5. **Access Swagger UI**

   Navigate to `https://localhost:{PORT}/swagger` to explore the API.

---

## 🔐 Authentication

This API uses JWT Bearer token authentication. To access protected endpoints:

1. Register or log in to receive a token.
2. Include the token in the `Authorization` header:
   ```
   Authorization: Bearer <your_token>
   ```

---

## 📬 API Endpoints

Full API documentation is available via Swagger at `/swagger` when the app is running.

---

## 🤝 Contributing

Contributions are welcome! Please open an issue or submit a pull request.

---

## 📄 License

This project is intended for use at Montclair State University. See the repository for license details.
