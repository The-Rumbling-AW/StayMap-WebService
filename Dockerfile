# Imagen oficial del SDK de .NET 9.0.302
FROM mcr.microsoft.com/dotnet/sdk:9.0.302

# Crear directorio de la app
WORKDIR /app

# Copiar el código al contenedor
COPY . .

# Restaurar dependencias
RUN dotnet restore

# Compilar el proyecto
RUN dotnet build -c Release --no-restore

# Publicar el proyecto para producción
RUN dotnet publish -c Release -o out --no-restore

# Establecer el directorio donde se ejecutará la app
WORKDIR /app/out

# Ejecutar la aplicación
ENTRYPOINT ["dotnet", "backendAppsWeb.dll"]
