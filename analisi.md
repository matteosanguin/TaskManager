# Analisi e Piano di Implementazione Kanboard Clone .NET

## 1. ANALISI DETTAGLIATA DEL SOFTWARE ORIGINALE

### 1.1 Panoramica Generale

Kanboard https://github.com/kanboard/kanboard è un software open source di project management che si concentra sulla metodologia Kanban. L'applicazione è progettata per essere semplice e minimalista, con un numero di funzionalità volutamente limitato.

### 1.2 Architettura Tecnologica Originale

- **Backend**: PHP (versione 8.1+)
- **Database**: SQLite (default), MySQL/MariaDB, PostgreSQL
- **Frontend**: HTML, CSS, JavaScript vanilla
- **Framework**: Custom PHP framework
- **Licenza**: MIT License

### 1.3 Funzionalità Core Identificate

#### 1.3.1 Gestione Progetti

Ogni progetto ha proprietà come: ID, nome, descrizione, identificativo, token, owner, date di inizio/fine, stato attivo/inattivo, tipo privato/team

#### 1.3.2 Gestione Board Kanban

La board Kanban è il modo migliore per conoscere lo stato attuale di un progetto perché è visuale. È molto facile da comprendere e non richiede training.

**Caratteristiche delle Board:**

- Possibilità di aggiungere, rinominare e rimuovere colonne in qualsiasi momento per adattare la board al progetto
- Limiti per colonna per evitare multitasking e rimanere concentrati. Quando si supera il limite, la colonna viene evidenziata
- Nomi di colonna predefiniti: Backlog, Ready, Work in Progress, e Done

#### 1.3.3 Gestione Task

I task possono essere spostati tra colonne, copiati, duplicati tra progetti. Ogni movimento tra colonne viene registrato nel database.

**Proprietà dei Task:**

- Titolo, descrizione, categoria
- Date (creazione, scadenza, inizio)
- Assignee (utente assegnato)
- Priorità, complessità
- Tag/etichette
- Allegati e screenshot
- Task ricorrenti basati su eventi della board piuttosto che su date

#### 1.3.4 Subtask

Possibilità di suddividere un task in sotto-task, stimare il tempo o la complessità

#### 1.3.5 Sistema di Ricerca e Filtri

Kanboard ha un linguaggio di query molto semplice che offre la flessibilità di trovare task rapidamente. È possibile applicare dinamicamente filtri personalizzati sulla board

#### 1.3.6 Gestione Utenti e Autenticazione Locale

Sistema di utenti locali con ruoli e permessi

**Ruoli identificati:**

- Sistema di ruoli con utenti standard come default
- Amministratori di progetto
- Membri del team

#### 1.3.7 Sistema di Notifiche

- Email notifications
- Webhook support
- Integrazione con sistemi di chat

#### 1.3.8 API

Kanboard utilizza il protocollo JSON-RPC per interagire con programmi esterni. Supporta batch requests per fare multiple chiamate API in una singola richiesta HTTP

### 1.4 Schema Database Analizzato

#### 1.4.1 Tabelle Principali Identificate

Basandomi sui file di migrazione MySQL analizzati:

**Projects Table:**

- id, name, description, identifier, token
- owner_id, is_active, is_private, is_public
- start_date, end_date, last_modified
- task_limit, per_swimlane_task_limits
- enable_global_tags

**Tasks Table:**

- id, title, description, project_id, column_id
- owner_id, creator_id, assignee_id
- date_creation, date_modification, date_completed, date_due, date_started
- time_estimated, time_spent, position
- score, category*id, priority, recurrence*\*
- reference, swimlane_id

**Columns Table:**

- id, title, position, project_id
- task_limit, description, hide_in_dashboard

**Users Table:**

- id, username, password, email, role
- name, notifications_enabled, timezone
- language, disable_login_form
- token, theme

**Comments Table:**

- id, task_id, user_id, date_creation
- comment, reference
- visibility

**Swimlanes Table:**

- id, name, project_id, description, position, is_active, task_limit

**Categories Table:**

- id, name, project_id, description

**Tags Table:**

- id, name, project_id, color_id

**Links Table:**

- id, label, opposite_label

## 2. ARCHITETTURA PROPOSTA PER IL CLONE .NET

### 2.1 Stack Tecnologico

- **Backend**: ASP.NET Core 9.0 Web API
- **Database**: SQLite con Entity Framework Core
- **Frontend**: Blazor Server con MudBlazor
- **Autenticazione**: ASP.NET Core Identity (solo utenti locali)
- **Testing**: xUnit, Moq, WebApplicationFactory
- **API**: RESTful API + Server-Sent Events (SSE) per real-time updates
- **Mapping**: Mapperly invece di AutoMapper
- **Command/Query Pattern**: Libreria alternativa permissiva a MediatR

### 2.2 Alternativa a MediatR con Licenza Permissiva

Utilizzeremo **FastEndpoints** (licenza MIT) che include un sistema di mediator integrato oppure implementeremo un semplice mediator pattern custom con:

- `ICommandHandler<TCommand, TResult>`
- `IQueryHandler<TQuery, TResult>`
- `IMediator` interface per orchestrazione
- Dependency injection per risoluzione automatica handlers

### 2.3 Struttura del Progetto

```
TaskManager/
├── src/
│   ├── TaskManager.Domain/           # Entità e interfacce
│   ├── TaskManager.Infrastructure/   # Data Access, External Services
│   ├── TaskManager.Application/      # Business Logic, Command/Query
│   ├── TaskManager.WebApi/          # REST API Controllers
│   ├── TaskManager.BlazorApp/       # Blazor Server App
│   └── TaskManager.Shared/          # DTOs e Models condivisi
├── tests/
│   ├── TaskManager.UnitTests/       # Test Unitari
│   ├── TaskManager.IntegrationTests/ # Test di Integrazione API
│   └── TaskManager.BlazorTests/     # Test UI Blazor
└── docs/
    ├── technical/                     # Documentazione tecnica
    ├── functional/                    # Documentazione funzionale
    ├── deployment/                    # Manuale di deploy
    └── user/                         # Manuale utente
```

### 2.4 Pattern Architetturali

- **Clean Architecture** con separazione in layers
- **Command/Query Pattern** con mediator custom o FastEndpoints
- **Repository Pattern** con Unit of Work
- **Domain-Driven Design** per la logica business

