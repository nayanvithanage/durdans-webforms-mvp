# Technical Implementation - Durdans Hospital Clinic Management System (Web Forms)

## 1. Architecture Overview
For an enterprise-grade Web Forms application in a hospital setting, the standard **N-Tier Architecture** (3-Layer) is recommended. This ensures separation of concerns, maintainability, and scalability.

### Layers
1.  **Presentation Layer (UI)**: ASP.NET Web Forms (`.aspx` pages + Code-Behind).
2.  **Business Logic Layer (BLL)**: C# Classes handling validation, calculations, and workflow rules.
3.  **Data Access Layer (DAL)**: Handles communication with the SQL Server database.

---

## 2. Technology Stack

| Component | Technology | Reason |
| :--- | :--- | :--- |
| **Framework** | .NET Framework 4.8 | The latest/stable version for legacy Web Forms apps. |
| **Frontend** | ASP.NET Server Controls + Bootstrap 5 | Server controls for logic, Bootstrap for responsive layout. |
| **Language** | C# | Standard backend language. |
| **Database** | Microsoft SQL Server | LocalDB/Express for development, full SQL Server for production. |
| **Data Access** | Entity Framework 6.x (Code First) | Modern ORM for rapid development, automatic migrations, and LINQ support. |

---

## 3. Detailed Implementation Strategy

### 3.1 Database Design (Entity Framework Code First)
*   **Domain Models**: `Patient`, `Doctor`, `Appointment`, `Hospital`, `User`.
*   **DbContext**: `ClinicDbContext` manages database connection and entity configurations.
*   **Relationships**:
    *   `Doctor` → Many-to-Many → `Hospital` (via junction table).
    *   `Appointment` → Many-to-One → `Patient`.
    *   `Appointment` → Many-to-One → `Doctor`.
    *   `Appointment` → Many-to-One → `Hospital`.
*   **Migrations**: Automatic schema generation and updates via EF Migrations.

