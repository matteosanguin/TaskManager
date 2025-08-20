# Flussi di Lavoro Principali

## Panoramica

Questo documento descrive i flussi di lavoro principali dell'applicazione TaskManager, inclusi i processi di gestione dei progetti, dei task e delle board Kanban.

## Flusso di Creazione Progetto

1. L'utente accede all'applicazione
2. L'utente seleziona "Crea nuovo progetto"
3. L'utente inserisce il nome e la descrizione del progetto
4. Il sistema crea automaticamente una board Kanban per il progetto
5. Il sistema genera colonne di default (Backlog, Ready, In Progress, Done)
6. Il progetto viene assegnato all'utente come proprietario
7. L'utente può invitare altri membri del team al progetto

## Flusso di Gestione Task

### Creazione Task

1. L'utente apre una board Kanban
2. L'utente seleziona "Aggiungi task" in una colonna
3. L'utente inserisce il titolo, la descrizione e altre informazioni del task
4. Il task viene creato nella colonna selezionata
5. Il task viene assegnato automaticamente all'utente che lo ha creato

### Spostamento Task

1. L'utente trascina un task da una colonna all'altra
2. Il sistema verifica che lo spostamento sia possibile:
   - Il task appartiene allo stesso progetto della colonna di destinazione
   - La colonna di destinazione non ha raggiunto il limite massimo di task
   - Il task non è già nella colonna di destinazione
3. Se le verifiche passano, il task viene spostato
4. La posizione del task nella colonna di destinazione viene calcolata
5. La data di modifica del task viene aggiornata
6. Un evento `TaskMovedEvent` viene generato

### Assegnazione Task

1. L'utente apre i dettagli di un task
2. L'utente seleziona "Assegna a" e sceglie un membro del team
3. Il sistema verifica che l'utente possa essere assegnato al task:
   - L'utente ha accesso al progetto del task
   - L'utente non è già assegnato al task
4. Se le verifiche passano, il task viene assegnato all'utente
5. La data di modifica del task viene aggiornata
6. Un evento `TaskAssignedEvent` viene generato

## Flusso di Collaborazione in Tempo Reale

### Aggiornamenti Board

1. Un utente effettua un'azione sulla board (spostamento task, modifica task, ecc.)
2. L'azione viene processata dal server
3. Un evento viene generato per l'azione
4. Il server invia l'evento a tutti gli utenti connessi alla board
5. Gli utenti ricevono l'aggiornamento in tempo reale senza dover ricaricare la pagina

### Notifiche

1. Quando un task viene assegnato a un utente, una notifica viene generata
2. Quando un commento viene aggiunto a un task, una notifica viene generata per gli utenti interessati
3. Le notifiche vengono inviate in tempo reale agli utenti connessi
4. Le notifiche vengono archiviate per gli utenti non connessi

## Flusso di Gestione Progetti

### Modifica Progetto

1. Il proprietario del progetto accede alle impostazioni del progetto
2. Il proprietario può modificare il nome, la descrizione e altre impostazioni
3. Solo il proprietario può effettuare modifiche al progetto

### Eliminazione Progetto

1. Il proprietario del progetto seleziona "Elimina progetto"
2. Il sistema verifica che l'utente sia il proprietario del progetto
3. Il sistema mostra un avviso di conferma
4. Se l'utente conferma, il progetto e tutti i dati associati vengono eliminati

### Controllo Accessi

1. Il proprietario del progetto può invitare membri del team
2. I membri del team possono visualizzare i progetti pubblici
3. Solo il proprietario può modificare i progetti
4. I permessi vengono verificati ogni volta che un utente tenta di accedere a un progetto