## 3. ELENCO TASK COMPLETI E SUCCESSIVI

### FASE 1: SETUP INIZIALE E FONDAMENTA

#### Task 1.1: Setup Struttura Progetto e Documentazione Base

**Obiettivo**: Creare la struttura base del progetto con tutti i progetti necessari e iniziare la documentazione

**Attività dettagliate:**

1. Creare solution "TaskManager"
2. Aggiungere progetti:
   - `TaskManager.Domain` (Class Library)
   - `TaskManager.Infrastructure` (Class Library)
   - `TaskManager.Application` (Class Library)
   - `TaskManager.WebApi` (ASP.NET Core 9.0 Web API)
   - `TaskManager.BlazorApp` (Blazor Server App)
   - `TaskManager.Shared` (Class Library)
3. Configurare riferimenti tra progetti secondo Clean Architecture
4. Installare pacchetti NuGet base:
   - Entity Framework Core 9.0
   - Mapperly
   - MudBlazor
   - xUnit
   - Moq
   - FastEndpoints (o implementare mediator custom)
5. **Documentazione da creare:**
   - `docs/technical/architecture-overview.md` - Panoramica architettura
   - `docs/technical/project-structure.md` - Struttura del progetto
   - `docs/functional/requirements.md` - Requisiti funzionali
   - `docs/deployment/environment-setup.md` - Setup ambiente sviluppo

**Deliverable**: Struttura progetto compilabile con documentazione iniziale

#### Task 1.2: Configurazione Database e Entity Framework

**Obiettivo**: Configurare SQLite con Entity Framework Core 9.0

**Attività dettagliate:**

1. In `TaskManager.Infrastructure`:
   - Creare `KanboardDbContext`
   - Configurare connection string per SQLite
   - Implementare `IUnitOfWork` interface
2. Configurare dependency injection in `Program.cs`
3. Creare prima migration vuota per testare setup
4. Implementare database seeding per dati di test
5. **Documentazione da aggiornare:**
   - `docs/technical/database-design.md` - Schema database e relazioni
   - `docs/deployment/database-setup.md` - Setup e migrazione database

**Deliverable**: Database SQLite funzionante con migration system e documentazione

#### Task 1.3: Setup Testing Infrastructure

**Obiettivo**: Configurare ambiente di testing completo

**Attività dettagliate:**

1. In `TaskManager.UnitTests`:
   - Configurare xUnit con Moq
   - Creare base classes per testing
   - Setup AutoFixture per generazione dati test
2. In `TaskManager.IntegrationTests`:
   - Configurare `WebApplicationFactory`
   - Setup database in-memory per integration tests
   - Creare helper per autenticazione nei test
3. In `TaskManager.BlazorTests`:
   - Configurare bUnit per testing Blazor components
   - Setup mock services per testing UI
4. **Documentazione da creare:**
   - `docs/technical/testing-strategy.md` - Strategia di testing
   - `docs/technical/testing-guidelines.md` - Linee guida per test

**Deliverable**: Test projects configurati con documentazione testing

#### Task 1.4: Implementare Mediator Pattern Custom o FastEndpoints

**Obiettivo**: Configurare sistema di command/query handling

**Attività dettagliate:**

1. **Opzione A - Mediator Custom:**

   - Creare `ICommandHandler<TCommand, TResult>` interface
   - Creare `IQueryHandler<TQuery, TResult>` interface
   - Implementare `IMediator` con dependency injection
   - Creare `MediatorExtensions` per registrazione automatica

2. **Opzione B - FastEndpoints:**

   - Configurare FastEndpoints package
   - Setup endpoint discovery e mapping
   - Configurare validation e serialization

3. **Documentazione da aggiornare:**
   - `docs/technical/command-query-pattern.md` - Pattern implementato
   - `docs/technical/dependency-injection.md` - Configurazione DI

**Deliverable**: Sistema command/query funzionante con documentazione

### FASE 2: DOMAIN LAYER E ENTITÀ

#### Task 2.1: Implementare Entità Domain Core

**Obiettivo**: Creare tutte le entità del dominio con le relazioni

**Attività dettagliate:**

1. In `TaskManager.Domain/Entities`:

   - Creare `User` entity con proprietà: Id, Username, Email, PasswordHash, Role, CreatedAt, Theme, TimeZone, IsActive
   - Creare `Project` entity con: Id, Name, Description, Identifier, OwnerId, IsActive, IsPrivate, StartDate, EndDate, TaskLimit, CreatedAt, ModifiedAt
   - Creare `Board` entity con: Id, Name, ProjectId, CreatedAt
   - Creare `Column` entity con: Id, Title, Position, ProjectId, TaskLimit, Description, HideInDashboard
   - Creare `Task` entity con: Id, Title, Description, ProjectId, ColumnId, AssigneeId, CreatorId, Position, Priority, DueDate, CreatedAt, ModifiedAt, TimeEstimated, TimeSpent
   - Creare `Comment` entity con: Id, TaskId, UserId, Content, CreatedAt, Visibility
   - Creare `Category` entity con: Id, Name, ProjectId, Description, Color
   - Creare `Tag` entity con: Id, Name, ProjectId, Color
   - Creare `TaskTag` entity per many-to-many relationship
   - Creare `Swimlane` entity con: Id, Name, ProjectId, Position, IsActive, TaskLimit
   - Creare `Attachment` entity con: Id, TaskId, FileName, FilePath, FileSize, UploadedAt, UploadedById

2. Configurare Navigation Properties per tutte le relazioni
3. Implementare Value Objects dove appropriato (es: Email, Priority)
4. **Documentazione da aggiornare:**
   - `docs/technical/domain-model.md` - Modello di dominio completo
   - `docs/functional/data-entities.md` - Descrizione entità e relazioni

**Test Unitari da implementare:**

- Test creazione entità con proprietà valide
- Test validazione proprietà obbligatorie
- Test relazioni tra entità
- Test Value Objects validation

**Deliverable**: Entità domain complete con relazioni e documentazione

#### Task 2.2: Implementare Domain Services e Business Rules

**Obiettivo**: Implementare la logica business core nel domain

**Attività dettagliate:**

