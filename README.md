# Project "Your Rest"

## Архитектура

Проект основан на паттерне **Порты и Адаптеры**.
Состоит из двух основных частей:
1. **YourRest.ClientWebApp** - клиентская часть, чистое фронтенд приложение, откуда делаются запросы на сервер.
2. **YourRest.WebApi** - серверная часть.

В приложении присутствуют явно выделенные **BoundedContext** и общие библиотеки: **SharedKernel** (ядро домена) и **YourRest.Infrastructure**. Каждый контекст обладает своими слоями (Domain, Application, Infrastructure), но также может использовать общие слои из-за наличия единой базы данных. Несмотря на то, что в настоящее время проект является модульным монолитом, в будущем его можно разбить на микросервисы по границам BoundedContexts.

Для добавления зависимостей из проекта используются команды:

```
dotnet add reference ../SharedKernel/SharedKernel.csproj
dotnet add reference ../YourRest.Infrastructure/YourRest.Infrastructure.csproj
```

[Подробнее о контекстах](./_docs/bounded_contexts.md)
## Добавление миграции

Для добавления новой миграции в общий SharedDbContext выполните:
```
dotnet new webapi -n BookingContextApi
cd BookingContextApi
dotnet add reference ../SharedKernel/SharedKernel.csproj
dotnet add reference ../YourRest.Infrastructure/YourRest.Infrastructure.csproj
```
### Добавление миграции из YourRest.WebApi в общий SharedDbContext.
```
dotnet ef migrations add InitialCreate --project ../YourRest.Infrastructure/ --startup-project ./
dotnet ef database update --project ../YourRest.Infrastructure/ --startup-project ./

```
## Запуск тестов

Для запуска всех тестов используйте команду:

```
dotnet test

```
- **YourRest.WepApi.Tests** - интеграционные тесты.
- **YourRest.BLL.Tests** - юнит-тесты.

## Документация в Swagger

export ASPNETCORE_ENVIRONMENT=Development
dotnet run

Доступ к документации по API: [Swagger](http://localhost:5201/swagger/index.html)

## База данных

База данных запускается в Docker контейнере с помощью команды:

```
docker compose up -d
```
В интеграционных тестах используется отдельная тестовая база, которая очищается после каждого выполненного теста. При добавлении новых сущностей необходимо обновить метод `ClearAllTables()` в `SharedDbContext`, чтобы очищать соответствующую таблицу. Если одна таблица ссылается на другую, следите за порядком удаления, учитывая foreign key.


