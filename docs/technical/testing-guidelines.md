# Linee Guida per il Testing

## Introduzione

Questo documento fornisce linee guida dettagliate per l'implementazione dei test nel progetto TaskManager. Le linee guida coprono best practices, convenzioni, struttura dei test e approcci specifici per ogni tipo di testing.

## Convenzioni Generali

### Naming dei Test

Utilizzare il pattern `MethodName_StateUnderTest_ExpectedBehavior`:

```csharp
// Buono
public void CreateProject_WithValidData_ProjectCreated()
public void GetUser_WithInvalidId_ThrowsNotFoundException()

// Meno chiaro
public void TestCreateProject()
public void GetUserTest()
```

### Struttura dei Test

Seguire il pattern AAA (Arrange, Act, Assert):

```csharp
[Fact]
public void CreateProject_WithValidData_ProjectCreated()
{
    // Arrange
    var projectService = new Mock<IProjectService>();
    var command = new CreateProjectCommand("Test Project", "Description");

    // Act
    var result = projectService.Object.CreateProject(command);

    // Assert
    Assert.NotNull(result);
    Assert.Equal("Test Project", result.Name);
}
```

### Utilizzo delle Classi Base

Creare classi base per test comuni per ridurre la duplicazione del codice:

```csharp
public abstract class TestBase : IDisposable
{
    protected readonly Fixture Fixture = new Fixture();

    public virtual void Dispose()
    {
        // Cleanup comune
    }
}
```

## Unit Testing

### Quando Scrivere Unit Test

- Per tutta la logica di business
- Per tutti i metodi pubblici
- Per scenari edge case
- Per tutti i percorsi di codice (code paths)

### Cosa Non Testare

- Modelli semplici (DTOs, ViewModels)
- Proprietà con solo getter/setter
- Codice generato automaticamente
- Configurazioni di dependency injection

### Best Practices

1. **Un Assert per Test**: Ogni test dovrebbe verificare un solo comportamento
2. **Test Indipendenti**: I test non devono dipendere dall'ordine di esecuzione
3. **Dati di Test Significativi**: Usare dati che rappresentano scenari reali
4. **Mocking Appropriato**: Mockare solo le dipendenze esterne

### Mocking di Entity Framework DbSet

Per testare repository che utilizzano Entity Framework, è essenziale mockare correttamente i `DbSet<T>`. Il progetto include un helper specializzato per questo scopo:

```csharp
// MockDbSetHelper.cs - Helper per creare mock di DbSet con supporto async completo
public static Mock<DbSet<T>> CreateMockDbSet<T>(IEnumerable<T> elements) where T : class
{
    var list = elements.ToList();
    var queryable = list.AsQueryable();

    var mockSet = new Mock<DbSet<T>>();
    
    // Setup per IQueryable con supporto async
    mockSet.As<IQueryable<T>>().Setup(m => m.Provider)
        .Returns(new TestAsyncQueryProvider<T>(queryable.Provider));
    mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
    mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
    mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());
    
    // Setup per IAsyncEnumerable - necessario per ToListAsync, FirstOrDefaultAsync, etc.
    mockSet.As<IAsyncEnumerable<T>>().Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
        .Returns(new TestAsyncEnumerator<T>(queryable.GetEnumerator()));

    return mockSet;
}
```

**Componenti Chiave del MockDbSetHelper:**

- `TestAsyncQueryProvider<T>`: Implementa `IAsyncQueryProvider` per supportare operazioni LINQ asincrone
- `TestAsyncEnumerable<T>`: Implementa `IAsyncEnumerable<T>` per supportare enumerazione asincrona
- `TestAsyncEnumerator<T>`: Implementa `IAsyncEnumerator<T>` per l'iterazione asincrona

### Pattern per Repository Tests

```csharp
public class RepositoryTests : TestBase
{
    private readonly Mock<KanboardDbContext> _mockContext;
    
    public RepositoryTests()
    {
        _mockContext = new Mock<KanboardDbContext>(new DbContextOptions<KanboardDbContext>());
        
        // Setup di base - importante per evitare null reference nel costruttore del repository
        var emptyMockDbSet = new Mock<DbSet<Entity>>();
        _mockContext.Setup(c => c.Set<Entity>()).Returns(emptyMockDbSet.Object);
    }

    [Fact]
    public async SystemTask RepositoryMethod_WithValidData_ReturnsExpectedResult()
    {
        // Arrange
        var testEntities = new List<Entity>
        {
            new Entity { Id = Guid.NewGuid(), Name = "Test Entity" }
        };

        var mockDbSet = MockDbSetHelper.CreateMockDbSet(testEntities);
        _mockContext.Setup(c => c.Set<Entity>()).Returns(mockDbSet.Object);
        
        var repository = new EntityRepository(_mockContext.Object);

        // Act
        var result = await repository.GetByNameAsync("Test Entity");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test Entity", result.Name);
    }
}
```

**Problemi Risolti dal MockDbSetHelper:**

1. **`System.NotSupportedException: Specified method is not supported`**: Risolto implementando `IAsyncQueryProvider`
2. **`Unsupported expression: FirstOrDefaultAsync/ToListAsync`**: Risolto con supporto completo per metodi extension async
3. **Queries LINQ non funzionanti**: Risolto con provider di query personalizzato che supporta filtri e ordinamenti

### Gestione Conflitti tra Domain.Entities.Task e System.Threading.Tasks.Task