1. Creare `IDomainService` interface base
2. Implementare `TaskMovementService`:
   - Logica per spostamento task tra colonne
   - Validazione limiti colonna
   - Calcolo nuova posizione
   - Registrazione movimento nella history
3. Implementare `ProjectPermissionService`:
   - Controllo permessi utente su progetto
   - Validazione accesso alle risorse
4. Implementare `TaskAssignmentService`:
   - Logica assegnazione task
   - Validazione disponibilità utente
5. Creare Domain Events:
   - `TaskMovedEvent`
   - `TaskAssignedEvent`
   - `ProjectCreatedEvent`
   - `CommentAddedEvent`
6. **Documentazione da aggiornare:**
   - `docs/technical/business-rules.md` - Regole business implementate
   - `docs/functional/workflows.md` - Flussi di lavoro principali

**Test Unitari da implementare:**

- Test spostamento task tra colonne
- Test validazione limiti colonna
- Test permessi utente
- Test domain events

**Deliverable**: Domain services con business logic e documentazione

### FASE 3: INFRASTRUCTURE LAYER

#### Task 3.1: Implementare Repository Pattern

**Obiettivo**: Creare repositories per accesso ai dati

**Attività dettagliate:**

1. In `TaskManager.Domain/Interfaces`:
   - Creare `IRepository<T>` interface generica
   - Creare interfaces specifiche: `IProjectRepository`, `ITaskRepository`, `IUserRepository`, `IBoardRepository`, `IColumnRepository`
2. In `TaskManager.Infrastructure/Repositories`:
   - Implementare `BaseRepository<T>`
   - Implementare `ProjectRepository` con metodi specifici:
     - `GetByOwnerIdAsync(int ownerId)`
     - `GetWithTasksAsync(int projectId)`
     - `GetPublicProjectsAsync()`
   - Implementare `TaskRepository`:
     - `GetByColumnIdAsync(int columnId)`
     - `GetByAssigneeIdAsync(int assigneeId)`
     - `SearchTasksAsync(string query)`
     - `GetTaskHistoryAsync(int taskId)`
   - Implementare altri repositories con metodi specifici
3. Configurare dependency injection
4. **Documentazione da aggiornare:**
   - `docs/technical/data-access-layer.md` - Pattern repository e implementazione
   - `docs/technical/database-queries.md` - Query principali e ottimizzazioni

**Test Unitari da implementare:**

- Test CRUD operations per ogni repository
- Test metodi di query specifici
- Test gestione delle relazioni

**Deliverable**: Repository pattern completo con test e documentazione

#### Task 3.2: Configurare Entity Framework Mappings

**Obiettivo**: Configurare mapping EF Core per tutte le entità

**Attività dettagliate:**

1. In `TaskManager.Infrastructure/Data/Configurations`:
   - Creare `UserConfiguration` con fluent API mapping
   - Creare `ProjectConfiguration` con indici e constraints
   - Creare `TaskConfiguration` con relazioni complesse
   - Creare configurazioni per tutte le altre entità
2. Configurare seed data per:
   - Utente amministratore default
   - Ruoli base del sistema
   - Categorie predefinite
3. Creare migration completa con schema database
4. Implementare `DbContextInitializer` per setup automatico
5. **Documentazione da aggiornare:**
   - `docs/technical/database-schema.md` - Schema completo con relazioni
   - `docs/deployment/database-migration.md` - Procedura migrazione database

**Test di Integrazione da implementare:**

- Test migration database
- Test seed data
- Test query complex con join
- Test performance query principali

**Deliverable**: Database schema completo con migrations e documentazione

#### Task 3.3: Implementare Authentication Infrastructure con ASP.NET Core Identity

**Obiettivo**: Configurare sistema di autenticazione locale

**Attività dettagliate:**

1. Configurare ASP.NET Core Identity con custom User entity
2. Implementare `IAuthService` interface:
   - `LoginAsync(string username, string password)`
   - `RegisterAsync(RegisterRequest request)`
   - `GetCurrentUserAsync()`
   - `UpdateUserAsync(UpdateUserRequest request)`
3. Implementare JWT token authentication per API
4. Configurare cookie authentication per Blazor
5. Implementare `IPermissionService`:
   - `CanAccessProjectAsync(int userId, int projectId)`
   - `CanEditTaskAsync(int userId, int taskId)`
   - `IsProjectOwnerAsync(int userId, int projectId)`
6. **Documentazione da creare:**
   - `docs/technical/authentication-system.md` - Sistema autenticazione implementato
   - `docs/technical/authorization-policies.md` - Politiche di autorizzazione
   - `docs/functional/user-management.md` - Gestione utenti e ruoli

**Test Unitari da implementare:**

- Test login/logout
- Test registrazione utente
- Test validazione token JWT
- Test sistema permessi

**Deliverable**: Sistema autenticazione locale completo con documentazione

### FASE 4: APPLICATION LAYER

#### Task 4.1: Implementare Command Pattern con Handlers

**Obiettivo**: Implementare tutti i command handler per operazioni di scrittura

**Attività dettagliate:**

1. In `TaskManager.Application/Commands`:

   **Project Commands:**

   - `CreateProjectCommand` + `CreateProjectCommandHandler`
   - `UpdateProjectCommand` + `UpdateProjectCommandHandler`
   - `DeleteProjectCommand` + `DeleteProjectCommandHandler`
   - `ArchiveProjectCommand` + `ArchiveProjectCommandHandler`

   **Task Commands:**

   - `CreateTaskCommand` + `CreateTaskCommandHandler`
   - `UpdateTaskCommand` + `UpdateTaskCommandHandler`
   - `MoveTaskCommand` + `MoveTaskCommandHandler`
   - `AssignTaskCommand` + `AssignTaskCommandHandler`
   - `DeleteTaskCommand` + `DeleteTaskCommandHandler`

   **Column Commands:**

   - `CreateColumnCommand` + `CreateColumnCommandHandler`
   - `UpdateColumnCommand` + `UpdateColumnCommandHandler`
   - `ReorderColumnsCommand` + `ReorderColumnsCommandHandler`
   - `DeleteColumnCommand` + `DeleteColumnCommandHandler`

   **User Commands:**

   - `CreateUserCommand` + `CreateUserCommandHandler`
   - `UpdateUserCommand` + `UpdateUserCommandHandler`
   - `DeactivateUserCommand` + `DeactivateUserCommandHandler`

