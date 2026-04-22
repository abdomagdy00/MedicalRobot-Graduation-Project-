# 🏥 Medical Care Robot System (Backend)

An intelligent healthcare assistant robot designed to bridge the gap between software and hardware. This system handles face recognition, automated medicine dispensing, and real-time vital signs monitoring.

## 🏗️ Architecture: Clean Architecture
The project is built following **Clean Architecture** principles to ensure scalability, maintainability, and independence of frameworks.

### 📂 Project Layers:
- **Core (Domain):** Entities, Enums, and Repository Interfaces.
- **Application:** Business Logic, DTOs, Mapping, and Services.
- **Infrastructure:** Persistence (SQL Server), Entity Framework Core, and Repository Implementations.
- **API (Presentation):** Controllers, SignalR Hubs, and Global Exception Handling.

## 🛠️ Tech Stack
- **Framework:** .NET 8 (Web API)
- **Database:** MS SQL Server
- **ORM:** Entity Framework Core
- **Hardware Integration:** SignalR for Real-time communication with ESP32.
- **AI Integration:** HuskyLens (Face Recognition).

## Folder Structure

    ├── MedicalRobot.Core/          # Entities, Enums, Interfaces
    ├── MedicalRobot.Application/   # Services, DTOs, Mapping, Validators
    ├── MedicalRobot.Infrastructure/# Data Context, Migrations, Repositories
    └── MedicalRobot.Api/           # Controllers, Hubs, Program.cs