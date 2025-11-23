# Low Level Design Diagrams

## 1. Appointment Booking Module

### 1.1 Class Diagram
Details the classes involved in scheduling an appointment.

```mermaid
classDiagram
    %% Presentation Layer
    class BookAppointment {
        +Page_Load()
        +ddlSpecialization_SelectedIndexChanged()
        +Book_Click()
        -LoadPatients()
    }

    %% Business Logic Layer
    class ClinicService {
        +GetAllDoctors()
        +GetDoctorsBySpecialization(specialization)
        +RegisterPatient(patient)
        +GetAllPatients()
        +BookAppointment(appointment)
        +GetAllAppointments()
    }

    %% Data Access Layer
    class AppointmentRepository {
        +InsertAppointment(appointment)
        +GetAllAppointments()
    }

    class DoctorRepository {
        +GetAllDoctors()
        +GetDoctorsBySpecialization(specialization)
    }

    class SqlHelper {
        +GetNextId(list)
    }

    %% Domain Models
    class Appointment {
        +int Id
        +int DoctorId
        +int PatientId
        +DateTime AppointmentDate
        +Doctor Doctor
        +Patient Patient
    }

    %% Relationships
    BookAppointment ..> ClinicService : Uses
    ClinicService ..> AppointmentRepository : Uses
    ClinicService ..> DoctorRepository : Uses
    AppointmentRepository ..> SqlHelper : Uses
    ClinicService ..> Appointment : Manipulates
    AppointmentRepository ..> Appointment : Persists
```

### 1.2 Sequence Diagram
The flow when a patient clicks "Book".

```mermaid
sequenceDiagram
    participant User as Patient
    participant UI as BookAppointment.aspx.cs
    participant BLL as ClinicService
    participant DAL as AppointmentRepository
    participant DB as In-Memory List

    User->>UI: Click "Book Appointment"
    UI->>UI: Create Appointment Object
    UI->>BLL: BookAppointment(appt)
    
    activate BLL
    BLL->>BLL: Validate Date (appt.AppointmentDate)
    alt Date is Invalid
        BLL-->>UI: Throw Exception
        UI-->>User: Show Error Message
    else Date is Valid
        BLL->>DAL: InsertAppointment(appt)
        activate DAL
        DAL->>DB: Add to List
        activate DB
        DB-->>DAL: Return Void
        deactivate DB
        DAL-->>BLL: Return NewId
        deactivate DAL
        BLL-->>UI: Return Void
    end
    deactivate BLL

    UI-->>User: Show "Appointment Booked Successfully"
```

---

## 2. Patient Registration Module

### 2.1 Class Diagram
Details the classes involved in registering a new patient.

```mermaid
classDiagram
    %% Presentation Layer
    class RegisterPatient {
        +Page_Load()
        +Register_Click()
    }

    %% Business Logic Layer
    class ClinicService {
        +RegisterPatient(patient)
        +GetAllPatients()
    }

    %% Data Access Layer
    class PatientRepository {
        +InsertPatient(patient)
        +GetAllPatients()
        +GetPatientById(id)
    }

    %% Domain Models
    class Patient {
        +int Id
        +string Name
        +DateTime DateOfBirth
        +string ContactNumber
    }

    %% Relationships
    RegisterPatient ..> ClinicService : Uses
    ClinicService ..> PatientRepository : Uses
    PatientRepository ..> SqlHelper : Uses
    ClinicService ..> Patient : Manipulates
```

### 2.2 Sequence Diagram
The flow when a receptionist registers a new patient.

```mermaid
sequenceDiagram
    participant User as Receptionist
    participant UI as RegisterPatient.aspx.cs
    participant BLL as ClinicService
    participant DAL as PatientRepository
    participant DB as In-Memory List

    User->>UI: Click "Register"
    UI->>BLL: RegisterPatient(patient)
    
    activate BLL
    BLL->>DAL: InsertPatient(patient)
    activate DAL
    DAL->>DB: Add to List
    activate DB
    DB-->>DAL: Return Void
    deactivate DB
    DAL-->>BLL: Return NewId
    deactivate DAL
    BLL-->>UI: Return NewId
    deactivate BLL

    UI-->>User: Show "Patient registered successfully"
```
