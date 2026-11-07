# Proyecto Final PPE - Películas

API de películas en **ASP.NET Core 8** que reemplaza el backend de NestJS del
Taller 1, manteniendo el mismo contrato para el cliente **Vue 3** que se usó en
ese taller. Base de datos **SQLite** con Entity Framework Core y migraciones.

## Estructura

- `ProyectoFinalPPE/` - API ASP.NET Core (controladores, servicios, DTOs, EF Core).
- `front-vue-peliculas/` - Cliente Vue 3 + TypeScript (el mismo del Taller 1).

## Requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Node.js](https://nodejs.org/) 18 o superior

## Levantar la API

```bash
cd ProyectoFinalPPE/ProyectoFinalPPE
dotnet restore
dotnet ef database update   # crea peliculas.db con las migraciones
dotnet run
```

Si no tienes la herramienta `dotnet ef`:

```bash
dotnet tool install --global dotnet-ef
```

Con el perfil **https** la API queda en `https://localhost:7178` (y
`http://localhost:5097` con el perfil http). Si el navegador o el cliente Vue
rechazan el certificado de desarrollo:

```bash
dotnet dev-certs https --trust
```

### Documentación interactiva (OpenAPI)

En modo Development, Swagger UI queda en `https://localhost:7178/swagger`.
Para probar los endpoints protegidos: haz login en `POST /auth/login`, copia el
`access_token`, pulsa **Authorize** y pega el token. Después podrás ejecutar
`POST`, `PATCH` y `DELETE /peliculas` desde la interfaz.

## Levantar el cliente Vue

```bash
cd front-vue-peliculas
cp .env.example .env   # VITE_API_URL debe apuntar a la API
npm install
npm run dev
```

El cliente queda en `http://localhost:5173`. El `.env.example` ya apunta a
`https://localhost:7178`; si corres la API en otro puerto, cambia solo
`VITE_API_URL` en tu `.env` (no hace falta tocar el código del cliente).

## Uso

1. Regístrate en `POST /auth/register` (o desde la pantalla de registro del Vue).
2. Inicia sesión en `POST /auth/login`; el cliente guarda el token y lo envía
   como `Authorization: Bearer <token>` en cada petición.
3. Listar películas (`GET /peliculas`) es público; **crear, editar y eliminar
   requieren token** (responden `401` sin él).
4. `GET /peliculas` acepta `?search=` (buscar por nombre) y
   `?page=&pageSize=` (paginación), igual que la API original de NestJS.

## Notas

- La clave JWT de `appsettings.json` es solo un ejemplo para pruebas locales;
  en un despliegue real debe guardarse fuera del código.
- `peliculas.db` y el `.env` real están en `.gitignore` y no se suben al repo.
