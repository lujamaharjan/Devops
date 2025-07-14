# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app
COPY ./TodoApi/TodoApi.csproj ./TodoApi.csproj
RUN dotnet restore

COPY . ./
WORKDIR /app/TodoApi
RUN dotnet publish -c Release -o /app/out

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

RUN useradd --shell /bin/bash appuser
RUN chown -R appuser:appuser /app
USER appuser

ENV ASPNETCORE_URLS=http://+:80
ENTRYPOINT ["dotnet", "TodoApi.dll"]
