# Project "Your Rest"

## Архитектура (./_docs/architecture.md)

Проект основан на паттерне **Порты и Адаптеры**.
Состоит из двух основных частей:
1. **YourRest.HotelManagementWebApi** - серверная часть системы отельеров.
2. **YourRest.WebApi** - серверная часть системы путешественников.

## Запуск тестов

Для запуска всех тестов используйте команду:

```
dotnet test

```
- **YourRest.WepApi.Tests** - интеграционные тесты.

## Документация в Swagger

export ASPNETCORE_ENVIRONMENT=Development (Linux)
setx ASPNETCORE_ENVIRONMENT "Development" (Windows)

dotnet run

Доступ к документации по API: [Swagger](http://localhost:5201/swagger/index.html)

## База данных

База данных запускается в Docker контейнере с помощью команды:

```
docker compose up -d
```
В интеграционных тестах используется отдельная тестовая база, которая очищается после каждого выполненного теста. При добавлении новых сущностей необходимо обновить метод `ClearAllTables()` в `SharedDbContext`, чтобы очищать соответствующую таблицу. Если одна таблица ссылается на другую, следите за порядком удаления, учитывая foreign key.

## Добавление миграций
## Если выполнять из директории YourRest\Infrastructure:
## Это для SharedDbContext
Windows:
dotnet ef migrations add InitialCreate -s ..\YourRest.WebApi\YourRest.WebApi.csproj -c SharedDbContext -p YourRest.Producer.Infrastructure\YourRest.Producer.Infrastructure.csproj -v

dotnet ef database update -s ..\YourRest.WebApi\YourRest.WebApi.csproj -p YourRest.Producer.Infrastructure\YourRest.Producer.Infrastructure.csproj -c SharedDbContext -v

Linux:
dotnet ef migrations add AddRoomFacilities -s ../YourRest.WebApi/YourRest.WebApi.csproj -c SharedDbContext -p YourRest.Producer.Infrastructure/YourRest.Producer.Infrastructure.csproj -v
dotnet ef database update -s ../YourRest.WebApi/YourRest.WebApi.csproj -p YourRest.Producer.Infrastructure/YourRest.Producer.Infrastructure.csproj -c SharedDbContext -v