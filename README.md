# Palladium Parking API

API REST construida con **.NET 8** y **SQL Server** para gestionar un sistema de parqueo: vehículos, sesiones de parqueo, tarifas, caja registradora, suscripciones y autenticación JWT con refresh tokens.

## Arquitectura

```
Parking.API/
 Program.cs                          # Configuración del pipeline y DI
 scr/
 Core/                           # Casos de uso y controladores
 Security/Users/             # Autenticación (login, refresh, revoke)
 VehicleUseCase/             # CRUD de vehículos
 RateUseCase/                # CRUD de tarifas
 ParkingSessionUseCase/      # Sesiones de parqueo
 CashRegisterUseCase/        # Caja registradora
 SubscriptionUseCase/        # Suscripciones
 Infrastructure/                 # Persistencia y servicios
 Persistence/                # DbContext, repositorios, seed data
 services/                   # Token, password hasher, datetime
 Shared/                         # Entidades, interfaces, DTOs
 Entities/                   # Modelos de base de datos
 Interfaces/                 # Contratos
 Configurations/             # AppSettings
 TestParking/                        # Pruebas unitarias
```

## Requisitos

- .NET 8 SDK
- SQL Server
- Variables de entorno gestionadas con `DotEnv.Core`

## Configuración rápida

1. Crea un archivo `.env` en la raíz del proyecto `Parking/`:

```env
ASPNETCORE_ENVIRONMENT=Development
ACCESS_TOKEN_KEY=tu-clave-secreta-larga-minimo-32-caracteres
ACCESS_TOKEN_EXPIRES=15
REFRESH_TOKEN_EXPIRES=7
```

2. Configura la connection string en `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=PalladiumParking;Integrated Security=True;TrustServerCertificate=True;"
}
```

3. Ejecutar:

```bash
dotnet restore
dotnet ef database update --project Parking
dotnet run --project Parking
```

Swagger disponible en desarrollo: `http://localhost:5050/swagger`

## Endpoints

Todas las rutas están prefijadas con `/api`.

### Autenticación (`/api/User`)

| Método | Ruta | Auth | Descripción |
|--------|------|------|-------------|
| `POST` | `/User` | ?? JWT | Crear usuario |
| `POST` | `/User/login` | ?? | Login (devuelve access + refresh token) |
| `POST` | `/User/refresh` | ?? | Renovar tokens |
| `POST` | `/User/revoke` | ?? | Cerrar sesión (revocar refresh token) |
| `GET` | `/User` | ?? JWT | Listar usuarios |
| `DELETE` | `/User/{id}` | ?? JWT | Eliminar usuario |

### Vehículos (`/api/vehicles`)

| Método | Ruta | Auth | Descripción |
|--------|------|------|-------------|
| `POST` | `/vehicles` | ?? JWT | Crear vehículo |
| `GET` | `/vehicles` | ?? JWT | Listar vehículos |
| `GET` | `/vehicles/{id}` | ?? JWT | Obtener por ID |
| `PUT` | `/vehicles/{id}` | ?? JWT | Actualizar |
| `DELETE` | `/vehicles/{id}` | ?? JWT | Eliminar |

### Tarifas (`/api/rates`)

| Método | Ruta | Auth | Descripción |
|--------|------|------|-------------|
| `POST` | `/rates` | ?? JWT | Crear tarifa |
| `GET` | `/rates` | ?? | Listar tarifas |
| `PUT` | `/rates/{id}` | ?? JWT | Actualizar precio |
| `DELETE` | `/rates/{id}` | ?? JWT | Desactivar tarifa |

### Sesiones de Parqueo (`/api/ParkingSeassion`)

| Método | Ruta | Auth | Descripción |
|--------|------|------|-------------|
| `POST` | `/ParkingSeassion` | ?? JWT | Abrir sesión |
| `POST` | `/ParkingSeassion/CloseParkingSession` | ?? JWT | Cerrar y cobrar |
| `GET` | `/ParkingSeassion/GetParkingsession` | ?? JWT | Sesiones abiertas |
| `GET` | `/ParkingSeassion/history` | ?? JWT | Historial completo |
| `GET` | `/ParkingSeassion/{id}` | ?? JWT | Detalle |
| `POST` | `/ParkingSeassion/{id}/cancel` | ?? JWT | Cancelar sesión |

### Caja Registradora (`/api/CashRegister`)

| Método | Ruta | Auth | Descripción |
|--------|------|------|-------------|
| `POST` | `/CashRegister/open` | ?? JWT | Abrir caja |
| `POST` | `/CashRegister/close` | ?? JWT | Cerrar caja |
| `GET` | `/CashRegister/history` | ?? JWT | Historial de cajas |

### Suscripciones (`/api/subscriptions`)

| Método | Ruta | Auth | Descripción |
|--------|------|------|-------------|
| `POST` | `/subscriptions` | ?? JWT | Crear suscripción |
| `GET` | `/subscriptions` | ?? JWT | Listar todas |
| `GET` | `/subscriptions/active` | ?? JWT | Solo activas |
| `GET` | `/subscriptions/check/{plate}` | ?? JWT | Verificar placa |
| `GET` | `/subscriptions/prices` | ?? | Ver precios |
| `POST` | `/subscriptions/{id}/cancel` | ?? JWT | Cancelar |

## Seguridad

- **JWT Access Token**: expira en 15 minutos
- **Refresh Token**: expira en 7 días, rotación automática, hash SHA256 en BD
- **Replay Detection**: si se reutiliza un refresh token revocado, se cierran todas las sesiones
- **Rate Limiting**: 100 req/min global, 10 req/min en endpoints de auth
- **Security Headers**: X-Content-Type-Options, X-Frame-Options, X-XSS-Protection
- **CORS restringido**: solo orígenes configurados
- **SQL Injection**: protegido por Entity Framework (queries parametrizadas)
- **Request Body Limit**: máximo 1 MB

## Tarifas por defecto

| Tipo | Precio/hora |
|------|-------------|
| Carro | ?1,000 |
| Moto | ?800 |

## Precios de Suscripción

| Tipo | Día | Quincena | Mes |
|------|-----|----------|-----|
| Carro | ?3,000 | ?25,000 | ?40,000 |
| Moto | ?3,000 | ?20,000 | ?35,000 |

## Pruebas

```bash
dotnet test
```

## Despliegue (VPS Linux)

```bash
# Publicar
dotnet publish Parking.API.csproj -c Release -o /var/www/parkingapi/publish

# Servicio systemd
systemctl start parkingapi
systemctl status parkingapi

