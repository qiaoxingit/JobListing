FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY . .
RUN dotnet restore ./JobService/JobService.csproj
RUN rm appsettings.json
RUN mv appsettings-docker.json appsettings.json
RUN dotnet publish ./JobService/JobService.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

# Set the entrypoint
CMD ["dotnet", "JobService.dll"]