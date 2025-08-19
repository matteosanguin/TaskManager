# Piano di Implementazione Entità Domain

## Panoramica

Questo documento fornisce un piano dettagliato per l'implementazione di tutte le entità del dominio e i Value Objects richiesti per il Task 2.1.

## Struttura del Progetto

```
TaskManager.Domain/
├── Common/
│   └── BaseEntity.cs
├── Entities/
│   ├── User.cs
│   ├── Project.cs
│   ├── Board.cs
│   ├── Column.cs
│   ├── Task.cs
│   ├── Comment.cs
│   ├── Category.cs
│   ├── Tag.cs
│   ├── TaskTag.cs
│   ├── Swimlane.cs
│   └── Attachment.cs
├── ValueObjects/
│   ├── Email.cs
│   └── Priority.cs
└── Enums/
    └── PriorityLevel.cs
```

## Implementazione Passo-Passo

### Passo 1: Creazione Value Objects

#### Email.cs

```csharp
using System.Text.RegularExpressions;

namespace TaskManager.Domain.ValueObjects
{
    public class Email
    {
        public string Value { get; }

        private Email(string value)
        {
            Value = value;
        }

        public static Email Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Email cannot be null or empty", nameof(value));

            if (!IsValidEmail(value))
                throw new ArgumentException("Invalid email format", nameof(value));

            return new Email(value);
        }

        private static bool IsValidEmail(string email)
        {
            // Implementazione della validazione email
            var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            return regex.IsMatch(email);
        }

        public override bool Equals(object? obj)
        {
            if (obj is Email other)
                return Value.Equals(other.Value, StringComparison.OrdinalIgnoreCase);

            return false;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode(StringComparison.OrdinalIgnoreCase);
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
```

#### PriorityLevel.cs (Enum)

```csharp
namespace TaskManager.Domain.Enums
{
    public enum PriorityLevel
    {
        Low = 1,
        Normal = 2,
        High = 3,
        Urgent = 4
    }
}
```

#### Priority.cs

```csharp
using TaskManager.Domain.Enums;

namespace TaskManager.Domain.ValueObjects
{
    public class Priority
    {
        public PriorityLevel Level { get; }

        private Priority(PriorityLevel level)
        {
            Level = level;
        }

        public static Priority Create(PriorityLevel level)
        {
            return new Priority(level);
        }

        public static Priority Low => new Priority(PriorityLevel.Low);
        public static Priority Normal => new Priority(PriorityLevel.Normal);
        public static Priority High => new Priority(PriorityLevel.High);
        public static Priority Urgent => new Priority(PriorityLevel.Urgent);

        public override bool Equals(object? obj)
        {
            if (obj is Priority other)
                return Level == other.Level;

            return false;
        }

        public override int GetHashCode()
        {
            return Level.GetHashCode();
        }

        public override string ToString()
        {
            return Level.ToString();
        }
    }
}
```

### Passo 2: Creazione Entità Base

#### BaseEntity.cs (già esistente, ma potrebbe richiedere modifiche)

```csharp
namespace TaskManager.Domain.Common
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
```

### Passo 3: Creazione Entità

#### User.cs

```csharp
using TaskManager.Domain.Common;
using TaskManager.Domain.ValueObjects;

namespace TaskManager.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Username { get; set; } = string.Empty;
        public Email Email { get; set; } = null!;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Theme { get; set; } = string.Empty;
        public string TimeZone { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;

        // Navigation properties
        public ICollection<Project> OwnedProjects { get; set; } = new List<Project>();
        public ICollection<Task> AssignedTasks { get; set; } = new List<Task>();
        public ICollection<Task> CreatedTasks { get; set; } = new List<Task>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Attachment> UploadedAttachments { get; set; } = new List<Attachment>();
    }
}
```

#### Project.cs (da modificare)

```csharp
using TaskManager.Domain.Common;

namespace TaskManager.Domain.Entities
{
    public class Project : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Identifier { get; set; } = string.Empty;
        public Guid OwnerId { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsPrivate { get; set; } = false;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? TaskLimit { get; set; }
        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public User Owner { get; set; } = null!;
        public Board Board { get; set; } = null!;
        public ICollection<Column> Columns { get; set; } = new List<Column>();
        public ICollection<Task> Tasks { get; set; } = new List<Task>();
        public ICollection<Category> Categories { get; set; } = new List<Category>();
        public ICollection<Tag> Tags { get; set; } = new List<Tag>();
        public ICollection<Swimlane> Swimlanes { get; set; } = new List<Swimlane>();
    }
}
```

#### Board.cs

```csharp
using TaskManager.Domain.Common;

namespace TaskManager.Domain.Entities
{
    public class Board : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public Guid ProjectId { get; set; }

        // Navigation properties
        public Project Project { get; set; } = null!;
        public ICollection<Column> Columns { get; set; } = new List<Column>();
    }
}
```

