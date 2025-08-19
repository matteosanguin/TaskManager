# Domain Model

## Panoramica

Questo documento descrive il modello di dominio completo per l'applicazione TaskManager. Il modello è stato progettato seguendo i principi della Clean Architecture e implementa il pattern Entity-Relationship completo per un sistema Kanban.

## Entità del Dominio

### 1. User

Rappresenta un utente del sistema.

**Proprietà:**

- `Id`: Guid - Identificatore univoco
- `Username`: string - Nome utente univoco
- `Email`: Email (Value Object) - Indirizzo email
- `PasswordHash`: string - Hash della password
- `Role`: string - Ruolo dell'utente (Admin, User, etc.)
- `CreatedAt`: DateTime - Data di creazione
- `Theme`: string - Tema dell'interfaccia utente
- `TimeZone`: string - Fuso orario dell'utente
- `IsActive`: bool - Stato attivo/disattivo dell'utente

### 2. Project

Rappresenta un progetto Kanban.

**Proprietà:**

- `Id`: Guid - Identificatore univoco
- `Name`: string - Nome del progetto
- `Description`: string - Descrizione del progetto
- `Identifier`: string - Identificatore univoco leggibile (es. "PROJ-001")
- `OwnerId`: Guid - ID dell'utente proprietario
- `IsActive`: bool - Stato attivo/disattivo del progetto
- `IsPrivate`: bool - Visibilità del progetto
- `StartDate`: DateTime? - Data di inizio
- `EndDate`: DateTime? - Data di fine
- `TaskLimit`: int? - Limite massimo di task per il progetto
- `CreatedAt`: DateTime - Data di creazione
- `ModifiedAt`: DateTime - Data di ultima modifica

### 3. Board

Rappresenta una board Kanban associata a un progetto.

**Proprietà:**

- `Id`: Guid - Identificatore univoco
- `Name`: string - Nome della board
- `ProjectId`: Guid - ID del progetto associato
- `CreatedAt`: DateTime - Data di creazione

### 4. Column

Rappresenta una colonna nella board Kanban.

**Proprietà:**

- `Id`: Guid - Identificatore univoco
- `Title`: string - Titolo della colonna
- `Position`: int - Posizione della colonna nella board
- `ProjectId`: Guid - ID del progetto associato
- `TaskLimit`: int? - Limite massimo di task nella colonna
- `Description`: string - Descrizione della colonna
- `HideInDashboard`: bool - Nascondi la colonna nella dashboard

### 5. Task

Rappresenta un task nella board Kanban.

**Proprietà:**

- `Id`: Guid - Identificatore univoco
- `Title`: string - Titolo del task
- `Description`: string - Descrizione del task
- `ProjectId`: Guid - ID del progetto associato
- `ColumnId`: Guid - ID della colonna corrente
- `AssigneeId`: Guid? - ID dell'utente assegnatario
- `CreatorId`: Guid - ID dell'utente creatore
- `Position`: int - Posizione del task nella colonna
- `Priority`: Priority (Value Object) - Priorità del task
- `DueDate`: DateTime? - Data di scadenza
- `CreatedAt`: DateTime - Data di creazione
- `ModifiedAt`: DateTime - Data di ultima modifica
- `TimeEstimated`: int? - Tempo stimato in minuti
- `TimeSpent`: int? - Tempo effettivamente speso in minuti

### 6. Comment

Rappresenta un commento su un task.

**Proprietà:**

- `Id`: Guid - Identificatore univoco
- `TaskId`: Guid - ID del task associato
- `UserId`: Guid - ID dell'utente autore
- `Content`: string - Contenuto del commento
- `CreatedAt`: DateTime - Data di creazione
- `Visibility`: string - Visibilità del commento (Public, Private, etc.)

### 7. Category

Rappresenta una categoria per i task di un progetto.

**Proprietà:**

- `Id`: Guid - Identificatore univoco
- `Name`: string - Nome della categoria
- `ProjectId`: Guid - ID del progetto associato
- `Description`: string - Descrizione della categoria
- `Color`: string - Colore associato alla categoria

### 8. Tag

Rappresenta un tag per i task di un progetto.

**Proprietà:**

- `Id`: Guid - Identificatore univoco
- `Name`: string - Nome del tag
- `ProjectId`: Guid - ID del progetto associato
- `Color`: string - Colore associato al tag

### 9. TaskTag

Entità di relazione many-to-many tra Task e Tag.

**Proprietà:**

- `TaskId`: Guid - ID del task
- `TagId`: Guid - ID del tag

### 10. Swimlane

