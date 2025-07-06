# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ContosoPizza.Web/*.csproj ./ContosoPizza.Web/
COPY ContosoPizza.sln .
RUN dotnet restore "ContosoPizza.Web/ContosoPizza.csproj"

COPY . .
WORKDIR /src/ContosoPizza.Web
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ContosoPizza.dll"]
