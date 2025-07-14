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

RUN addgroup --gid 1001 appgroup 
RUN adduser --uid 1001 -G appgroup --disabled-password -h /app appuser
RUN chown -R appuser:appgroup /app
USER appuser

ENV ASPNETCORE_URLS=http://+:80
ENTRYPOINT ["dotnet", "TodoApi.dll"]