2. Implementare validation con FluentValidation per ogni command
3. Configurare pipeline behaviors per logging e validation
4. **Documentazione da aggiornare:**
   - `docs/technical/command-handlers.md` - Documentazione command handlers
   - `docs/technical/validation-rules.md` - Regole di validazione implementate

**Test Unitari da implementare:**

- Test per ogni command handler
- Test validation rules
- Test behavior pipeline

**Deliverable**: Command handlers completi con validation e documentazione

#### Task 4.2: Implementare Query Pattern con Handlers

**Obiettivo**: Implementare tutti i query handler per operazioni di lettura

**Attività dettagliate:**

1. In `TaskManager.Application/Queries`:

   **Project Queries:**

   - `GetProjectByIdQuery` + `GetProjectByIdQueryHandler`
   - `GetProjectsByOwnerQuery` + `GetProjectsByOwnerQueryHandler`
   - `GetPublicProjectsQuery` + `GetPublicProjectsQueryHandler`
   - `SearchProjectsQuery` + `SearchProjectsQueryHandler`

   **Task Queries:**

   - `GetTaskByIdQuery` + `GetTaskByIdQueryHandler`
   - `GetTasksByColumnQuery` + `GetTasksByColumnQueryHandler`
   - `GetTasksByAssigneeQuery` + `GetTasksByAssigneeQueryHandler`
   - `SearchTasksQuery` + `SearchTasksQueryHandler`
   - `GetTaskHistoryQuery` + `GetTaskHistoryQueryHandler`

   **Board Queries:**

   - `GetBoardByProjectIdQuery` + `GetBoardByProjectIdQueryHandler`
   - `GetColumnsForProjectQuery` + `GetColumnsForProjectQueryHandler`

2. Implementare DTOs ottimizzati per ogni query
3. Configurare Mapperly profiles per mapping entità->DTOs
4. Implementare caching per query frequenti
5. **Documentazione da aggiornare:**
   - `docs/technical/query-handlers.md` - Documentazione query handlers
   - `docs/technical/dto-mapping.md` - Mapping con Mapperly

**Test Unitari da implementare:**

- Test per ogni query handler
- Test mapping Mapperly
- Test caching behavior

**Deliverable**: Query handlers completi con DTOs e documentazione

#### Task 4.3: Implementare Application Services

**Obiettivo**: Creare servizi applicativi per operazioni complesse

**Attività dettagliate:**

1. Implementare `IProjectService`:

   - `CreateProjectWithDefaultColumnsAsync()`
   - `DuplicateProjectAsync(int projectId)`
   - `GetProjectStatsAsync(int projectId)`
   - `ExportProjectDataAsync(int projectId)`

2. Implementare `IBoardService`:

   - `InitializeBoardAsync(int projectId)`
   - `GetBoardViewModelAsync(int projectId, int userId)`
   - `UpdateBoardLayoutAsync(UpdateBoardLayoutRequest request)`

3. Implementare `ITaskService`:

   - `CreateTaskWithNotificationAsync(CreateTaskRequest request)`
   - `BulkUpdateTasksAsync(BulkUpdateRequest request)`
   - `GetTaskAnalyticsAsync(int projectId)`

4. Implementare `INotificationService`:

   - `SendTaskAssignedNotificationAsync(int taskId, int assigneeId)`
   - `SendTaskMovedNotificationAsync(int taskId, string fromColumn, string toColumn)`
   - `SendCommentAddedNotificationAsync(int commentId)`

5. **Documentazione da aggiornare:**
   - `docs/technical/application-services.md` - Servizi applicativi implementati
   - `docs/functional/business-processes.md` - Processi business supportati

**Test Unitari da implementare:**

- Test per ogni application service
- Test integration con notification system
- Test business logic complessa

**Deliverable**: Application services completi con documentazione

### FASE 5: WEB API LAYER

#### Task 5.1: Implementare Controllers REST API

**Obiettivo**: Creare tutti i controller per le API REST

**Attività dettagliate:**

1. In `TaskManager.WebApi/Controllers`:

   **ProjectsController:**

   - `GET /api/projects` - Lista progetti utente
   - `GET /api/projects/{id}` - Dettaglio progetto
   - `POST /api/projects` - Crea progetto
   - `PUT /api/projects/{id}` - Aggiorna progetto
   - `DELETE /api/projects/{id}` - Elimina progetto
   - `GET /api/projects/{id}/stats` - Statistiche progetto

   **TasksController:**

   - `GET /api/projects/{projectId}/tasks` - Lista task per progetto
   - `GET /api/tasks/{id}` - Dettaglio task
   - `POST /api/tasks` - Crea task
   - `PUT /api/tasks/{id}` - Aggiorna task
   - `DELETE /api/tasks/{id}` - Elimina task
   - `POST /api/tasks/{id}/move` - Sposta task
   - `POST /api/tasks/{id}/assign` - Assegna task

   **BoardsController:**

   - `GET /api/projects/{projectId}/board` - Board del progetto
   - `PUT /api/projects/{projectId}/board` - Aggiorna layout board

   **ColumnsController:**

   - `GET /api/projects/{projectId}/columns` - Colonne del progetto
   - `POST /api/projects/{projectId}/columns` - Crea colonna
   - `PUT /api/columns/{id}` - Aggiorna colonna
   - `DELETE /api/columns/{id}` - Elimina colonna
   - `PUT /api/columns/reorder` - Riordina colonne

   **UsersController:**

   - `GET /api/users/me` - Profilo utente corrente
   - `PUT /api/users/me` - Aggiorna profilo
   - `GET /api/users` - Lista utenti (admin)

2. Implementare middleware per:

   - Error handling globale
   - Request/Response logging
   - Rate limiting
   - CORS configuration

3. Configurare Swagger/OpenAPI documentation
4. **Documentazione da creare:**
   - `docs/technical/api-documentation.md` - Documentazione API completa
   - `docs/technical/api-authentication.md` - Autenticazione API
   - `docs/deployment/api-configuration.md` - Configurazione API

**Test di Integrazione da implementare:**

- Test per ogni endpoint API
- Test autenticazione e autorizzazione
- Test validation degli input
- Test error handling

**Deliverable**: API REST completa con documentazione

