# KrovNadGlavom - Api

## Running the app

To run the app we need to use the following command:

```bash
dotnet watch run serve
```

## Swagger Url

```bash
http://localhost:5020/swagger/index.html
```

## Creating new MySql migrations

To create a new migration for db run the following command at the root of the project:

```bash
dotnet ef migrations add MigrationName -o Infrastructure/MySql/Migrations
```

Then to apply the migrations run the following command at the root of the project:

```bash
dotnet ef database update
```