#### Column.cs

```csharp
using TaskManager.Domain.Common;

namespace TaskManager.Domain.Entities
{
    public class Column : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public int Position { get; set; }
        public Guid ProjectId { get; set; }
        public int? TaskLimit { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool HideInDashboard { get; set; } = false;

        // Navigation properties
        public Project Project { get; set; } = null!;
        public ICollection<Task> Tasks { get; set; } = new List<Task>();
    }
}
```

#### Task.cs (sostituire TodoTask)

```csharp
using TaskManager.Domain.Common;
using TaskManager.Domain.ValueObjects;

namespace TaskManager.Domain.Entities
{
    public class Task : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Guid ProjectId { get; set; }
        public Guid ColumnId { get; set; }
        public Guid? AssigneeId { get; set; }
        public Guid CreatorId { get; set; }
        public int Position { get; set; }
        public Priority Priority { get; set; } = Priority.Normal;
        public DateTime? DueDate { get; set; }
        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
        public int? TimeEstimated { get; set; }
        public int? TimeSpent { get; set; }

        // Navigation properties
        public Project Project { get; set; } = null!;
        public Column Column { get; set; } = null!;
        public User? Assignee { get; set; }
        public User Creator { get; set; } = null!;
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
        public ICollection<Tag> Tags { get; set; } = new List<Tag>();
    }
}
```

#### Comment.cs

```csharp
using TaskManager.Domain.Common;

namespace TaskManager.Domain.Entities
{
    public class Comment : BaseEntity
    {
        public Guid TaskId { get; set; }
        public Guid UserId { get; set; }
        public string Content { get; set; } = string.Empty;
        public string Visibility { get; set; } = "Public";

        // Navigation properties
        public Task Task { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}
```

#### Category.cs

```csharp
using TaskManager.Domain.Common;

namespace TaskManager.Domain.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public Guid ProjectId { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;

        // Navigation properties
        public Project Project { get; set; } = null!;
    }
}
```

#### Tag.cs

```csharp
using TaskManager.Domain.Common;

namespace TaskManager.Domain.Entities
{
    public class Tag : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public Guid ProjectId { get; set; }
        public string Color { get; set; } = string.Empty;

        // Navigation properties
        public Project Project { get; set; } = null!;
        public ICollection<Task> Tasks { get; set; } = new List<Task>();
    }
}
```

#### TaskTag.cs

```csharp
namespace TaskManager.Domain.Entities
{
    public class TaskTag
    {
        public Guid TaskId { get; set; }
        public Guid TagId { get; set; }

        // Navigation properties
        public Task Task { get; set; } = null!;
        public Tag Tag { get; set; } = null!;
    }
}
```

#### Swimlane.cs

```csharp
using TaskManager.Domain.Common;

namespace TaskManager.Domain.Entities
{
    public class Swimlane : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public Guid ProjectId { get; set; }
        public int Position { get; set; }
        public bool IsActive { get; set; } = true;
        public int? TaskLimit { get; set; }

        // Navigation properties
        public Project Project { get; set; } = null!;
    }
}
```

#### Attachment.cs

```csharp
using TaskManager.Domain.Common;

namespace TaskManager.Domain.Entities
{
    public class Attachment : BaseEntity
    {
        public Guid TaskId { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
        public Guid UploadedById { get; set; }

        // Navigation properties
        public Task Task { get; set; } = null!;
        public User UploadedBy { get; set; } = null!;
    }
}
```

## Configurazione DbContext

Nel file `KanboardDbContext.cs`, aggiornare le DbSet e le configurazioni:

```csharp
public DbSet<User> Users { get; set; }
public DbSet<Project> Projects { get; set; }
public DbSet<Board> Boards { get; set; }
public DbSet<Column> Columns { get; set; }
public DbSet<Task> Tasks { get; set; }
public DbSet<Comment> Comments { get; set; }
public DbSet<Category> Categories { get; set; }
public DbSet<Tag> Tags { get; set; }
public DbSet<TaskTag> TaskTags { get; set; }
public DbSet<Swimlane> Swimlanes { get; set; }
public DbSet<Attachment> Attachments { get; set; }
```

E aggiornare il metodo `OnModelCreating` con tutte le relazioni.

## Test Unitari

I test unitari dovranno coprire:

1. Creazione di ogni entità con proprietà valide
2. Validazione delle proprietà obbligatorie
3. Verifica delle relazioni tra entità
4. Validazione dei Value Objects
5. Test di uguaglianza per Value Objects

## Considerazioni Finali

1. Eliminare le entità temporanee create nella Fase 1 (Project.cs e TodoTask.cs) dopo aver creato le nuove
2. Aggiornare tutti i riferimenti nel codice esistente
3. Creare nuove migrations per il database
4. Aggiornare il DataSeeder con dati di esempio per le nuove entità