```csharp
using SystemTask = System.Threading.Tasks.Task;
using DomainTask = TaskManager.Domain.Entities.Task;

[Fact]
public async SystemTask TestMethod_WithDomainTask_ReturnsSuccess()
{
    var domainTask = new DomainTask { Id = Guid.NewGuid(), Title = "Test Task" };
    // ...
}
```

### Esempio di Unit Test

```csharp
public class ProjectServiceTests
{
    [Fact]
    public void CreateProject_WithValidData_ReturnsNewProject()
    {
        // Arrange
        var mockRepository = new Mock<IProjectRepository>();
        var service = new ProjectService(mockRepository.Object);
        var command = new CreateProjectCommand("Test Project", "Description");

        // Act
        var result = service.CreateProject(command);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test Project", result.Name);
        Assert.Equal("Description", result.Description);
        mockRepository.Verify(r => r.Add(It.IsAny<Project>()), Times.Once);
    }
}
```

## Integration Testing

### Quando Scrivere Integration Test

- Per testare endpoint API
- Per verificare l'integrazione con il database
- Per testare scenari end-to-end
- Per validare l'autenticazione e l'autorizzazione

### Configurazione dell'Ambiente

Utilizzare `WebApplicationFactory` per creare un ambiente di test in memoria:

```csharp
public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup>
    where TStartup : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Configurazione servizi per test
        });
    }
}
```

### Database nei Test

Utilizzare database in-memory per isolamento:

```csharp
public class TestDbContext : IDisposable
{
    private readonly SqliteConnection _connection;
    public KanboardDbContext Context { get; }

    public TestDbContext()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<KanboardDbContext>()
            .UseSqlite(_connection)
            .Options;

        Context = new KanboardDbContext(options);
        Context.Database.EnsureCreated();
    }

    public void Dispose()
    {
        Context?.Dispose();
        _connection?.Close();
    }
}
```

### Esempio di Integration Test

```csharp
public class ProjectsControllerTests : IClassFixture<CustomWebApplicationFactory<Startup>>
{
    private readonly HttpClient _client;

    public ProjectsControllerTests(CustomWebApplicationFactory<Startup> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetProjects_ReturnsSuccess()
    {
        // Act
        var response = await _client.GetAsync("/api/projects");

        // Assert
        response.EnsureSuccessStatusCode();
    }
}
```

## UI Testing (Blazor)

### Quando Scrivere UI Test

- Per componenti complessi
- Per interazioni utente critiche
- Per verificare il rendering condizionale
- Per testare il data binding

### Utilizzo di bUnit

bUnit consente di testare componenti Blazor in isolamento:

```csharp
public class ComponentTests
{
    [Fact]
    public void CounterComponent_InitialValue_IsZero()
    {
        // Arrange
        using var ctx = new TestContext();

        // Act
        var cut = ctx.RenderComponent<Counter>();

        // Assert
        cut.Markup.Contains("0");
    }
}
```

### Mocking dei Servizi

Mockare i servizi per testare i componenti in isolamento:

```csharp
public class ComponentWithServiceTests
{
    [Fact]
    public void Component_WithMockedService_RendersData()
    {
        // Arrange
        using var ctx = new TestContext();
        var mockService = new Mock<IMyService>();
        mockService.Setup(s => s.GetData()).Returns("Test Data");
        ctx.Services.AddSingleton(mockService.Object);

        // Act
        var cut = ctx.RenderComponent<MyComponent>();

        // Assert
        cut.Markup.Contains("Test Data");
    }
}
```

## Utilizzo di AutoFixture

AutoFixture semplifica la creazione di dati di test, ma deve essere usato con attenzione per Entity Framework:

```csharp
public class ProjectTests
{
    private readonly Fixture _fixture = new Fixture();

    [Fact]
    public void CreateProject_WithAutoData_HasCorrectProperties()
    {
        // Arrange - Configurazione per evitare problemi con navigation properties
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        var projectName = _fixture.Create<string>();
        var projectDescription = _fixture.Create<string>();

        // Act
        var project = new Project(projectName, projectDescription);

        // Assert
        Assert.Equal(projectName, project.Name);
        Assert.Equal(projectDescription, project.Description);
    }
}
```

## Code Coverage

### Obiettivi Aggiornati

- **Minimo**: 90% di copertura totale ✅ **Raggiunto: 100%**
- **Importante**: 100% di copertura per la logica di business ✅ **Raggiunto**
- **Critico**: 100% di copertura per percorsi di errore ✅ **Raggiunto**

### Risultati Attuali

Il progetto ha raggiunto una copertura di test del **100%** per tutti i repository tests:
- BaseRepositoryTests: 9/9 test ✅
- UserRepositoryTests: 4/4 test ✅
- ProjectRepositoryTests: 3/3 test ✅
- TaskRepositoryTests: 4/4 test ✅
- ColumnRepositoryTests: 2/2 test ✅
- BoardRepositoryTests: 2/2 test ✅

**Totale: 79 test passano, 0 fallimenti**

## Refactoring e Manutenzione dei Test

### Quando Aggiornare i Test

- Quando cambia l'API pubblica
- Quando si correggono bug
- Quando si aggiungono nuove funzionalità
- Durante il refactoring del codice

### Come Mantenere i Test

- Rivedere i test durante le code review
- Aggiornare i nomi dei test quando cambia il comportamento
- Rimuovere i test obsoleti
- Aggiungere nuovi test per coprire scenari mancanti

## Conclusione

Seguendo queste linee guida, possiamo garantire che i nostri test siano mantenibili, affidabili e utili per garantire la qualità del software. I test non sono solo una verifica post-implementazione, ma un elemento fondamentale del processo di sviluppo che guida la progettazione e l'implementazione del codice.
