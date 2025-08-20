# Database Queries e Ottimizzazioni

## Panoramica

Questo documento descrive le query principali utilizzate nell'applicazione TaskManager e le strategie di ottimizzazione implementate per garantire prestazioni elevate.

## Query Principali per Entità

### Progetti

#### 1. Ottenere progetti per proprietario

```csharp
// Metodo: IProjectRepository.GetByOwnerIdAsync
var projects = await _context.Projects
    .Where(p => p.OwnerId == ownerId)
    .ToListAsync();
```

**Ottimizzazione**: Indice sull'OwnerId nella tabella Projects.

#### 2. Ottenere progetto con task associati

```csharp
// Metodo: IProjectRepository.GetWithTasksAsync
var project = await _context.Projects
    .Include(p => p.Tasks)
    .FirstOrDefaultAsync(p => p.Id == projectId);
```

**Ottimizzazione**: Utilizzo di Include per eager loading, riducendo il numero di query al database.

#### 3. Ottenere progetti pubblici

```csharp
// Metodo: IProjectRepository.GetPublicProjectsAsync
var publicProjects = await _context.Projects
    .Where(p => !p.IsPrivate)
    .ToListAsync();
```

**Ottimizzazione**: Indice sull'IsPrivate nella tabella Projects.

### Task

#### 1. Ottenere task per colonna

```csharp
// Metodo: ITaskRepository.GetByColumnIdAsync
var tasks = await _context.Tasks
    .Where(t => t.ColumnId == columnId)
    .ToListAsync();
```

**Ottimizzazione**: Indice sull'ColumnId nella tabella Tasks.

#### 2. Ottenere task per assegnatario

```csharp
// Metodo: ITaskRepository.GetByAssigneeIdAsync
var tasks = await _context.Tasks
    .Where(t => t.AssigneeId == assigneeId)
    .ToListAsync();
```

**Ottimizzazione**: Indice sull'AssigneeId nella tabella Tasks.

#### 3. Ricerca task per testo

```csharp
// Metodo: ITaskRepository.SearchTasksAsync
var tasks = await _context.Tasks
    .Where(t => t.Title.Contains(query) || t.Description.Contains(query))
    .ToListAsync();
```

**Ottimizzazione**: Indici full-text su Title e Description per ricerche più efficienti.

#### 4. Ottenere cronologia task

```csharp
// Metodo: ITaskRepository.GetTaskHistoryAsync
// Implementazione attuale restituisce solo il task stesso
var task = await _context.Tasks.FindAsync(taskId);
```

**Nota**: In futuro potrebbe essere implementata una vera cronologia dei cambiamenti.

### Utenti

#### 1. Ottenere utente per nome utente

```csharp
// Metodo: IUserRepository.GetByUsernameAsync
var user = await _context.Users
    .FirstOrDefaultAsync(u => u.Username == username);
```

**Ottimizzazione**: Indice univoco sull'Username nella tabella Users.

#### 2. Ottenere utente per email

```csharp
// Metodo: IUserRepository.GetByEmailAsync
var user = await _context.Users
    .FirstOrDefaultAsync(u => u.Email.Value == email);
```

**Ottimizzazione**: Indice univoco sull'Email.Value nella tabella Users.

#### 3. Ottenere utenti attivi

```csharp
// Metodo: IUserRepository.GetActiveUsersAsync
var activeUsers = await _context.Users
    .Where(u => u.IsActive)
    .ToListAsync();
```

**Ottimizzazione**: Indice sull'IsActive nella tabella Users.

### Board

#### 1. Ottenere board per ID progetto

```csharp
// Metodo: IBoardRepository.GetByProjectIdAsync
var board = await _context.Boards
    .FirstOrDefaultAsync(b => b.ProjectId == projectId);
```

**Ottimizzazione**: Indice sull'ProjectId nella tabella Boards.

#### 2. Ottenere board con colonne

```csharp
// Metodo: IBoardRepository.GetWithColumnsAsync
var board = await _context.Boards
    .Include(b => b.Columns)
    .FirstOrDefaultAsync(b => b.Id == boardId);
```

**Ottimizzazione**: Utilizzo di Include per eager loading.

### Colonne

#### 1. Ottenere colonne per ID progetto

```csharp
// Metodo: IColumnRepository.GetByProjectIdAsync
var columns = await _context.Columns
    .Where(c => c.ProjectId == projectId)
    .ToListAsync();
```

**Ottimizzazione**: Indice sull'ProjectId nella tabella Columns.

#### 2. Ottenere colonne ordinate per posizione

