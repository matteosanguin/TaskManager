# Business Rules

## Panoramica

Questo documento descrive le regole business implementate nel sistema TaskManager. Le business rules definiscono i vincoli e le logiche che governano il comportamento dell'applicazione.

## Domain Services

### TaskMovementService

Il `TaskMovementService` gestisce lo spostamento dei task tra le colonne della board Kanban.

#### Regole implementate:

1. **Validazione progetto**: Un task può essere spostato solo in una colonna che appartiene allo stesso progetto.
2. **Controllo limite colonna**: Non è possibile spostare un task in una colonna che ha raggiunto il limite massimo di task.
3. **Prevenzione spostamento duplicato**: Un task non può essere spostato nella colonna in cui si trova già.
4. **Calcolo posizione**: La posizione del task nella colonna di destinazione viene calcolata automaticamente alla fine della colonna.
5. **Aggiornamento data modifica**: La data di modifica del task viene aggiornata ogni volta che il task viene spostato.

#### Metodi principali:

- `MoveTask(Task task, Column targetColumn)`: Sposta un task in una colonna
- `MoveTask(Task task, Column targetColumn, int position)`: Sposta un task in una posizione specifica di una colonna
- `CanMoveTask(Task task, Column targetColumn)`: Verifica se è possibile spostare un task

### ProjectPermissionService

Il `ProjectPermissionService` gestisce i permessi di accesso ai progetti.

#### Regole implementate:

1. **Proprietario progetto**: Il proprietario di un progetto ha accesso completo in lettura e scrittura.
2. **Accesso progetti pubblici**: I progetti pubblici possono essere letti da tutti gli utenti.
3. **Accesso progetti privati**: Solo il proprietario può accedere ai progetti privati.
4. **Controllo scrittura**: Solo il proprietario può modificare un progetto.

#### Metodi principali:

- `CanReadProject(User user, Project project)`: Verifica se un utente può leggere un progetto
- `CanWriteProject(User user, Project project)`: Verifica se un utente può modificare un progetto
- `IsProjectOwner(User user, Project project)`: Verifica se un utente è il proprietario di un progetto
- `CanDeleteProject(User user, Project project)`: Verifica se un utente può eliminare un progetto

### TaskAssignmentService

Il `TaskAssignmentService` gestisce l'assegnazione dei task agli utenti.

#### Regole implementate:

1. **Controllo accesso progetto**: Un utente può essere assegnato a un task solo se ha accesso al progetto del task.
2. **Prevenzione assegnazione duplicata**: Un utente non può essere assegnato a un task a cui è già assegnato.
3. **Aggiornamento data modifica**: La data di modifica del task viene aggiornata ogni volta che viene modificata l'assegnazione.

#### Metodi principali:

- `AssignTask(Task task, User user)`: Assegna un task a un utente
- `UnassignTask(Task task)`: Rimuove l'assegnazione di un task
- `CanAssignUserToTask(Task task, User user)`: Verifica se un utente può essere assegnato a un task
- `IsUserAssignedToTask(Task task, User user)`: Verifica se un utente è assegnato a un task

## Domain Events

### TaskMovedEvent

Generato quando un task viene spostato da una colonna all'altra.

### TaskAssignedEvent

Generato quando un task viene assegnato a un utente.

### ProjectCreatedEvent

Generato quando un nuovo progetto viene creato.

### CommentAddedEvent

Generato quando un commento viene aggiunto a un task.
