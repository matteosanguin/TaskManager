# Database Design

This document outlines the database schema for the TaskManager application.

## Schema

The database uses SQLite and is managed via Entity Framework Core migrations.

### Tables

#### `Projects`

Stores project information.

| Column      | Type        | Constraints      | Description                  |
|-------------|-------------|------------------|------------------------------|
| `Id`        | `TEXT`      | PRIMARY KEY      | The unique identifier.       |
| `Name`      | `TEXT`      | NOT NULL         | The name of the project.     |
| `Description` | `TEXT`      | NULL             | A description of the project.|
| `CreatedAt` | `TEXT`      | NOT NULL         | Timestamp of creation.       |
| `UpdatedAt` | `TEXT`      | NOT NULL         | Timestamp of last update.    |

#### `Tasks`

Stores task information related to a project.

| Column      | Type        | Constraints      | Description                  |
|-------------|-------------|------------------|------------------------------|
| `Id`        | `TEXT`      | PRIMARY KEY      | The unique identifier.       |
| `Title`     | `TEXT`      | NOT NULL         | The title of the task.       |
| `Description` | `TEXT`      | NULL             | A description of the task.   |
| `DueDate`   | `TEXT`      | NULL             | The due date of the task.    |
| `IsDone`    | `INTEGER`   | NOT NULL         | Whether the task is complete.|
| `ProjectId` | `TEXT`      | FOREIGN KEY      | FK to the `Projects` table.  |
| `CreatedAt` | `TEXT`      | NOT NULL         | Timestamp of creation.       |
| `UpdatedAt` | `TEXT`      | NOT NULL         | Timestamp of last update.    |

### Relationships

- A `Project` can have many `Tasks`.
- A `Task` belongs to exactly one `Project`. This is a one-to-many relationship.
