# Strategia di Testing

## Panoramica

Questo documento descrive la strategia di testing adottata per il progetto TaskManager, un clone di Kanboard sviluppato con tecnologie .NET. La strategia copre tutti gli aspetti del testing, dall'unit testing all'integrazione e al testing dell'interfaccia utente.

## Obiettivi del Testing

- Garantire la qualità del software attraverso test automatizzati
- Raggiungere una copertura del codice superiore al 90%
- Identificare e correggere i bug prima del rilascio
- Verificare che tutte le funzionalità soddisfino i requisiti specificati
- Assicurare che l'applicazione sia performante e sicura

## Tipi di Testing

### 1. Unit Testing

I test unitari verificano il comportamento di singole unità di codice (classi, metodi) in isolamento. Utilizziamo:

- **Framework**: xUnit
- **Mocking**: Moq
- **Generazione dati**: AutoFixture

#### Obiettivi:

- Testare la logica di business nel Domain Layer
- Verificare i Command e Query Handlers
- Validare i Repository e i Services
- Coprire edge cases e scenari di errore

### 2. Integration Testing

I test di integrazione verificano l'interazione tra diversi componenti del sistema e con il database. Utilizziamo:

- **Framework**: xUnit
- **Factory applicazione**: WebApplicationFactory
- **Database**: In-memory database per test isolati

#### Obiettivi:

- Testare gli endpoint API
- Verificare l'integrazione con il database
- Validare l'autenticazione e l'autorizzazione
- Testare scenari end-to-end

### 3. UI Testing

I test dell'interfaccia utente verificano il comportamento dei componenti Blazor. Utilizziamo:

- **Framework**: bUnit
- **Mocking**: Servizi mock per isolare i componenti UI

#### Obiettivi:

- Testare il rendering dei componenti
- Verificare le interazioni utente
- Validare il data binding
- Testare componenti complessi come la board Kanban

## Struttura dei Progetti di Test

```
tests/
├── TaskManager.UnitTests/       # Test unitari
├── TaskManager.IntegrationTests/ # Test di integrazione
└── TaskManager.BlazorTests/     # Test UI Blazor
```

## Pratiche di Testing

### Convenzioni di Naming

- Nome test: `MethodName_StateUnderTest_ExpectedBehavior`
- Esempio: `CreateProject_WithValidData_ProjectCreated`

### Struttura dei Test

1. **Arrange**: Preparazione del contesto di test
2. **Act**: Esecuzione del metodo da testare
3. **Assert**: Verifica dei risultati

### Code Coverage

- Target minimo: 90%
- Esclusione: Modelli di binding e DTO semplici
- Focus: Business logic e regole di validazione

## Strumenti e Configurazioni

### Unit Testing

- xUnit per il framework di testing
- Moq per il mocking di dipendenze
- AutoFixture per la generazione automatica di dati di test

### Integration Testing

- WebApplicationFactory per creare istanze dell'applicazione in memoria
- Database in-memory per test isolati
- Helper per l'autenticazione nei test

### UI Testing

- bUnit per il testing dei componenti Blazor
- Mock dei servizi per isolare i componenti UI

## CI/CD e Testing

- Esecuzione automatica di tutti i test su ogni commit
- Blocco dei merge se i test falliscono
- Report di copertura del codice
- Analisi statica del codice

## Best Practices

1. **Mantenere i test indipendenti**: Ogni test deve poter essere eseguito in isolamento
2. **Usare dati di test specifici**: Evitare dipendenze da dati esterni
3. **Mantenere i test leggibili**: I test devono essere autoesplicativi
4. **Evitare logiche complesse nei test**: I test devono essere semplici e diretti
5. **Mockare le dipendenze esterne**: Database, servizi esterni, ecc.
6. **Testare un solo concetto per test**: Ogni test dovrebbe verificare un singolo comportamento

## Monitoraggio e Manutenzione

- Revisione periodica dei test per rimuovere quelli obsoleti
- Aggiornamento dei test quando cambiano i requisiti
- Monitoraggio della copertura del codice
- Refactoring dei test insieme al codice di produzione

## Conclusione

Questa strategia di testing garantisce che il progetto TaskManager sia robusto, affidabile e mantenibile. Attraverso un approccio stratificato al testing, possiamo catturare bug in diverse fasi dello sviluppo e assicurarci che l'applicazione soddisfi i requisiti funzionali e non funzionali.
