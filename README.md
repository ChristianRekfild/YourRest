# Project "Your Rest"

## Архитектура (./_docs/architecture.md)

Проект основан на паттерне **Порты и Адаптеры**.
Состоит из нескольких проектов:
1. **YourRest.WebApi** - серверная часть системы отельеров.
2. **TravelSystem.WebApi** - серверная часть системы путешественников. (будет добавлена)
3. **Keycloak** - сервис аутентификации и авторизации, реализованный на основе Keycloak. Этот сервис обеспечивает возможность единого входа (Single Sign-On, SSO) для двух систем. Позволяет входить в обе системы под одним и тем же логином и паролем.

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

```
В интеграционных тестах используется отдельная тестовая база, которая очищается после каждого выполненного теста. При добавлении новых сущностей необходимо обновить метод `ClearAllTables()` в `SharedDbContext`, чтобы очищать соответствующую таблицу. Если одна таблица ссылается на другую, следите за порядком удаления, учитывая foreign key.

## Добавление миграций
## Если выполнять из директории YourRest\Infrastructure:
## Это для SharedDbContext
Windows:
	Producer:
		dotnet ef migrations add InitialCreate -s ..\YourRest.WebApi\YourRest.WebApi.csproj -c SharedDbContext -p YourRest.Producer.Infrastructure\YourRest.Producer.Infrastructure.csproj -v
		dotnet ef database update -s ..\YourRest.WebApi\YourRest.WebApi.csproj -p YourRest.Producer.Infrastructure\YourRest.Producer.Infrastructure.csproj -c SharedDbContext -v		
	ClientIdentity:
		dotnet ef migrations add InitialCreate -s ..\YourRest.ClientWebApp\YourRest.ClientWebApp.csproj -c ClientAppIdentityContext -p YourRest.ClientIdentity.Infrastructure\YourRest.ClientIdentity.Infrastructure.csproj -v
		dotnet ef database update -s ..\YourRest.ClientWebApp\YourRest.ClientWebApp.csproj -p YourRest.ClientIdentity.Infrastructure\YourRest.ClientIdentity.Infrastructure.csproj -c ClientAppIdentityContext -v		
	
Linux:
dotnet ef migrations add AddUserAccommodation -s ../YourRest.WebApi/YourRest.WebApi.csproj -c SharedDbContext -p YourRest.Producer.Infrastructure/YourRest.Producer.Infrastructure.csproj -v
dotnet ef database update -s ../YourRest.WebApi/YourRest.WebApi.csproj -p YourRest.Producer.Infrastructure/YourRest.Producer.Infrastructure.csproj -c SharedDbContext -v
dotnet ef migrations remove -s ../YourRest.WebApi/YourRest.WebApi.csproj -c SharedDbContext -p YourRest.Producer.Infrastructure/YourRest.Producer.Infrastructure.csproj -v

## Запуск проекта в докере
Из папки YourRest:
docker compose up --build*
docker compose up -d

*Если надо перезапустить только основной проект, то docker compose up --build webapi

Пользовательский интерфейс Keycloak - http://localhost:8080/auth/
Keycloak Admin Console - http://localhost:8080/auth/admin/

