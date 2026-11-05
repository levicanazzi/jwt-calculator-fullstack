# 🧮 Full Stack Calculator with JWT Authentication

A full stack application built as a **technical assessment for Threeo**: a login-protected web calculator with a **REST API in ASP.NET Core 6** secured by **JWT (access + refresh tokens)** and a **React 18 frontend**, all orchestrated with **Docker Compose**.

---

## ✨ Features

- 🔐 **Login** that issues an *access token* and a *refresh token* (JWT, HMAC-SHA256)
- 🔄 **Token renewal** through a refresh endpoint
- ➕ **Operations**: addition, subtraction, multiplication and division, computed on the backend
- 🛡️ **Protected route** on the frontend: the calculator is only reachable after authentication
- 📄 **Swagger** available in the development environment
- 📬 **Postman collection** included for testing the API on its own
- 🐳 **Docker Compose** starts frontend and backend with a single command

---

## 🛠️ Tech Stack

| Layer     | Stack |
|-----------|-------|
| Backend   | .NET 6 · ASP.NET Core Web API · JwtBearer · Swashbuckle (Swagger) |
| Frontend  | React 18 · React Router 6 · Create React App |
| Infra     | Docker · Docker Compose |

---

## 📁 Project Structure

```
.
├── docker-compose.yml
├── ThreeoBack/                      # Backend (.NET 6)
│   ├── ThreeoBack.sln
│   └── ThreeoAPI/
│       ├── Controllers/
│       │   ├── AuthController.cs        # login and refresh-token
│       │   └── CalculatorController.cs  # operations (protected route)
│       ├── Services/
│       │   ├── AuthServices/            # JWT generation/validation
│       │   └── CalculateServices/       # operation logic
│       ├── Models/                      # LoginModel, requests and responses
│       ├── appsettings.json             # JWT keys and expiration
│       ├── ThreeoAPICalls.postman_collection.json
│       └── dockerfile
└── ThreeoFront/                     # Frontend (React)
    └── threeo-v1/
        ├── src/
        │   ├── App.js                   # routes and token handling
        │   └── components/
        │       ├── Login.js
        │       ├── Calculator.js
        │       └── styles.css
        └── dockerfile
```

---

## 🚀 Getting Started

### With Docker (recommended)

Requirement: [Docker](https://www.docker.com/) with Docker Compose.

From the repository root:

```bash
docker-compose up --build
```

| Service  | URL |
|----------|-----|
| Frontend | http://localhost:3000 |
| Backend  | http://localhost:8080 |

> ⚠️ **Linux/macOS:** `docker-compose.yml` references `Dockerfile` (capital D), but the files in the repository are named `dockerfile`. This works on Windows; on case-sensitive systems, rename the files to `Dockerfile` or update the compose file.

### Without Docker

**Backend** (requires the [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)):

```bash
cd ThreeoBack/ThreeoAPI
dotnet restore
dotnet run
```

Swagger is available at `/swagger` in the development environment. The frontend expects the API at `http://localhost:8080`, so adjust the port in `Properties/launchSettings.json` if needed.

**Frontend** (requires [Node.js 18+](https://nodejs.org/)):

```bash
cd ThreeoFront/threeo-v1
npm install
npm start
```

---

## 🔑 Login Credentials

| Username | Password    |
|----------|-------------|
| `admin`  | `admin2024` |

> These credentials are hardcoded in `AuthController` for demonstration purposes only.

---

## 📡 API Endpoints

### `POST /api/auth/login`

```json
// Request
{ "username": "admin", "password": "admin2024" }

// Response 200
{
  "accessToken": "eyJhbGciOi...",
  "expiresIn": 899,
  "refreshToken": "eyJhbGciOi..."
}
```

Returns `401 Unauthorized` for invalid credentials.

### `POST /api/auth/refresh-token`

```json
// Request
{ "username": "admin", "refreshToken": "eyJhbGciOi..." }
```

Returns a new token pair in the same format as the login response.

### `POST /api/calculator/calculate` 🔒

Requires the header `Authorization: Bearer <accessToken>`.

```json
// Request
{ "firstValue": 10, "secondValue": 5, "operation": "sum" }

// Response 200
{ "firstValue": 10, "secondValue": 5, "operation": "sum", "result": 15 }
```

Supported operations: `sum`, `subtraction`, `multiplication`, `division`. An invalid operation returns `400 Bad Request`.

### JWT Configuration

Set in `ThreeoAPI/appsettings.json`:

| Key | Default |
|-----|---------|
| `AccessTokenExpirationMinutes` | 15 |
| `RefreshTokenExpirationMinutes` | 30 |

---

## 🧪 Testing with Postman

Import `ThreeoBack/ThreeoAPI/ThreeoAPICalls.postman_collection.json` into Postman. Log in, copy the returned `accessToken`, and use it in the `Authorization` header of the calculator requests.

---

## 🔭 Future Improvements

- Move JWT keys to environment variables / User Secrets (currently in `appsettings.json`)
- Replace hardcoded credentials with a real user store
- Handle division by zero on the backend (it currently returns `Infinity`)
- Use the refresh token automatically on the frontend when the access token expires
- Make the API URL configurable on the frontend through an environment variable
- Add unit tests for `CalculatorService` and `TokenService`