# Piano di Test Unitari per Entità Domain

## Panoramica

Questo documento descrive il piano di test unitari per tutte le entità del dominio e i Value Objects che saranno implementati nel Task 2.1.

## Struttura dei Test

```
TaskManager.UnitTests/
└── Domain/
    ├── ValueObjects/
    │   ├── EmailTests.cs
    │   └── PriorityTests.cs
    ├── Entities/
    │   ├── UserTests.cs
    │   ├── ProjectTests.cs
    │   ├── BoardTests.cs
    │   ├── ColumnTests.cs
    │   ├── TaskTests.cs
    │   ├── CommentTests.cs
    │   ├── CategoryTests.cs
    │   ├── TagTests.cs
    │   ├── TaskTagTests.cs
    │   ├── SwimlaneTests.cs
    │   └── AttachmentTests.cs
```

## Test per Value Objects

### EmailTests.cs

1. **Creazione con valore valido**

   - Dato: Un indirizzo email valido
   - Quando: Viene creato un oggetto Email
   - Allora: L'oggetto viene creato correttamente con il valore specificato

2. **Creazione con valore null**

   - Dato: Un valore null
   - Quando: Viene creato un oggetto Email
   - Allora: Viene lanciata un'eccezione ArgumentException

3. **Creazione con valore vuoto**

   - Dato: Una stringa vuota
   - Quando: Viene creato un oggetto Email
   - Allora: Viene lanciata un'eccezione ArgumentException

4. **Creazione con formato non valido**

   - Dato: Una stringa con formato email non valido
   - Quando: Viene creato un oggetto Email
   - Allora: Viene lanciata un'eccezione ArgumentException

5. **Uguaglianza**

   - Dato: Due oggetti Email con lo stesso valore
   - Quando: Si confrontano i due oggetti
   - Allora: I due oggetti sono considerati uguali

6. **ToString**
   - Dato: Un oggetto Email
   - Quando: Si chiama il metodo ToString()
   - Allora: Viene restituito il valore dell'email

### PriorityTests.cs

1. **Creazione con valore valido**

   - Dato: Un valore di PriorityLevel
   - Quando: Viene creato un oggetto Priority
   - Allora: L'oggetto viene creato correttamente con il livello specificato

2. **Creazione con metodi statici**

   - Dato: Utilizzo dei metodi statici (Low, Normal, High, Urgent)
   - Quando: Si creano oggetti Priority con i metodi statici
   - Allora: Gli oggetti vengono creati con i livelli corretti

3. **Uguaglianza**

   - Dato: Due oggetti Priority con lo stesso livello
   - Quando: Si confrontano i due oggetti
   - Allora: I due oggetti sono considerati uguali

4. **ToString**
   - Dato: Un oggetto Priority
   - Quando: Si chiama il metodo ToString()
   - Allora: Viene restituito il nome del livello di priorità

## Test per Entità

### UserTests.cs

1. **Creazione con proprietà valide**

   - Dato: Valori validi per tutte le proprietà obbligatorie
   - Quando: Viene creato un oggetto User
   - Allora: L'oggetto viene creato correttamente

2. **Proprietà collezioni inizializzate**

   - Dato: Un nuovo oggetto User
   - Quando: Si accede alle collezioni di navigazione
   - Allora: Le collezioni sono inizializzate come liste vuote

3. **Proprietà obbligatorie**
   - Dato: Valori mancanti per proprietà obbligatorie
   - Quando: Si tenta di creare un oggetto User
   - Allora: L'oggetto viene comunque creato (la validazione avviene a livello di applicazione/database)

### ProjectTests.cs

1. **Creazione con proprietà valide**

   - Dato: Valori validi per tutte le proprietà obbligatorie
   - Quando: Viene creato un oggetto Project
   - Allora: L'oggetto viene creato correttamente

2. **Proprietà collezioni inizializzate**

   - Dato: Un nuovo oggetto Project
   - Quando: Si accede alle collezioni di navigazione
   - Allora: Le collezioni sono inizializzate come liste vuote

3. **Relazioni con entità correlate**
   - Dato: Un oggetto Project con entità correlate
   - Quando: Si accede alle proprietà di navigazione
   - Allora: Le relazioni sono correttamente stabilite

### BoardTests.cs

1. **Creazione con proprietà valide**

   - Dato: Valori validi per tutte le proprietà obbligatorie
   - Quando: Viene creato un oggetto Board
   - Allora: L'oggetto viene creato correttamente

2. **Relazione con Project**
   - Dato: Un oggetto Board associato a un Project
   - Quando: Si accede alla proprietà di navigazione Project
   - Allora: La relazione è correttamente stabilita

### ColumnTests.cs