### 3.2 Data Access Layer (DAL)
*   Use the **Repository Pattern** with Entity Framework.
*   **`ClinicDbContext`**: Inherits from `DbContext`, manages `DbSet<T>` for each entity.
*   **Repository Classes**: `PatientRepository`, `DoctorRepository`, `AppointmentRepository`, `HospitalRepository`.
*   Example Method:
    ```csharp
    public class DoctorRepository {
        private ClinicDbContext _context = new ClinicDbContext();
        
        public List<Doctor> GetDoctorsBySpecialization(string specialization) {
            return _context.Doctors
                .Where(d => d.Specialization == specialization)
                .ToList();
        }
    }

### 3.3 Business Logic Layer (BLL)
*   Acts as a bridge between UI and DAL.
*   **Validation**: Ensures appointment dates are not in the past *before* calling the DAL.
*   **Calculations**: Computes total bill (Doctor Fee + Hospital Charge).

### 3.4 Presentation Layer (Web Forms)
*   **Master Pages**: Use `Site.Master` for consistent header/footer/navigation.
*   **User Controls (`.ascx`)**: Reusable components (e.g., `PatientDetails.ascx` used in both Registration and Booking pages).
*   **State Management**:
    *   **Session**: Store logged-in user details.
    *   **ViewState**: Minimal usage (disable on grids) to improve performance.

---

## 4. Security Implementation
*   **Authentication**: ASP.NET Forms Authentication.
*   **Authorization**: Role-based access (e.g., `<location path="Admin"> <allow roles="Admin"/> </location>` in `web.config`).
*   **Input Sanitization**: 
    *   **SQL Injection Prevention**: Entity Framework automatically uses parameterized queries.
    *   **XSS Prevention**: Use `Server.HtmlEncode()` for user input display and validation controls.

## 5. Deployment
*   **Server**: IIS (Internet Information Services).
*   **Configuration**: 
    *   EF connection strings stored in `web.config` (`<connectionStrings>` section).
    *   Use SQL Server LocalDB for development, SQL Server Express/Full for production.
    *   Enable automatic migrations or use manual migration scripts for production deployments.

---

## 6. Feature Implementation Details (Web Forms + Entity Framework)

### 6.1 Patient Management (One page?)
*   **Registration Form (`RegisterPatient.aspx`)**
    *   **Controls**: `asp:TextBox` (Name, Contact), `asp:Calendar` (DOB), `asp:Button` (Submit).
    *   **Validation**: `asp:RequiredFieldValidator` (Name), `asp:RegularExpressionValidator` (Phone Number).
    *   **Database**: `PatientRepository.InsertPatient()` → `context.SaveChanges()`.
*   **Patient Search (`PatientList.aspx`)**
    *   **Controls**: `asp:GridView` (Display list), `asp:TextBox` (Search filter).
    *   **Logic**: Filter using LINQ queries or GridView filtering.

### 6.2 Hospital Management (One page?)
*   **Hospital Registry (`AddHospital.aspx`)**
    *   **Controls**: `asp:TextBox` (Name, Address), `asp:Button` (Submit).
    *   **Database**: `HospitalRepository.InsertHospital()` → `context.SaveChanges()`.
*   **Hospital Search (`HospitalList.aspx`)**
    *   **Controls**: `asp:GridView` (Display list), `asp:TextBox` (Search filter).
    *   **Logic**: Filter using LINQ: `context.Hospitals.Where(h => h.Name.Contains(searchTerm))`.

### 6.3 Doctor Management (One page?)
*   **Doctor Registry (`AddDoctor.aspx`)**
    *   **Controls**: `asp:DropDownList` (Specialization), `asp:TextBox` (Fee), `asp:CheckBoxList` (Hospital)
    *   **Database**: `DoctorRepository.InsertDoctor()` → `context.SaveChanges()`.
    *   **Many-to-Many**: Doctor-Hospital relationship managed via navigation properties.
*   **Doctor Search (`DoctorList.aspx`)**
    *   **Controls**: `asp:GridView` (Display list), `asp:TextBox` (Search filter).
    *   **Logic**: Filter using LINQ: `context.Doctors.Where(d => d.Name.Contains(searchTerm))`.
    **Availability Settings**
    *   **Controls**: `asp:CheckBoxList` (Days of Week), `asp:DropDownList` (Time Slots), `asp:DropDownList` (Hospital), `asp:TextBox` (Bookings per Time Slot limit).
    *   **Database**: `AvailabilityRepository.InsertAvailability()` → `context.SaveChanges()`.
    *   **Logic**: `AvailabilityRepository.InsertAvailability()` is called on `btnSubmitAvailability_Click` event. 
    *   **Workflow**: 
        1. this will add the availability record to the availablity list below the form. 
        2. availability list will have the following columns: 
            1. Doctor Name 
            2. Hospital Name
            3. Day of Week
            4. Time Slot
            5. Actions (Edit, Delete, Save)
        3. we can remove the record from the list using `btnRemoveAvailability_Click` event in the availability list.
        4. we can change the record from the list using `btnSaveAvailability_Click` event in the availability list.
        5. Timeslots dropdown will be shown 12AM to 12PM to select the time slot when setting the availability. (every 15 minutes, one hour slots. ex:6:15PM-7:15PM)

### 6.4 Appointment Scheduling 
*   **Booking Interface (`BookAppointment.aspx`)**
    *   **Workflow**:
        1.  Select Specialization (`AutoPostBack="true"` triggers reload).
        2.  Select Doctor (Populated via LINQ: `context.Doctors.Where(d => d.Specialization == selected)`).
        3.  Select Date (`asp:Calendar` with `OnDayRender` to disable past dates).
        4.  Select Hospital (Populated from Doctor's navigation property: `doctor.Hospitals`).
        5.  Select Time Slot (Check existing appointments via LINQ to show available slots). 
            *   **Implementation**: Use `asp:Repeater` to display time slots as color-coded boxes (every 15 minutes, one hour slots. ex:6:15PM-7:15PM)
            *   **Color Coding**:
                *   **Green**: Available (no bookings)
                *   **Amber**: Almost Full (bookings < max limit)
                *   **Red**: Unavailable (bookings >= max limit or doctor not available)
            *   **LINQ Logic**:
                ```csharp
                // Get doctor's availability for selected day
                var availability = context.DoctorAvailabilities
                    .FirstOrDefault(a => a.DoctorId == doctorId 
                        && a.HospitalId == hospitalId 
                        && a.DayOfWeek == selectedDate.DayOfWeek.ToString());
                
                // Get existing appointments
                var existingAppointments = context.Appointments
                    .Where(a => a.DoctorId == doctorId 
                        && a.HospitalId == hospitalId
                        && DbFunctions.TruncateTime(a.AppointmentDate) == selectedDate.Date)
                    .ToList();
                
                // Generate time slots and calculate status
                var slots = GenerateTimeSlots(availability.StartTime, availability.EndTime);
                foreach (var slot in slots) {
                    var bookingsInSlot = existingAppointments.Count(a => 
                        a.AppointmentTime >= slot.StartTime && 
                        a.AppointmentTime < slot.EndTime);
                    
                    slot.Status = bookingsInSlot == 0 ? "Available" : 
                                  bookingsInSlot < availability.MaxBookingsPerSlot ? "AlmostFull" : 
                                  "Unavailable";
                }
                ```
            *   **Required Entity**: `DoctorAvailability` with properties: `DoctorId`, `HospitalId`, `DayOfWeek`, `StartTime`, `EndTime`, `MaxBookingsPerSlot`.
        6.  All the available timeslots will be shown based on the doctor's availability (Date, Hospital, Doctor, Booked Appointments).
        7.  If booked appointments are exceeded for the time slot, the time slot will be shown in Amber.
        **Logic:**: 
        1. An Admin can Book an appointment for a patient using the `BookAppointment.aspx` page. BookingType will be admin, and the booked by will be the admin's login name.
        2. A Patient can Book an appointment for themselves using the `BookAppointment.aspx` page. BookingType will be patient, and the booked by will be the patient's login name.
    *   **Concurrency Control**: Use EF transactions (`context.Database.BeginTransaction()`) to prevent double-booking.
    *   **Validation**: Check for conflicts before `SaveChanges()`.
*   **Appointment Dashboard (`Dashboard.aspx`)**
    *   **Controls**: `asp:Repeater` or `asp:ListView` for custom card layout of upcoming visits.
    *   **Data**: LINQ query with `.Include()` for eager loading: `context.Appointments.Include(a => a.Doctor).Include(a => a.Patient)`.
        **Logic:**: 
        1. The Dashboard will show the upcoming visits of the logged-in user.
        2. An admin can see all the upcoming visits of all users. 
        3. An admin can filter the visits based on the patient name, doctor name, hospital name, and date.

### 6.5 System Features
*   **Authentication**: Login page using `asp:Login` control backed by EF `User` entity (`context.Users`).
*   **Master Page (`Site.Master`)**: Contains the Navigation Menu (`asp:Menu`) and Footer.
*   **Error Handling**: Global `Application_Error` in `Global.asax` to log errors to database via EF or text file.