#### Task 5.2: Implementare Server-Sent Events (SSE) per Real-time Updates

**Obiettivo**: Aggiungere funzionalità real-time alla board con SSE

**Attività dettagliate:**

1. Creare `SseController` per gestione connessioni SSE:

   - `GET /api/projects/{projectId}/events` - Stream eventi progetto
   - Gestione connessioni persistenti
   - Heartbeat per mantenere connessioni attive

2. Implementare `IRealTimeService`:

   - `NotifyTaskMovedAsync(int projectId, TaskMovedEvent @event)`
   - `NotifyTaskUpdatedAsync(int projectId, TaskUpdatedEvent @event)`
   - `NotifyUserActivityAsync(int projectId, string activity)`

3. Integrare SSE nei command handlers per notifiche automatiche

4. Implementare connection management e filtering per progetti

5. **Documentazione da aggiornare:**
   - `docs/technical/real-time-system.md` - Sistema real-time con SSE
   - `docs/technical/sse-implementation.md` - Implementazione SSE

**Test di Integrazione da implementare:**

- Test connessione SSE
- Test notifiche real-time
- Test gestione connessioni multiple
- Test disconnessioni e riconnessioni

**Deliverable**: Sistema real-time con SSE e documentazione

#### Task 5.3: Implementare API Authentication e Authorization

**Obiettivo**: Securizzare tutte le API

**Attività dettagliate:**

1. Implementare JWT Bearer authentication
2. Creare custom authorization policies:

   - `ProjectOwnerPolicy` - Solo owner del progetto
   - `ProjectMemberPolicy` - Membri del progetto
   - `TaskAssigneePolicy` - Assegnatario del task
   - `AdminPolicy` - Solo amministratori

3. Implementare authorization handlers per policies complesse

4. Aggiungere rate limiting per prevenire abuse

5. Implementare API key support per integrations esterne

6. **Documentazione da aggiornare:**
   - `docs/technical/api-security.md` - Sicurezza API
   - `docs/deployment/security-configuration.md` - Configurazione sicurezza

**Test di Integrazione da implementare:**

- Test autenticazione JWT
- Test authorization policies
- Test rate limiting
- Test API keys

**Deliverable**: API completamente securizzate con documentazione

### FASE 6: BLAZOR FRONTEND

#### Task 6.1: Setup MudBlazor e Componenti Base

**Obiettivo**: Configurare MudBlazor e creare componenti base riutilizzabili

**Attività dettagliate:**

1. Configurare MudBlazor theme personalizzato
2. Creare layout principale:

   - `MainLayout.razor` con sidebar e header
   - `AuthLayout.razor` per pagine login/register
   - Navigation menu con icone
   - User profile dropdown

3. Creare componenti base riutilizzabili:

   - `KanboardCard.razor` - Card task personalizzata
   - `KanboardColumn.razor` - Colonna Kanban
   - `KanboardBoard.razor` - Board completa
   - `UserAvatar.razor` - Avatar utente
   - `ProjectSelector.razor` - Selettore progetti
   - `TaskModal.razor` - Modale dettaglio task
   - `ConfirmDialog.razor` - Dialog conferma azioni

4. Implementare servizi client:

   - `IApiClient` per chiamate HTTP
   - `IStateService` per stato applicazione
   - `INotificationService` per toast notifications

5. **Documentazione da creare:**
   - `docs/technical/blazor-architecture.md` - Architettura frontend Blazor
   - `docs/technical/component-library.md` - Libreria componenti custom
   - `docs/user/user-interface-guide.md` - Guida interfaccia utente

**Test UI da implementare:**

- Test rendering componenti base
- Test interazioni utente
- Test responsive design

**Deliverable**: Setup MudBlazor completo con componenti base e documentazione

#### Task 6.2: Implementare Pagine Principali

**Obiettivo**: Creare tutte le pagine principali dell'applicazione

**Attività dettagliate:**

1. **Authentication Pages:**

   - `Login.razor` - Pagina login con form validation
   - `Register.razor` - Pagina registrazione
   - `ForgotPassword.razor` - Reset password

2. **Dashboard:**

   - `Dashboard.razor` - Overview progetti e statistiche
   - Widgets per: progetti recenti, task assegnati, attività recente
   - Quick actions per creazione progetto/task

3. **Project Pages:**

   - `ProjectList.razor` - Lista progetti con filtri e ricerca
   - `ProjectDetail.razor` - Dettaglio progetto con tabs
   - `ProjectSettings.razor` - Impostazioni progetto
   - `CreateProject.razor` - Wizard creazione progetto

4. **Board Page:**

   - `Board.razor` - Board Kanban principale
   - Drag & drop implementation
   - Filtri e search box
   - Quick add task

5. **Task Pages:**

   - `TaskDetail.razor` - Dettaglio task completo
   - `TaskHistory.razor` - Storia modifiche task
   - Form per editing task con tutti i campi

6. **User Pages:**

   - `Profile.razor` - Profilo utente
   - `Settings.razor` - Impostazioni personali
   - `UserManagement.razor` - Gestione utenti (admin)

7. **Documentazione da aggiornare:**
   - `docs/user/navigation-guide.md` - Guida navigazione sistema
   - `docs/user/project-management.md` - Gestione progetti
   - `docs/user/task-management.md` - Gestione task
   - `docs/functional/user-workflows.md` - Flussi di lavoro utente

**Test UI da implementare:**

- Test navigazione tra pagine
- Test form validation
- Test state management
- Test responsive behavior

**Deliverable**: Applicazione Blazor completa con documentazione utente

#### Task 6.3: Implementare Drag & Drop e Interazioni Avanzate

**Obiettivo**: Implementare le funzionalità avanzate di UI

**Attività dettagliate:**

1. **Drag & Drop System:**

   - Implementare drag & drop per task tra colonne
   - Visual feedback durante il trascinamento
   - Validation durante il drop (limiti colonna)
   - Animazioni smooth per le transizioni

2. **Real-time Updates con SSE:**

   - Configurare SSE client in Blazor
   - Gestire aggiornamenti real-time della board
   - Mostrare utenti online sul progetto
   - Notifiche toast per aggiornamenti in tempo reale

