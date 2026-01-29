# Cinema ASP.NET

ASP.NET 10 Cinema API with JWT Authentication, Role-based Authorization, Swagger, and Clean Architecture.

## Prerequisites

* [.NET 10 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)
* [Docker & Docker Compose](https://www.docker.com/products/docker-desktop)
* Git

## Installation

1. **Clone the repository**

```bash
git clone <your-repo-url>
cd <your-repo-folder>
```

2. **Create `.env` file**

Create a `.env` file in the root directory with required environment variables. Example:

```env
JWT_SECRET=your_secret_key
DB_CONNECTION=your_database_connection_string
```

3. **Start services using Docker Compose**

```bash
docker-compose up -d
```

This will start all required services, such as the database.

4. **Run Cinema API**

```bash
cd cinema.api
dotnet run
```

The API will be available at `http://localhost:5258`.

## Features

* JWT Authentication
* Role-based Authorization Middleware
* Registration, Login, Logout
* Swagger for API testing
* Clean Architecture (API / Application layers)
* Dockerized environment

## Swagger

Access Swagger UI at:

```
http://localhost:5258/swagger
```

Use it to explore and test API endpoints.

## Notes

* Ensure `.env` variables match your Docker setup.
* To rebuild Docker containers if needed:

```bash
docker-compose down
docker-compose up --build -d
```
