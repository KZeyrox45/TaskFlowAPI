# TaskFlowAPI

TaskFlowAPI is a layered ASP.NET Core 8 REST API for task management with JWT authentication, PostgreSQL persistence, and Docker support.

## Tech Stack

- ASP.NET Core 8
- Entity Framework Core
- PostgreSQL
- JWT Bearer Authentication
- xUnit + Moq
- Docker + Docker Compose

## Project Structure

- `TaskFlowAPI.API`: Controllers, authentication pipeline, app bootstrap
- `TaskFlowAPI.Application`: Service layer (`AuthService`, `TaskService`)
- `TaskFlowAPI.Core`: Domain entities and repository interfaces
- `TaskFlowAPI.Infrastructure`: EF Core DbContext and repository implementations
- `TaskFlowAPI.Tests`: Unit tests

## Environment Variables

Create a `.env` file in the `TaskFlowAPI` root:

```env
POSTGRES_USER=postgres
POSTGRES_PASSWORD=your_local_password
POSTGRES_DB=TaskFlowDb
JWT_KEY=your_long_random_secret_key
JWT_ISSUER=TaskFlowAPI
JWT_AUDIENCE=TaskFlowAPI_Users
```

Notes:
- Define `POSTGRES_PASSWORD` and `JWT_KEY` yourself for local development.
- Keep `.env` private and never commit it.

## Run Locally (without Docker)

1. Make sure PostgreSQL is running.
2. Update `appsettings.Development.json` or environment variables for connection string/JWT.
3. Start API:

```bash
dotnet run --project TaskFlowAPI.API
```

Swagger:
- `http://localhost:5085/swagger` (port may vary by launch profile)

## Run with Docker

From `TaskFlowAPI` directory:

```bash
docker-compose up -d --build
```

API endpoints:
- Swagger: `http://localhost:8080/swagger`
- Base API: `http://localhost:8080/api`

Stop containers:

```bash
docker-compose down
```

Reset database volume (if you changed env/password/init and want a fresh DB):

```bash
docker-compose down -v
docker-compose up -d --build
```

## Run Tests

```bash
dotnet test TaskFlowAPI.Tests
```