3. **Advanced Search e Filtri:**

   - Implementare search box con syntax highlighting
   - Filtri avanzati: assignee, categoria, tag, data
   - Salvataggio filtri personalizzati
   - Search suggestions e autocomplete

4. **Keyboard Shortcuts:**

   - Shortcuts per navigazione rapida
   - Quick actions (Ctrl+N per nuovo task)
   - Accessibility support completo

5. **Responsive Design:**

   - Layout mobile-friendly
   - Touch gestures per mobile
   - Adaptive UI basata su screen size

6. **Documentazione da aggiornare:**
   - `docs/technical/drag-drop-implementation.md` - Implementazione drag & drop
   - `docs/technical/sse-client.md` - Client SSE in Blazor
   - `docs/user/advanced-features.md` - Funzionalità avanzate utente
   - `docs/user/keyboard-shortcuts.md` - Shortcuts tastiera

**Test UI da implementare:**

- Test drag & drop functionality
- Test real-time updates
- Test search e filtri
- Test keyboard shortcuts
- Test mobile responsiveness

**Deliverable**: UI avanzata con tutte le interazioni e documentazione

### FASE 7: TESTING COMPLETO

#### Task 7.1: Test Unitari Completi

**Obiettivo**: Raggiungere copertura di test >90% per tutta la business logic

**Attività dettagliate:**

1. **Domain Layer Tests:**

   - Test per tutte le entità e value objects
   - Test per domain services e business rules
   - Test per domain events
   - Test per validazioni e constraints

2. **Application Layer Tests:**

   - Test per tutti i command handlers
   - Test per tutti i query handlers
   - Test per application services
   - Test per validation behaviors
   - Test per mapping profiles Mapperly

3. **Infrastructure Layer Tests:**

   - Test per repositories
   - Test per database configurations
   - Test per external services integration
   - Test per caching mechanisms

4. **Test Utilities:**

   - Builder pattern per creazione test data
   - Custom assertions per domain objects
   - Test fixtures e utilities condivise

5. **Documentazione da creare:**
   - `docs/technical/unit-testing-guide.md` - Guida test unitari
   - `docs/technical/test-coverage-report.md` - Report copertura test

**Metriche da raggiungere:**

- Code coverage >90%
- Tutti i critical paths testati
- Edge cases e error scenarios coperti

**Deliverable**: Suite di test unitari completa con documentazione

#### Task 7.2: Test di Integrazione API

**Obiettivo**: Testare tutti gli endpoint API end-to-end

**Attività dettagliate:**

1. **Setup Integration Test Environment:**

   - Database in-memory per test isolati
   - Mock per servizi esterni
   - Test data builders e seeders

2. **API Endpoint Tests:**

   - Test per tutti i controller endpoints
   - Test authentication e authorization
   - Test input validation
   - Test error handling e status codes
   - Test performance per endpoint critici

3. **Integration Scenarios:**

   - Test flussi business completi
   - Test transazioni database
   - Test concurrent operations
   - Test SSE functionality

4. **Load Testing:**

   - Test carico per API critiche
   - Test stress con molti utenti simultanei
   - Test memory leaks e performance

5. **Documentazione da aggiornare:**
   - `docs/technical/integration-testing.md` - Guida test integrazione
   - `docs/technical/api-testing-scenarios.md` - Scenari test API
   - `docs/deployment/performance-benchmarks.md` - Benchmark performance

**Test Scenarios:**

- Creazione progetto con board e colonne default
- Flusso completo spostamento task
- Authentication flow completo
- Scenario multi-utente con conflitti

**Deliverable**: Test di integrazione completi con documentazione

#### Task 7.3: Test UI Blazor

**Obiettivo**: Testare tutti i componenti e le interazioni UI

**Attività dettagliate:**

1. **Component Unit Tests (bUnit):**

   - Test rendering per tutti i componenti
   - Test prop binding e data flow
   - Test event handling
   - Test conditional rendering

2. **Page Integration Tests:**

   - Test navigazione tra pagine
   - Test form submissions
   - Test data loading e error states
   - Test authentication flows

3. **UI Interaction Tests:**

   - Test drag & drop functionality
   - Test modal dialogs
   - Test responsive behavior
   - Test keyboard navigation

4. **End-to-End Tests (Playwright):**

   - Test user journeys completi
   - Test cross-browser compatibility
   - Test mobile responsiveness
   - Test performance UI

5. **Documentazione da creare:**
   - `docs/technical/ui-testing-guide.md` - Guida test UI
   - `docs/technical/e2e-testing-scenarios.md` - Scenari test E2E
   - `docs/user/browser-compatibility.md` - Compatibilità browser

**Critical UI Flows da testare:**

- Login → Dashboard → Crea Progetto → Board → Crea Task → Sposta Task
- Registrazione nuovo utente
- Gestione profilo utente
- Search e filtri avanzati

**Deliverable**: Test UI completi con documentazione

### FASE 8: FEATURES AVANZATE

#### Task 8.1: Sistema di Notifiche

**Obiettivo**: Implementare sistema completo di notifiche

**Attività dettagliate:**

1. **Email Notifications:**

   - Configurare SMTP provider
   - Template email responsive
   - Notifiche per: task assigned, due date, comments, mentions
   - Preferenze utente per tipi di notifica

2. **In-App Notifications:**

   - Centro notifiche nell'header
   - Notifiche real-time via SSE
   - Mark as read/unread functionality
   - Notifiche persistenti nel database

3. **Webhook System:**

   - API per configurazione webhook
   - Eventi webhook per integrazioni esterne
   - Retry logic per webhook falliti
   - Webhook security (signatures)

4. **Mobile Push Notifications (futura):**

   - Setup per PWA notifications
   - Service worker configuration

5. **Documentazione da aggiornare:**
   - `docs/technical/notification-system.md` - Sistema notifiche
   - `docs/user/notification-settings.md` - Impostazioni notifiche
   - `docs/technical/webhook-integration.md` - Integrazione webhook

**Deliverable**: Sistema notifiche completo con documentazione

#### Task 8.2: Import/Export e API Estesa

**Obiettivo**: Funzionalità avanzate per integrazione e backup

**Attività dettagliate:**

1. **Export Functionality:**

   - Export progetti in JSON/CSV
   - Export board as image/PDF
   - Backup completo database
   - Export per analytics (Excel)

