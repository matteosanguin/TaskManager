Panoramica del Progetto

TaskManager è un'applicazione open-source di project management che reimplementa le funzionalità core di Kanboard utilizzando tecnologie .NET moderne. Il progetto mira a fornire un'esperienza di gestione progetti basata su Kanban pulita e intuitiva, con funzionalità avanzate di collaborazione in tempo reale.
Obiettivi Principali

    Creare un clone completamente funzionale di Kanboard con parità di funzionalità
    Implementare un'architettura .NET moderna seguendo i principi della Clean Architecture
    Fornire collaborazione in tempo reale tramite Server-Sent Events (SSE)
    Raggiungere una copertura di test completa (>90%)
    Consegnare un deployment production-ready con documentazione completa

Funzionalità Chiave

    Gestione Board Kanban: Gestione task drag-and-drop con colonne e swimlane personalizzabili
    Organizzazione Progetti: Supporto multi-progetto con controllo accessi basato su ruoli
    Gestione Task: Ciclo di vita completo dei task con assegnazioni, priorità, scadenze e commenti
    Aggiornamenti Real-time: Sincronizzazione live della board tra più utenti via SSE
    Autenticazione Utenti: Sistema di autenticazione locale con ruoli e permessi
    API REST: API RESTful completa per integrazioni esterne
    Ricerca e Filtri: Ricerca avanzata dei task con linguaggio di query personalizzato
    UI Responsiva: Interfaccia Blazor moderna ottimizzata per desktop e mobile

Stack Tecnologico

    Backend: ASP.NET Core 9.0 Web API
    Frontend: Blazor Server con componenti UI MudBlazor
    Database: SQLite con Entity Framework Core
    Autenticazione: ASP.NET Core Identity
    Testing: xUnit, Moq, bUnit, test di integrazione
    Architettura: Clean Architecture con pattern Command/Query
    Real-time: Server-Sent Events (SSE)
    Mapping: Mapperly per il mapping degli oggetti

Significato del Progetto

Questo progetto dimostra le pratiche di sviluppo .NET moderne fornendo al contempo una soluzione pratica e deployabile di project management. Serve sia come applicazione funzionale che come implementazione di riferimento per Clean Architecture, strategie di testing complete e applicazioni web real-time nell'ecosistema .NET.
Approccio Implementativo

Il progetto segue una strategia di implementazione in 10 fasi, progredendo dal setup fondamentale attraverso funzionalità avanzate e documentazione completa. Ogni fase include deliverable specifici, requisiti di testing e aggiornamenti della documentazione per garantire un risultato production-ready.
Deliverable Attesi

    Codice sorgente completo con implementazione Clean Architecture
    Suite di test completa con copertura >90%
    API RESTful con documentazione OpenAPI
    UI Blazor moderna con design responsivo
    Containerizzazione Docker e pipeline CI/CD
    Documentazione tecnica e utente completa
    Guida al deployment in produzione
