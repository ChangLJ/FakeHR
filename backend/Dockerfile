FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY HumanResource.Api.csproj .
RUN dotnet restore
COPY . .
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# SQLite 資料庫目錄（Cloud Run 可掛載 Volume 至 /app/data）
RUN mkdir -p /app/data
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://0.0.0.0:8080
ENV DATABASE_PATH=/app/data/humanresource.db

# 容器內監聽 8080，對外映射見 docker-compose（5001:8080）
EXPOSE 8080

ENTRYPOINT ["dotnet", "HumanResource.Api.dll"]