2. **Import Functionality:**

   - Import da file CSV/JSON
   - Import da altri sistemi Kanban
   - Validation e error handling
   - Preview prima dell'import

3. **API Estesa:**

   - Bulk operations API
   - Search API avanzata
   - Analytics API
   - Reporting API

4. **Integrations:**

   - GitHub integration (issues sync)
   - Slack integration (notifications)
   - Calendar integration (due dates)

5. **Documentazione da aggiornare:**
   - `docs/user/import-export-guide.md` - Guida import/export
   - `docs/technical/api-extensions.md` - API estese
   - `docs/technical/third-party-integrations.md` - Integrazioni esterne

**Deliverable**: Sistema import/export e integrazioni con documentazione

#### Task 8.3: Analytics e Reporting

**Obiettivo**: Dashboard analytics per project management

**Attività dettagliate:**

1. **Project Analytics:**

   - Burndown charts
   - Velocity tracking
   - Time in column analytics
   - Task completion trends

2. **User Analytics:**

   - User productivity metrics
   - Workload distribution
   - Time tracking per user

3. **Dashboard Reporting:**

   - Executive dashboard
   - Custom report builder
   - Scheduled reports
   - Export reports functionality

4. **Performance Monitoring:**

   - Application performance metrics
   - User activity tracking
   - Error monitoring e logging

5. **Documentazione da aggiornare:**
   - `docs/user/analytics-dashboard.md` - Dashboard analytics
   - `docs/user/reporting-guide.md` - Guida reporting
   - `docs/technical/metrics-collection.md` - Raccolta metriche

**Deliverable**: Sistema analytics completo con documentazione

### FASE 9: DEPLOYMENT E DEVOPS

#### Task 9.1: Containerization e Docker

**Obiettivo**: Preparare l'applicazione per deployment

**Attività dettagliate:**

1. **Docker Configuration:**

   - Dockerfile multi-stage per WebAPI
   - Dockerfile per BlazorApp
   - docker-compose.yml per development
   - docker-compose.prod.yml per production

2. **Database Migration Strategy:**

   - Automatic migrations on startup
   - Database initialization scripts
   - Backup/restore procedures

3. **Environment Configuration:**

   - Configuration per development/staging/production
   - Secrets management
   - Environment variables

4. **Health Checks:**

   - Application health endpoints
   - Database connectivity checks
   - Dependencies health monitoring

5. **Documentazione da creare:**
   - `docs/deployment/docker-setup.md` - Setup Docker
   - `docs/deployment/environment-configuration.md` - Configurazione ambienti
   - `docs/deployment/database-migration-guide.md` - Guida migrazione DB
   - `docs/deployment/health-monitoring.md` - Monitoraggio salute sistema

**Deliverable**: Applicazione containerizzata con documentazione deploy

#### Task 9.2: CI/CD Pipeline

**Obiettivo**: Automatizzare build, test e deployment

**Attività dettagliate:**

1. **GitHub Actions Setup:**

   - Build pipeline per ogni commit
   - Automated testing su PR
   - Code quality checks (SonarQube)
   - Security scanning

2. **Deployment Pipeline:**

   - Automated deployment to staging
   - Manual approval per production
   - Blue/green deployment strategy
   - Rollback procedures

3. **Quality Gates:**

   - Test coverage requirements
   - Code quality thresholds
   - Security vulnerability checks
   - Performance benchmarks

4. **Monitoring e Logging:**

   - Application logging (Serilog)
   - Error tracking (Application Insights)
   - Performance monitoring
   - User analytics

5. **Documentazione da aggiornare:**
   - `docs/deployment/ci-cd-pipeline.md` - Pipeline CI/CD
   - `docs/deployment/deployment-procedures.md` - Procedure deployment
   - `docs/deployment/monitoring-setup.md` - Setup monitoraggio
   - `docs/deployment/troubleshooting.md` - Risoluzione problemi

**Deliverable**: Pipeline CI/CD completa con documentazione

#### Task 9.3: Production Readiness

**Obiettivo**: Preparare per deployment in produzione

**Attività dettagliate:**

1. **Security Hardening:**

   - Security headers configuration
   - HTTPS enforcement
   - Rate limiting implementation
   - Input sanitization review

2. **Performance Optimization:**

   - Database query optimization
   - Caching implementation
   - CDN configuration
   - Image optimization

3. **Scalability Preparation:**

   - Load balancer configuration
   - Database connection pooling
   - Session state management
   - Auto-scaling configuration

4. **Documentation:**

   - API documentation (Swagger)
   - Deployment guide
   - User manual
   - Admin guide

5. **Documentazione da aggiornare:**
   - `docs/deployment/production-checklist.md` - Checklist produzione
   - `docs/deployment/security-hardening.md` - Hardening sicurezza
   - `docs/deployment/performance-optimization.md` - Ottimizzazione performance
   - `docs/deployment/scaling-guide.md` - Guida scaling

**Deliverable**: Applicazione production-ready con documentazione completa

### FASE 10: DOCUMENTAZIONE FINALE E HANDOVER

#### Task 10.1: Completamento Documentazione Tecnica

**Obiettivo**: Finalizzare documentazione completa per sviluppatori

**Attività dettagliate:**

1. **Architecture Documentation:**

   - `docs/technical/system-architecture-final.md` - Architettura sistema finale
   - `docs/technical/database-schema-final.md` - Schema database finale
   - `docs/technical/api-documentation-complete.md` - Documentazione API completa
   - `docs/technical/component-diagrams.md` - Diagrammi componenti

2. **Developer Guide:**

   - `docs/technical/development-setup-complete.md` - Setup completo sviluppo
   - `docs/technical/coding-standards.md` - Standard e convenzioni
   - `docs/technical/testing-guidelines-final.md` - Linee guida testing finali
   - `docs/technical/deployment-procedures-final.md` - Procedure deployment finali

3. **API Documentation:**

   - Aggiornamento OpenAPI/Swagger documentation
   - `docs/technical/authentication-guide-final.md` - Guida autenticazione finale
   - `docs/technical/rate-limiting-documentation.md` - Documentazione rate limiting
   - `docs/technical/sdk-examples.md` - Esempi SDK