```csharp
// Metodo: IColumnRepository.GetByProjectIdOrderedAsync
var columns = await _context.Columns
    .Where(c => c.ProjectId == projectId)
    .OrderBy(c => c.Position)
    .ToListAsync();
```

**Ottimizzazione**: Indice composito su (ProjectId, Position) per ordinamenti efficienti.

## Strategie di Ottimizzazione

### 1. Indicizzazione

Gli indici sono stati creati per le colonne più comunemente utilizzate nelle query WHERE e ORDER BY:

- **Projects**: OwnerId, IsPrivate
- **Tasks**: ColumnId, AssigneeId, Title, Description
- **Users**: Username (univoco), Email.Value (univoco), IsActive
- **Boards**: ProjectId
- **Columns**: ProjectId, Position

### 2. Eager Loading

L'utilizzo di `Include` e `ThenInclude` per caricare le relazioni in una singola query invece di eseguire query multiple (N+1 problem).

### 3. Proiezioni

Per query che non richiedono l'intera entità, utilizzo di `Select` per proiettare solo i campi necessari:

```csharp
var projectNames = await _context.Projects
    .Where(p => p.IsActive)
    .Select(p => p.Name)
    .ToListAsync();
```

### 4. Paginazione

Per risultati di grandi dimensioni, implementazione della paginazione:

```csharp
var pagedProjects = await _context.Projects
    .Skip((page - 1) * pageSize)
    .Take(pageSize)
    .ToListAsync();
```

### 5. Caching

Per dati che cambiano raramente, implementazione del caching a livello di servizio:

```csharp
public async Task<IEnumerable<Project>> GetPublicProjectsAsync()
{
    var cacheKey = "public_projects";
    var cachedProjects = await _cache.GetAsync<IEnumerable<Project>>(cacheKey);

    if (cachedProjects != null)
        return cachedProjects;

    var projects = await _projectRepository.GetPublicProjectsAsync();
    await _cache.SetAsync(cacheKey, projects, TimeSpan.FromMinutes(10));

    return projects;
}
```

## Monitoraggio delle Prestazioni

### Query Lente

Utilizzo di strumenti come:

1. **Entity Framework Core logging** per monitorare le query generate
2. **SQLite EXPLAIN QUERY PLAN** per analizzare l'efficienza delle query
3. **Application Insights** (in futuro) per monitoraggio in produzione

### Statistiche di Utilizzo

Monitoraggio delle metriche chiave:

- Tempo medio di esecuzione delle query
- Frequenza di utilizzo degli indici
- Dimensione dei result set

## Best Practices

### 1. Evitare il N+1 Problem

Utilizzare sempre `Include` quando si ha bisogno di dati correlati:

```csharp
// Cattivo: causa N+1 queries
var projects = await _context.Projects.ToListAsync();
foreach (var project in projects)
{
    var tasks = await _context.Tasks.Where(t => t.ProjectId == project.Id).ToListAsync();
    // ...
}

// Buono: una singola query con join
var projectsWithTasks = await _context.Projects
    .Include(p => p.Tasks)
    .ToListAsync();
```

### 2. Utilizzare Async/Await

Tutte le operazioni di database devono essere asincrone per evitare blocchi:

```csharp
// Buono
public async Task<Project> GetProjectAsync(Guid projectId)
{
    return await _context.Projects.FindAsync(projectId);
}

// Cattivo
public Project GetProject(Guid projectId)
{
    return _context.Projects.Find(projectId);
}
```

### 3. Gestire le Transazioni

Utilizzare `IUnitOfWork` per operazioni che coinvolgono più repository:

```csharp
public async Task MoveTaskBetweenProjectsAsync(Guid taskId, Guid newProjectId)
{
    using var transaction = await _unitOfWork.BeginTransactionAsync();
    try
    {
        var task = await _taskRepository.GetByIdAsync(taskId);
        task.ProjectId = newProjectId;
        _taskRepository.Update(task);

        await _unitOfWork.SaveChangesAsync();
        await transaction.CommitAsync();
    }
    catch
    {
        await transaction.RollbackAsync();
        throw;
    }
}
```

## Futuri Miglioramenti

1. **Full-text Search**: Implementazione di ricerche full-text più avanzate
2. **Query Caching**: Caching a livello di query per risultati frequenti
3. **Database Connection Pooling**: Ottimizzazione del connection pooling
4. **Read Replicas**: Utilizzo di repliche di lettura per query read-heavy
5. **Database Sharding**: Sharding per progetti molto grandi
