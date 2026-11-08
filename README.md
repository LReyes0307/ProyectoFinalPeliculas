# Proyecto Final PPE - Películas

API de películas en **ASP.NET Core 9** que reemplaza el backend de NestJS del
Taller 1, manteniendo el mismo contrato para el cliente **Vue 3** que se usó en
ese taller. Base de datos **SQLite** con Entity Framework Core y migraciones.
La autenticación usa **JWT** y la documentación se genera con **OpenAPI**
(`Microsoft.AspNetCore.OpenApi`) con la interfaz interactiva **Scalar**.

## Estructura

- `ProyectoFinalPPE/` - API ASP.NET Core (controladores, servicios, DTOs, EF Core).
- `front-vue-peliculas/` - Cliente Vue 3 + TypeScript (el mismo del Taller 1).

## Requisitos

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
  (se verifica con `dotnet --version`, debe mostrar `9.0.x`)
- [Node.js](https://nodejs.org/) 18 o superior

## Levantar la API

```bash
cd ProyectoFinalPPE/ProyectoFinalPPE
dotnet restore
dotnet ef database update   # crea peliculas.db con las migraciones
dotnet run --launch-profile http
```

Si no tienes la herramienta `dotnet ef`:

```bash
dotnet tool install --global dotnet-ef
```

Si ya la tienes instalada con una versión anterior, actualízala:

```bash
dotnet tool update --global dotnet-ef --version 9.*
```

Con el perfil **http** la API queda en `http://localhost:5097`. Con el perfil
**https** queda en `https://localhost:7178` (y `http://localhost:5097`). Si el
navegador o el cliente Vue rechazan el certificado de desarrollo:

```bash
dotnet dev-certs https --trust
```

### Documentación interactiva (OpenAPI)

El documento OpenAPI se genera con `Microsoft.AspNetCore.OpenApi` y queda
disponible solo en modo Development:

- Documento JSON: `http://localhost:5097/openapi/v1.json`
- Interfaz interactiva (Scalar): `http://localhost:5097/scalar/v1`

Con el perfil https, las mismas rutas están en `https://localhost:7178`.

Para probar los endpoints protegidos:

1. Registra un usuario en `POST /auth/register`.
2. Haz login en `POST /auth/login` y copia el valor de `access_token`.
3. En la sección de autenticación **Bearer** de Scalar, pega el token (sin la
   palabra `Bearer`).
4. Ya puedes ejecutar `POST`, `PATCH` y `DELETE` sobre `/peliculas`. Sin el
   token responden `401`.

## Levantar el cliente Vue

```bash
cd front-vue-peliculas
cp .env.example .env   # VITE_API_URL debe apuntar a la API
npm install
npm run dev
```

El cliente queda en `http://localhost:5173`. El `.env.example` ya apunta a
`https://localhost:7178`; si corres la API con el perfil http, pon
`VITE_API_URL=http://localhost:5097` en tu `.env`. Solo se cambia esa variable,
no hace falta tocar el código del cliente.

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
- Las contraseñas se guardan hasheadas con BCrypt, nunca en texto plano.
- `peliculas.db` y el `.env` real están en `.gitignore` y no se suben al repo.
- La documentación (`/openapi/v1.json` y `/scalar/v1`) solo se expone en
  Developmen

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