4. **Technical Reference:**
   - `docs/technical/troubleshooting-guide.md` - Guida risoluzione problemi
   - `docs/technical/performance-tuning.md` - Tuning performance
   - `docs/technical/security-guidelines.md` - Linee guida sicurezza

**Deliverable**: Documentazione tecnica completa e finale

#### Task 10.2: Completamento Documentazione Funzionale

**Obiettivo**: Finalizzare documentazione per business e utenti

**Attività dettagliate:**

1. **Functional Specification:**

   - `docs/functional/requirements-complete.md` - Requisiti completi implementati
   - `docs/functional/business-processes-final.md` - Processi business finali
   - `docs/functional/user-roles-permissions.md` - Ruoli e permessi utente
   - `docs/functional/workflow-diagrams.md` - Diagrammi workflow

2. **Feature Documentation:**

   - `docs/functional/feature-specifications.md` - Specifiche funzionalità
   - `docs/functional/integration-capabilities.md` - Capacità integrazione
   - `docs/functional/reporting-analytics.md` - Reporting e analytics
   - `docs/functional/notification-system.md` - Sistema notifiche

3. **Business Documentation:**
   - `docs/functional/business-value.md` - Valore business
   - `docs/functional/roi-metrics.md` - Metriche ROI
   - `docs/functional/competitive-analysis.md` - Analisi competitiva

**Deliverable**: Documentazione funzionale completa

#### Task 10.3: Manuali Utente e Deploy Finali

**Obiettivo**: Creare manuali definitivi per utenti finali e amministratori

**Attività dettagliate:**

1. **User Manual Completo:**

   - `docs/user/getting-started-complete.md` - Guida introduttiva completa
   - `docs/user/feature-walkthrough-complete.md` - Tour funzionalità completo
   - `docs/user/best-practices-guide.md` - Guida best practices
   - `docs/user/faq-troubleshooting.md` - FAQ e risoluzione problemi utente

2. **Admin Guide Completo:**

   - `docs/user/admin-manual-complete.md` - Manuale amministratore completo
   - `docs/user/user-management-guide.md` - Guida gestione utenti
   - `docs/user/system-configuration.md` - Configurazione sistema
   - `docs/user/backup-procedures.md` - Procedure backup

3. **Deploy Manual Finale:**

   - `docs/deployment/complete-deployment-guide.md` - Guida deployment completa
   - `docs/deployment/installation-procedures.md` - Procedure installazione
   - `docs/deployment/upgrade-procedures.md` - Procedure upgrade
   - `docs/deployment/disaster-recovery.md` - Disaster recovery

4. **Training Materials:**
   - `docs/user/training-materials.md` - Materiali training
   - `docs/user/video-tutorials-scripts.md` - Script video tutorial
   - `docs/user/onboarding-checklist.md` - Checklist onboarding

**Deliverable**: Manuali completi per utenti e deploy

#### Task 10.4: Documentazione di Progetto e Handover

**Obiettivo**: Creare documentazione di progetto finale per handover

**Attività dettagliate:**

1. **Project Documentation:**

   - `docs/project/project-summary.md` - Riassunto progetto
   - `docs/project/implementation-timeline.md` - Timeline implementazione
   - `docs/project/deliverables-checklist.md` - Checklist deliverable
   - `docs/project/lessons-learned.md` - Lezioni apprese

2. **Quality Assurance:**

   - `docs/project/testing-summary.md` - Riassunto testing
   - `docs/project/code-quality-metrics.md` - Metriche qualità codice
   - `docs/project/performance-benchmarks.md` - Benchmark performance
   - `docs/project/security-assessment.md` - Assessment sicurezza

3. **Handover Documentation:**

   - `docs/project/handover-checklist.md` - Checklist handover
   - `docs/project/support-procedures.md` - Procedure supporto
   - `docs/project/maintenance-guide.md` - Guida manutenzione
   - `docs/project/future-enhancements.md` - Miglioramenti futuri

4. **Final Documentation Index:**
   - `docs/README.md` - Indice documentazione principale
   - `docs/documentation-map.md` - Mappa documentazione
   - `docs/version-history.md` - Storia versioni documentazione

**Deliverable**: Documentazione progetto completa per handover

## 4. PRIORITÀ DELLE FASI

### Priorità delle Fasi:

1. **CRITICA (Fasi 1-5)**: Foundation e core functionality
2. **ALTA (Fase 6)**: Frontend Blazor
3. **MEDIA (Fase 7)**: Testing completo
4. **BASSA (Fasi 8-10)**: Features avanzate e documentation

### Milestone Principali:

- **Milestone 1** (Fine Fase 3): Backend foundation completo
- **Milestone 2** (Fine Fase 5): API complete e funzionanti
- **Milestone 3** (Fine Fase 6): Applicazione completa end-to-end
- **Milestone 4** (Fine Fase 7): Applicazione testata e stabile
- **Milestone 5** (Fine Fase 10): Prodotto finale deployabile con documentazione completa

## 5. CONSIDERAZIONI TECNICHE AGGIUNTIVE

### 5.1 Performance Requirements

- Response time API < 200ms per 95% delle richieste
- UI responsiva < 100ms per interazioni comuni
- Supporto fino a 1000 utenti concorrenti
- Database ottimizzato per query complesse

### 5.2 Security Requirements

- Autenticazione sicura con JWT e ASP.NET Core Identity
- Autorizzazione granulare per risorse
- Input validation completa
- Protection contro OWASP Top 10

### 5.3 Scalability Considerations

- Architecture modulare per scaling orizzontale
- Database design ottimizzato per growth
- Caching strategy per performance
- CDN ready per static assets

### 5.4 Browser Support

- Chrome 90+, Firefox 88+, Safari 14+, Edge 90+
- Progressive Web App capabilities
- Mobile responsive design
- Offline capabilities (future enhancement)

### 5.5 Documentazione Requirements

- Documentazione tecnica completa per sviluppatori
- Documentazione funzionale per business stakeholder
- Manuali utente per end users
- Manuali deploy per amministratori di sistema
- Documentazione aggiornata ad ogni task

Questo piano fornisce una roadmap completa per implementare un clone di Kanboard in .NET Core 9.0 con tutte le funzionalità core, test completi, documentazione esaustiva e preparazione per produzione. Ogni task include specifiche attività di documentazione per garantire che il progetto sia completamente documentato durante l'implementazione.
