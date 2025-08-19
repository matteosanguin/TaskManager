# Database Setup

This document provides instructions for setting up and managing the application's database.

## Prerequisites

- .NET 9.0 SDK or later.
- Entity Framework Core tools installed globally (`dotnet tool install --global dotnet-ef`).

## Initial Setup

The application uses a SQLite database. The database file (`Kanboard.db`) will be created automatically in the `src/TaskManager.WebApi` directory when the application is first run.

The application is configured to automatically apply pending migrations on startup.

## Managing Migrations

Migrations are used to evolve the database schema over time.

### Creating a New Migration

When you make changes to the domain models in `TaskManager.Domain`, you will need to create a new migration to apply those changes to the database.

To create a new migration, run the following command from the root of the repository:

```bash
dotnet ef migrations add <MigrationName> --startup-project src/TaskManager.WebApi --project src/TaskManager.Infrastructure
```

Replace `<MigrationName>` with a descriptive name for your migration (e.g., `AddUserRoles`).

### Applying Migrations

Migrations are applied automatically at startup. If you need to apply them manually, you can run the following command:

```bash
dotnet ef database update --startup-project src/TaskManager.WebApi --project src/TaskManager.Infrastructure
```

### Reverting a Migration

To revert the last migration, use the following command:

```bash
dotnet ef database update <PreviousMigrationName> --startup-project src/TaskManager.WebApi --project src/TaskManager.Infrastructure
```
