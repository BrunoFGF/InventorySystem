# Inventory System

Sistema web para la gestión de inventario de productos, desarrollado con **.NET 8** en el backend y **Angular 21** en el frontend.

## ¿Qué hace?

Permite a los usuarios autenticados gestionar su inventario de productos, asociando cada producto a uno o más proveedores con información de precio, stock y número de lote. Todas las operaciones quedan registradas en un log de auditoría.

Funcionalidades principales:
- Autenticación con JWT
- CRUD completo de productos
- Asociación de múltiples proveedores por producto
- Auditoría automática de cambios (creación, edición, eliminación)
- Paginación y diseño responsivo

---

## Arquitectura

El proyecto sigue una **arquitectura en capas** (Clean Architecture simplificada), con separación clara de responsabilidades:

```
IS.Api           → Controladores REST, configuración JWT, middleware global de errores
IS.Application   → Servicios, interfaces, DTOs
IS.Infrastructure → DbContext (EF Core), repositorios, migraciones, seeder
IS.Domain        → Entidades, interfaces de repositorio, excepciones de dominio
IS.Shared        → Wrapper ApiResponse<T>, constantes, mensajes de error
```

**Flujo de dependencias:** `Api → Application → Domain ← Infrastructure`

---

## Patrones de diseño

| Patrón | Dónde se aplica |
|---|---|
| **Repository + Unit of Work** | `IGenericRepository<T>`, repositorios especializados, `IUnitOfWork` como punto único de acceso a datos |
| **DTO** | Separación entre entidades de dominio y datos transferidos por la API |
| **Middleware** | `GlobalExceptionMiddleware` centraliza el manejo de errores y logging |
| **Interceptor / Guard** | `authInterceptor` adjunta el token JWT; `errorInterceptor` maneja errores HTTP globales; `authGuard` protege rutas |
| **Soft Delete** | Los productos usan `IsDeleted` — nunca se eliminan físicamente |
| **Options Pattern** | `JwtSettings` se enlaza desde `appsettings.json` vía `IOptions<JwtSettings>` para configuración fuertemente tipada |

---

## Base de datos

Se realizó un análisis de la problemática y se diagramó un **Modelo Entidad-Relación (MER)** como punto de partida:

![MER preview](images/Diagrama MER.jpg)

A partir del MER se definieron las entidades del dominio y sus relaciones:

- `User` → `Product` (un usuario gestiona muchos productos)
- `Product` ↔ `Supplier` a través de `ProductSupplier` (tabla intermedia con `Price`, `Stock`, `BatchNumber`)
- `AuditLog` registra cada cambio con valor anterior y nuevo

Las migraciones se gestionan con **Entity Framework Core**. La base de datos se crea y pobla automáticamente al levantar la API por primera vez.

---

## Stack tecnológico

### Backend

| Tecnología / Librería | Versión |
|---|---|
| .NET | 8.0 |
| Microsoft.AspNetCore.Authentication.JwtBearer | 8.0.25 |
| Microsoft.EntityFrameworkCore | 8.0.25 |
| Microsoft.EntityFrameworkCore.SqlServer | 8.0.25 |
| Microsoft.EntityFrameworkCore.Tools | 8.0.25 |
| Microsoft.Extensions.DependencyInjection.Abstractions | 8.0.2 |
| BCrypt.Net-Next | 4.1.0 |
| Serilog.AspNetCore | 8.0.3 |
| Serilog.Sinks.File | 6.0.0 |
| Swashbuckle.AspNetCore | 6.5.0 |

### Frontend

| Tecnología / Librería | Versión |
|---|---|
| Node.js | 18+ |
| npm | 11.11.0 |
| Angular | 21.2.0 |
| Angular Material / CDK | 21.2.5 |
| Angular CLI | 21.2.6 |
| TypeScript | 5.9.2 |
| RxJS | 7.8.0 |
| Vitest | 4.0.8 |
| Prettier | 3.8.1 |

---

## Cómo ejecutar

### Requisitos
- .NET 8 SDK
- SQL Server (instancia local)
- Node.js 18+

### Backend
```bash
cd Backend/IS.Api
dotnet run
# API disponible en https://localhost:7088
# Swagger en https://localhost:7088/swagger
```

### Frontend
```bash
cd Frontend/inventory-system
npm install
ng serve
# App disponible en http://localhost:4200
```

### Usuario de prueba
| Campo | Valor |
|---|---|
| Email | `admin@inventory.com` |
| Contraseña | `Admin123*` |
