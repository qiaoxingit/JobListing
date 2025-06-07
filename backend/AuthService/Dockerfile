FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY . .
RUN dotnet restore ./AuthService/AuthService.csproj
RUN rm appsettings.json
RUN mv appsettings-docker.json appsettings.json
RUN dotnet publish ./AuthService/AuthService.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

# Set the entrypoint
ENTRYPOINT ["dotnet", "AuthService.dll"]