1. **Creazione con proprietà valide**

   - Dato: Valori validi per tutte le proprietà obbligatorie
   - Quando: Viene creato un oggetto Column
   - Allora: L'oggetto viene creato correttamente

2. **Relazione con Project**
   - Dato: Un oggetto Column associato a un Project
   - Quando: Si accede alla proprietà di navigazione Project
   - Allora: La relazione è correttamente stabilita

### TaskTests.cs

1. **Creazione con proprietà valide**

   - Dato: Valori validi per tutte le proprietà obbligatorie
   - Quando: Viene creato un oggetto Task
   - Allora: L'oggetto viene creato correttamente

2. **Relazioni con entità correlate**

   - Dato: Un oggetto Task con entità correlate
   - Quando: Si accede alle proprietà di navigazione
   - Allora: Le relazioni sono correttamente stabilite

3. **Utilizzo di Value Objects**
   - Dato: Un oggetto Task con Priority
   - Quando: Si accede alla proprietà Priority
   - Allora: Il Value Object è correttamente assegnato

### CommentTests.cs

1. **Creazione con proprietà valide**

   - Dato: Valori validi per tutte le proprietà obbligatorie
   - Quando: Viene creato un oggetto Comment
   - Allora: L'oggetto viene creato correttamente

2. **Relazioni con Task e User**
   - Dato: Un oggetto Comment associato a un Task e un User
   - Quando: Si accede alle proprietà di navigazione
   - Allora: Le relazioni sono correttamente stabilite

### CategoryTests.cs

1. **Creazione con proprietà valide**

   - Dato: Valori validi per tutte le proprietà obbligatorie
   - Quando: Viene creato un oggetto Category
   - Allora: L'oggetto viene creato correttamente

2. **Relazione con Project**
   - Dato: Un oggetto Category associato a un Project
   - Quando: Si accede alla proprietà di navigazione Project
   - Allora: La relazione è correttamente stabilita

### TagTests.cs

1. **Creazione con proprietà valide**

   - Dato: Valori validi per tutte le proprietà obbligatorie
   - Quando: Viene creato un oggetto Tag
   - Allora: L'oggetto viene creato correttamente

2. **Relazione con Project**

   - Dato: Un oggetto Tag associato a un Project
   - Quando: Si accede alla proprietà di navigazione Project
   - Allora: La relazione è correttamente stabilita

3. **Relazione many-to-many con Task**
   - Dato: Un oggetto Tag associato a più Task
   - Quando: Si accede alla collezione Tasks
   - Allora: La collezione contiene i Task corretti

### TaskTagTests.cs

1. **Creazione con proprietà valide**

   - Dato: Valori validi per TaskId e TagId
   - Quando: Viene creato un oggetto TaskTag
   - Allora: L'oggetto viene creato correttamente

2. **Relazioni con Task e Tag**
   - Dato: Un oggetto TaskTag associato a un Task e un Tag
   - Quando: Si accede alle proprietà di navigazione
   - Allora: Le relazioni sono correttamente stabilite

### SwimlaneTests.cs

1. **Creazione con proprietà valide**

   - Dato: Valori validi per tutte le proprietà obbligatorie
   - Quando: Viene creato un oggetto Swimlane
   - Allora: L'oggetto viene creato correttamente

2. **Relazione con Project**
   - Dato: Un oggetto Swimlane associato a un Project
   - Quando: Si accede alla proprietà di navigazione Project
   - Allora: La relazione è correttamente stabilita

### AttachmentTests.cs

1. **Creazione con proprietà valide**

   - Dato: Valori validi per tutte le proprietà obbligatorie
   - Quando: Viene creato un oggetto Attachment
   - Allora: L'oggetto viene creato correttamente

2. **Relazioni con Task e User**
   - Dato: Un oggetto Attachment associato a un Task e un User
   - Quando: Si accede alle proprietà di navigazione
   - Allora: Le relazioni sono correttamente stabilite

## Considerazioni sui Test

1. **Isolamento**: Ogni test deve essere isolato e non dipendere da altri test.

2. **Copertura**: I test devono coprire almeno il 90% del codice delle entità e Value Objects.

3. **Mocking**: Dove necessario, utilizzare mocking per isolare le dipendenze.

4. **Performance**: I test unitari devono essere veloci da eseguire.

5. **Manutenibilità**: I test devono essere facili da mantenere e aggiornare.

## Strumenti di Testing

- **Framework**: xUnit
- **Mocking**: Moq
- **Data Generation**: AutoFixture
- **Assertions**: FluentAssertions

## Esecuzione dei Test

I test verranno eseguiti come parte della pipeline CI/CD e dovranno passare tutti prima di ogni merge nella branch principale.
