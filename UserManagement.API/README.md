# UserManagement.API (.NET 8)

Simple ASP.NET Core Web API for learning CI/CD. No NuGet database dependencies. Stores users in `data/users.json` (created at first run). This is **practice storage**, not a production data store.

## Run
Install [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0), then in this folder:

```powershell
dotnet restore
dotnet run --launch-profile http
```

API base URL: `http://localhost:5000`.

## Endpoints
- `GET /health`
- `GET /api/users`
- `GET /api/users/{id}`
- `POST /api/users` (201, or 400/409)
- `PUT /api/users/{id}` (200, or 400/404/409)
- `DELETE /api/users/{id}` (204 or 404)

Request body for POST/PUT:
```json
{ "name": "Sam Lee", "email": "sam@example.com", "department": "Engineering" }
```

Test with PowerShell:
```powershell
Invoke-RestMethod http://localhost:5000/api/users
Invoke-RestMethod -Uri http://localhost:5000/api/users -Method Post -ContentType 'application/json' -Body '{"name":"Sam Lee","email":"sam@example.com","department":"Engineering"}'
```

## CI/CD
`azure-pipelines.yml` restores, builds, and publishes an artifact on a Microsoft-hosted Windows agent. For IIS deployment, install the ASP.NET Core Hosting Bundle on your Windows Server and configure a site/app pool and deployment stage. Do not overwrite `data/users.json` during deployments if you want to keep the practice data. Use a database for real applications.

CORS currently allows the Vite development origin (`http://localhost:5173`). Update allowed origins for your deployed React site.
