// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

// Install dotnet-ef
// dotnet tool install --global dotnet-ef

// Update dotnet-ef
// dotnet tool update --global dotnet-ef

// Init Migrations
// dotnet ef migrations add InitialMigration --project YourRest\YourRest.DAL.Postgre --startup-project YourRest\YourRest.DAL.Postgre.Migration

// Init Migrations
// dotnet ef migrations remove --project YourRest\YourRest.DAL.Postgre --startup-project YourRest\YourRest.DAL.Postgre.Migration

// Update DataBase
// source\repos>dotnet ef database update --project YourRest\YourRest.DAL.Postgre --startup-project YourRest\YourRest.DAL.Postgre.Migration --connection "Host=localhost;uid=postgres;pwd=12345;Port=5432;No Reset On Close=true;Database=YourRestDb;Application Name=YourRest.ClientWebApp" --verbose
