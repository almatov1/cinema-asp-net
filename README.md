# Cinema ASP.NET

ASP.NET 10 Cinema API with JWT Authentication, Role-based Authorization, Swagger, Clean Architecture, Redis Caching, and Paginated Sessions.

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
REDIS_HOST=localhost
REDIS_PORT=6379
```

3. **Start services using Docker Compose**

```bash
docker-compose up -d
```

This will start all required services, such as the database and Redis.

4. **Run Cinema API**

```bash
cd cinema.api
dotnet run
```

The API will be available at `http://localhost:5258`.

---

## Features

### Authentication & Authorization

* JWT Authentication
* Role-based Authorization Middleware
* Registration, Login, Logout
* Refresh Tokens

### Cinema Management

* Manager can create new sessions
* Users can book seats for sessions
* Admin can view all users

### Viewing & Filtering

* List all sessions with pagination and optional filters (`movie_title`, date range)
* Users can view their own bookings
* Managers can view bookings for their sessions

### Caching & Performance

* Redis caching for session lists with pagination
* Versioning for safe cache invalidation when sessions are added or updated
* Optimized queries using indexes for sessions and bookings

### Development & Testing

* Swagger UI for API exploration and testing
* Dockerized environment with PostgreSQL and Redis
* Clean Architecture separation (API / Application / Domain / Infrastructure)

---

## API Endpoints (Summary)

| Role    | Endpoint                                | Description                                   |
| ------- | --------------------------------------- | --------------------------------------------- |
| Any     | `GET /api/sessions`                     | List sessions (supports pagination & filters) |
| Manager | `POST /api/sessions`                    | Create a new session                          |
| User    | `POST /api/bookings`                    | Create a booking                              |
| User    | `GET /api/bookings/me`                  | List own bookings                             |
| Manager | `GET /api/bookings/session/{sessionId}` | List bookings for a specific session          |
| Admin   | `GET /api/users`                        | List all users                                |

---

## Swagger

Access Swagger UI at:

```
http://localhost:5258/swagger
```

Use it to explore and test API endpoints.

---

## Notes

* Ensure `.env` variables match your Docker setup.
* Redis is used for caching session lists — it must be running for cache features.
* To rebuild Docker containers if needed:

```bash
docker-compose down
docker-compose up --build -d
```

---

## License

This project is licensed under the [MIT license](LICENSE).