Rappresenta una swimlane (riga) nella board Kanban.

**Proprietà:**

- `Id`: Guid - Identificatore univoco
- `Name`: string - Nome della swimlane
- `ProjectId`: Guid - ID del progetto associato
- `Position`: int - Posizione della swimlane nella board
- `IsActive`: bool - Stato attivo/disattivo della swimlane
- `TaskLimit`: int? - Limite massimo di task nella swimlane

### 11. Attachment

Rappresenta un allegato associato a un task.

**Proprietà:**

- `Id`: Guid - Identificatore univoco
- `TaskId`: Guid - ID del task associato
- `FileName`: string - Nome del file
- `FilePath`: string - Percorso del file
- `FileSize`: long - Dimensione del file in byte
- `UploadedAt`: DateTime - Data di upload
- `UploadedById`: Guid - ID dell'utente che ha caricato il file

## Value Objects

### Email

Rappresenta un indirizzo email valido.

**Proprietà:**

- `Value`: string - L'indirizzo email

**Validazioni:**

- Formato email valido
- Non null o vuoto

### Priority

Rappresenta la priorità di un task.

**Valori possibili:**

- `Low`
- `Normal`
- `High`
- `Urgent`

## Relazioni tra Entità

### Relazioni Principali:

1. **User - Project**:

   - One-to-Many (Un utente può possedere molti progetti)
   - Foreign Key: Project.OwnerId → User.Id

2. **Project - Board**:

   - One-to-One (Un progetto ha una board)
   - Foreign Key: Board.ProjectId → Project.Id

3. **Project - Column**:

   - One-to-Many (Un progetto può avere molte colonne)
   - Foreign Key: Column.ProjectId → Project.Id

4. **Project - Task**:

   - One-to-Many (Un progetto può avere molti task)
   - Foreign Key: Task.ProjectId → Project.Id

5. **Column - Task**:

   - One-to-Many (Una colonna può contenere molti task)
   - Foreign Key: Task.ColumnId → Column.Id

6. **User - Task (Assignee)**:

   - One-to-Many (Un utente può essere assegnatario di molti task)
   - Foreign Key: Task.AssigneeId → User.Id

7. **User - Task (Creator)**:

   - One-to-Many (Un utente può creare molti task)
   - Foreign Key: Task.CreatorId → User.Id

8. **Task - Comment**:

   - One-to-Many (Un task può avere molti commenti)
   - Foreign Key: Comment.TaskId → Task.Id

9. **User - Comment**:

   - One-to-Many (Un utente può scrivere molti commenti)
   - Foreign Key: Comment.UserId → User.Id

10. **Project - Category**:

    - One-to-Many (Un progetto può avere molte categorie)
    - Foreign Key: Category.ProjectId → Project.Id

11. **Project - Tag**:

    - One-to-Many (Un progetto può avere molti tag)
    - Foreign Key: Tag.ProjectId → Project.Id

12. **Task - Tag**:

    - Many-to-Many (Un task può avere molti tag e un tag può essere associato a molti task)
    - Tabella di relazione: TaskTag

13. **Project - Swimlane**:

    - One-to-Many (Un progetto può avere molte swimlane)
    - Foreign Key: Swimlane.ProjectId → Project.Id

14. **Task - Attachment**:

    - One-to-Many (Un task può avere molti allegati)
    - Foreign Key: Attachment.TaskId → Task.Id

15. **User - Attachment**:
    - One-to-Many (Un utente può caricare molti allegati)
    - Foreign Key: Attachment.UploadedById → User.Id

## Considerazioni Architetturali

1. **Ereditarietà**: Tutte le entità ereditano da `BaseEntity` che fornisce proprietà comuni come `Id`, `CreatedAt`, `UpdatedAt`.

2. **Navigation Properties**: Ogni entità avrà navigation properties appropriate per facilitare il caricamento delle relazioni.

3. **Validazione**: Le entità implementano validazione a livello di dominio per garantire l'integrità dei dati.

4. **Value Objects**: Dove appropriato, vengono utilizzati Value Objects per rappresentare concetti che hanno identità propria ma non ciclo di vita indipendente.

5. **Nullable Properties**: Le proprietà opzionali sono marcate come nullable dove appropriato.

## Futura Implementazione

L'implementazione di queste entità seguirà questi passi:

1. Creazione dei Value Objects (Email, Priority)
2. Creazione delle entità con proprietà base
3. Aggiunta delle Navigation Properties
4. Configurazione delle relazioni nel DbContext
5. Aggiornamento della documentazione
6. Implementazione dei test unitari
