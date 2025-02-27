# Sistema de Gestión de Empleados

Aplicación desarrollada en ASP.NET Core 8 con SQL Server para la gestión de empleados, siguiendo una arquitectura en capas y utilizando buenas prácticas de desarrollo.

## Estructura del Proyecto

La solución está organizada con una arquitectura en capas:

- **EmpleadosAPI**: API REST que expone endpoints para la gestión de empleados
- **EmpleadosMVC**: Aplicación MVC que consume la API
- **Empleados.Models**: Biblioteca de clases que contiene los modelos de dominio
- **Empleados.DataAccess**: Capa de acceso a datos con Entity Framework Core
- **Empleados.Business**: Capa de lógica de negocio

## Características Implementadas

- ✅ API REST completa con endpoints para operaciones CRUD
- ✅ Aplicación MVC con interfaz de usuario amigable
- ✅ Autenticación con ASP.NET Core Identity
- ✅ Autorización con JWT para la API
- ✅ Paginación y búsqueda de empleados
- ✅ Validaciones del lado del cliente y del servidor
- ✅ Manejo de errores y excepciones
- ✅ Estructura de proyecto en capas con inyección de dependencias

## Requisitos Previos

Para ejecutar el proyecto, necesitas:

- .NET Core 8 SDK
- SQL Server (Express o superior)
- Internet Information Services (IIS) para despliegue en producción

## Instalación y Configuración

### Configuración de la Base de Datos

1. Crear las bases de datos necesarias:

```sql
CREATE DATABASE EmpleadosDB;
CREATE DATABASE EmpleadosIdentityDB;
```

2. En los archivos appsettings.json de EmpleadosAPI y EmpleadosMVC, configurar las cadenas de conexión según tu entorno:
```
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=EmpleadosDB;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true",
  "IdentityConnection": "Server=localhost\\SQLEXPRESS;Database=EmpleadosIdentityDB;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
}
```
3. Aplicar las migraciones de Entity Framework Core:
```
cd EmpleadosAPI
dotnet ef database update --project ../Empleados.DataAccess/Empleados.DataAccess.csproj

cd ../EmpleadosMVC
dotnet ef database update --context ApplicationDbContext
```
## Despliegue en IIS
### Requisitos del Servidor

- Windows Server 2016 o superior
- IIS 10 o superior
- ASP.NET Core Hosting Bundle 8.0
- URL Rewrite Module para IIS

### Paso 1: Instalar los componentes necesarios

- Instalar IIS:

  - Abrir Panel de control > Programas > Activar o desactivar características de Windows
  - Seleccionar "Internet Information Services" y sus subcomponentes básicos


- Instalar el ASP.NET Core Hosting Bundle:

  - Descargar desde Microsoft .NET Download Page
  - Ejecutar el instalador con permisos de administrador


- Instalar URL Rewrite Module:

  - Descargar desde IIS URL Rewrite
  - Ejecutar el instalador con permisos de administrador

### Paso 2: Configurar sitios web en IIS

- Para la API:

  - Abrir IIS Manager
  - Crear un nuevo sitio web:
  - Nombre del sitio: EmpleadosAPI
  - Ruta física: C:\inetpub\wwwroot\EmpleadosAPI (o la ruta donde hayas copiado los archivos publicados)
  - Puerto: 5000 (puedes elegir otro puerto disponible)
    
- Configurar el Application Pool:
  - Crear un nuevo Application Pool llamado "EmpleadosAPIPool"
  - .NET CLR version: No Managed Code
  - Managed pipeline mode: Integrated

- Para la aplicación MVC:
  - Crear un nuevo sitio web:
  - Nombre del sitio: EmpleadosMVC
  - Ruta física: C:\inetpub\wwwroot\EmpleadosMVC (o la ruta donde hayas copiado los archivos publicados)
  - Puerto: 5001 (puedes elegir otro puerto disponible)

- Configurar el Application Pool:
  - Crear un nuevo Application Pool llamado "EmpleadosMVCPool"
  - .NET CLR version: No Managed Code
  - Managed pipeline mode: Integrated

### Paso 3: Copiar archivos publicados

  - Copiar los archivos publicados de la API:
  - Copiar todo el contenido de la carpeta EmpleadosAPI/publish a C:\inetpub\wwwroot\EmpleadosAPI
  - Copiar los archivos publicados de la aplicación MVC:
  - Copiar todo el contenido de la carpeta EmpleadosMVC/publish a C:\inetpub\wwwroot\EmpleadosMVC

- Configurar los permisos:
  - Dar permisos de lectura y ejecución al usuario IIS_IUSRS en ambas carpetas

### Paso 4: Configurar web.config
Los archivos web.config se generan automáticamente durante la publicación, pero puede ser necesario ajustarlos. Verifica que contengan la configuración adecuada para variables de entorno, módulos y handlers.
Paso 5: Actualizar la configuración de la aplicación
En el appsettings.json de EmpleadosMVC, actualiza la URL de la API para que apunte a la URL del sitio web de la API en IIS:
```
"ApiSettings": {
  "BaseUrl": "http://localhost:5000"
}
```
### Paso 6: Verificar la instalación
- Navegar a http://localhost:5000/swagger para verificar que la API esté funcionando
- Navegar a http://localhost:5001 para verificar que la aplicación MVC esté funcionando

### Capturas de Pantalla de la aplicación
![image](https://github.com/user-attachments/assets/9d49636a-77eb-4072-b324-b61cf3efd606)
![image](https://github.com/user-attachments/assets/c688cb7c-cd6a-40a6-af27-8faf4fbc03be)
![image](https://github.com/user-attachments/assets/d4f619de-a009-4cf7-921a-7d119bc23130)

### Autor
Edwin Zuniga - Versión 1.0 - Febrero 2025
