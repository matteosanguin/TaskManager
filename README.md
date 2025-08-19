# TaskManager - Clone di Kanboard in .NET

## Panoramica

TaskManager è un clone open source di Kanboard, un'applicazione di project management basata sulla metodologia Kanban. Questo progetto è stato sviluppato utilizzando tecnologie .NET moderne per fornire un'esperienza di gestione dei progetti semplice e intuitiva.

## Caratteristiche Principali

- Gestione completa di progetti e board Kanban
- Tracciamento avanzato dei task con assegnazione, priorità e scadenze
- Sistema di autenticazione locale con ruoli e permessi
- Notifiche in tempo reale tramite Server-Sent Events (SSE)
- API REST completa per integrazioni esterne
- Interfaccia utente moderna realizzata con Blazor Server e MudBlazor
- Architettura pulita basata su Clean Architecture
- Testing completo con copertura >90%

## Stack Tecnologico

- **Backend**: ASP.NET Core 9.0 Web API
- **Database**: SQLite con Entity Framework Core
- **Frontend**: Blazor Server con MudBlazor
- **Autenticazione**: ASP.NET Core Identity
- **Testing**: xUnit, Moq, bUnit, Playwright
- **API**: RESTful API + Server-Sent Events (SSE) per aggiornamenti in tempo reale
- **Mapping**: Mapperly
- **Pattern**: Command/Query Pattern con mediator custom

## Architettura del Progetto

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

## Fasi di Implementazione

1. **Setup Iniziale e Fondamenta** - Struttura del progetto, database, testing
2. **Domain Layer e Entità** - Implementazione delle entità e della logica business
3. **Infrastructure Layer** - Repository pattern, configurazione EF Core, autenticazione
4. **Application Layer** - Command/Query handlers, servizi applicativi
5. **Web API Layer** - Controllers REST, SSE per real-time updates, sicurezza
6. **Blazor Frontend** - Interfaccia utente, drag & drop, interazioni avanzate
7. **Testing Completo** - Test unitari, integrazione API, test UI
8. **Features Avanzate** - Sistema notifiche, import/export, analytics
9. **Deployment e DevOps** - Containerization, CI/CD, production readiness
10. **Documentazione Finale** - Manuali completi per utenti e sviluppatori

## Requisiti di Sistema

- .NET 9.0 SDK
- SQLite (per lo sviluppo)
- Docker (opzionale, per deployment)
- Node.js (per alcuni strumenti di sviluppo)

## Installazione e Avvio

1. Clonare il repository
2. Aprire la solution in Visual Studio o VS Code
3. Eseguire il comando `dotnet restore` per installare le dipendenze
4. Configurare la stringa di connessione al database in `appsettings.json`
5. Eseguire le migration con `dotnet ef database update`
6. Avviare il progetto con `dotnet run` o dal proprio IDE

## Documentazione

La documentazione completa è disponibile nella cartella `docs/`:

- [Documentazione Tecnica](docs/technical/)
- [Documentazione Funzionale](docs/functional/)
- [Manuale Utente](docs/user/)
- [Guida al Deployment](docs/deployment/)

## Contribuire al Progetto

1. Forkare il repository
2. Creare un feature branch (`git checkout -b feature/NomeFeature`)
3. Committare le modifiche (`git commit -am 'Aggiunta nuova feature'`)
4. Pushare il branch (`git push origin feature/NomeFeature`)
5. Aprire una Pull Request

## Licenza

Questo progetto è distribuito sotto la licenza MIT. Consultare il file [LICENSE](LICENSE) per ulteriori informazioni.

## Contatti

Per qualsiasi domanda o suggerimento, aprire una issue su GitHub.
