# Architettura del Sistema

## Panoramica Architetturale

TaskManager implementa **Clean Architecture** con separazione netta in layer, garantendo manutenibilità, testabilità e scalabilità. Il sistema è progettato come clone di Kanboard utilizzando tecnologie .NET moderne.

## Struttura dei Progetti

```
TaskManager/
├── src/
│   ├── TaskManager.Domain/           # Core Business Logic
│   ├── TaskManager.Application/      # Use Cases & Command/Query
│   ├── TaskManager.Infrastructure/   # Data Access & External Services
│   ├── TaskManager.WebApi/          # REST API Controllers
│   ├── TaskManager.BlazorApp/       # Blazor Server Frontend
│   └── TaskManager.Shared/          # DTOs & Models condivisi
├── tests/
│   ├── TaskManager.UnitTests/       # Test Unitari
│   ├── TaskManager.IntegrationTests/ # Test Integrazione API
│   └── TaskManager.BlazorTests/     # Test UI Blazor
└── docs/                            # Documentazione completa
```

## Layer dell'Architettura

### 1. Domain Layer (`TaskManager.Domain`)

**Responsabilità**: Logica business core, entità, value objects, domain services

**Componenti Attuali**:

- `BaseEntity`: Entità base con Id, CreatedAt, UpdatedAt
- `Project`: Entità progetto con Name, Description, Tasks collection
- `TodoTask`: Entità task con Title, Description, DueDate, IsDone, ProjectId

**Principi**:

- Nessuna dipendenza da layer esterni
- Contiene solo logica business pura
- Definisce interfacce per servizi esterni

### 2. Application Layer (`TaskManager.Application`)

**Responsabilità**: Orchestrazione use cases, command/query handling, DTOs

**Componenti Attuali**:

- `IUnitOfWork`: Interface per transazioni database
- Pattern Command/Query (da implementare con mediator)
- Application services per logica complessa

**Patterns**:

- Command/Query Responsibility Segregation (CQRS)
- Mediator Pattern per decoupling
- Repository Pattern per data access

### 3. Infrastructure Layer (`TaskManager.Infrastructure`)

**Responsabilità**: Implementazione accesso dati, servizi esterni, persistenza

**Componenti Implementati**:

- `KanboardDbContext`: Context Entity Framework
- `DataSeeder`: Seeding dati iniziali
- Configurazioni EF Core per le entità
- Migration database (InitialCreate - 20250819091208)

**Configurazione Database**:

- SQLite per development e testing
- Entity Framework Core 9.0
- Code-First approach con Fluent API

### 4. WebApi Layer (`TaskManager.WebApi`)

**Responsabilità**: REST API endpoints, autenticazione, autorizzazione

**Configurazione Attuale**:

- ASP.NET Core 9.0 Web API
- Dependency injection configurato per DbContext e UnitOfWork
- OpenAPI/Swagger per documentazione
- DataSeeder automatico all'avvio

**Endpoints Pianificati**:

- Projects CRUD operations
- Tasks management
- Board visualization
- User authentication
- Real-time updates con SSE

### 5. Blazor Frontend (`TaskManager.BlazorApp`)

**Responsabilità**: Interfaccia utente, componenti UI, interazioni client

**Stack Frontend**:

- Blazor Server con SignalR
- MudBlazor per UI components
- Drag & Drop per board Kanban
- Real-time sync con backend

## Pattern Architetturali Chiave

### Clean Architecture

```
┌─────────────────────────────────────┐
│           Presentation              │ ← Controllers, Views, Components
├─────────────────────────────────────┤
│           Application               │ ← Use Cases, Commands, Queries
├─────────────────────────────────────┤
│           Infrastructure            │ ← Data Access, External Services
├─────────────────────────────────────┤
│              Domain                 │ ← Entities, Business Rules
└─────────────────────────────────────┘
```

### Dependency Injection

**Configurazione**:

- Built-in ASP.NET Core DI Container
- Scoped lifetime per DbContext
- Singleton per application services
- Transient per command/query handlers

### Repository Pattern

**Interfacce** (da implementare):

```csharp
IRepository<T>
IProjectRepository : IRepository<Project>
ITaskRepository : IRepository<TodoTask>
IUserRepository : IRepository<User>
```

## Database Design

### Schema Attuale

**Projects Table**:

- Id (Guid, PK)
- Name (string, required)
- Description (string, nullable)
- CreatedAt, UpdatedAt (DateTime)

**Tasks Table**:

- Id (Guid, PK)
- Title (string, required)
- Description (string, nullable)
- DueDate (DateTime, nullable)
- IsDone (bool)
- ProjectId (Guid, FK)
- CreatedAt, UpdatedAt (DateTime)

### Relazioni

- Project → Tasks (One-to-Many)
- Configurazione Fluent API in DbContext

## Security Architecture

### Autenticazione (Pianificata)

- ASP.NET Core Identity per utenti locali
- JWT Bearer tokens per API
- Cookie authentication per Blazor

### Autorizzazione (Pianificata)

- Policy-based authorization
- Resource-based authorization per progetti
- Role-based access control

## Real-time Architecture

### Server-Sent Events (SSE)

- Notifiche real-time per board updates
- Connection management per progetti
- Heartbeat per connessioni persistenti

## Testing Architecture

### Struttura Testing

- **Unit Tests**: Business logic isolation
- **Integration Tests**: API endpoints end-to-end
- **UI Tests**: Blazor components con bUnit

### Test Infrastructure

- xUnit framework
- Moq per mocking
- AutoFixture per test data generation
- WebApplicationFactory per integration tests
- In-memory database per isolamento

## Performance Considerations

### Database

- Indici ottimizzati per query frequenti
- Connection pooling
- Lazy loading configurabile

### Frontend

- Blazor Server per ridurre payload
- SignalR per updates real-time
- Component caching strategico

### API

- Response caching per query read-heavy
- Pagination per liste grandi
- Async/await pattern ovunque

## Scalability Design

### Horizontal Scaling

- Stateless application design
- Database separabile dal compute
- Load balancer ready architecture

### Vertical Scaling

- Ottimizzazione query database
- Memory management efficiente
- CPU usage optimization per Blazor

## Monitoring & Observability

### Logging (Pianificato)

- Structured logging con Serilog
- Request/response logging
- Error tracking e alerting

### Metrics (Pianificato)

- Application performance metrics
- Database performance monitoring
- User activity analytics

## Deployment Architecture

### Containerization

- Docker support per tutti i layer
- docker-compose per development
- Production-ready containers

### Environment Configuration

- appsettings per environment
- Secrets management
- Health checks endpoints

## Future Architecture Considerations

### Microservices Migration Path

- Domain boundaries già definiti
- API-first design
- Database per domain segregation

### Cloud-Native Features

- Azure/AWS deployment ready
- Kubernetes manifest prepared
- CI/CD pipeline integration

## Code Organization Principles

### Naming Conventions

- PascalCase per classi e metodi pubblici
- camelCase per campi privati
- Suffissi descrittivi (Service, Repository, Handler)

### Folder Structure

- Organizzazione per feature quando possibile
- Separazione per layer rispettata rigorosamente
- Shared components in progetti dedicati

### Dependency Management

- Principio di inversione delle dipendenze rispettato
- Interface segregation per testabilità
- Single responsibility per ogni classe
