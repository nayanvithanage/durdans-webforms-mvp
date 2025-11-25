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

### 4.1 Authentication
*   **Mechanism**: Custom Session-based Authentication.
*   **Session Management**:
    *   Upon successful login, `Session["UserId"]`, `Session["Username"]`, and `Session["UserRole"]` are populated.
    *   Session timeout is controlled via `web.config` (standard ASP.NET behavior).
*   **Password Security**:
    *   **Hashing Algorithm**: SHA-256 (implemented in `UserService.cs`).
    *   **Salt**: Currently using simple hashing; recommended to upgrade to BCrypt or PBKDF2 with per-user salt for production.
    *   Passwords are **never** stored in plain text.

### 4.2 Authorization (Role-Based Access Control - RBAC)
*   **Roles**: `Admin`, `Patient`.
*   **UI Level**:
    *   `Site.Master.cs` dynamically hides/shows navigation items based on `Session["UserRole"]`.
    *   Guest users see only Login/Register links.
*   **Page Level**:
    *   **BasePage Architecture**:
        *   `BasePage`: Abstract base class that checks for valid session (`Session["UserId"]`). Redirects to Login if session is null.
        *   `AdminBasePage`: Inherits from `BasePage`. Enforces `AllowedRoles = ["Admin"]`. Redirects to Default/Login if user is not an Admin.
    *   **Implementation**: All Admin pages (e.g., `ManageDoctors.aspx`) inherit from `AdminBasePage`, ensuring secure access by default.

### 4.3 Data Security
*   **SQL Injection Prevention**:
    *   All database interactions use **Entity Framework 6**.
    *   EF automatically parameterizes all queries, neutralizing SQL injection attacks.
*   **Sensitive Data**:
    *   Connection strings are stored in `web.config` (should be encrypted in production).

### 4.4 Input Validation & Sanitization
*   **Server-Side Validation**:
    *   ASP.NET Validation Controls (`RequiredFieldValidator`, `RegularExpressionValidator`, `CompareValidator`) used on all forms.
    *   Business Logic Layer (`PatientService`, `DoctorService`) performs additional data integrity checks (e.g., preventing past dates for DOB, duplicate records).
*   **XSS Prevention**:
    *   ASP.NET Web Forms automatically encodes output for most server controls (`<asp:Label>`, `<asp:TextBox>`).
    *   Explicit use of `Server.HtmlEncode()` recommended for any direct HTML output.

## 5. Deployment
*   **Server**: IIS (Internet Information Services).
*   **Configuration**: 
    *   EF connection strings stored in `web.config` (`<connectionStrings>` section).
    *   Use SQL Server LocalDB for development, SQL Server Express/Full for production.
    *   Enable automatic migrations or use manual migration scripts for production deployments.

---

## 7. CI/CD Pipeline Strategy

### 7.1 Overview
*   **Source Control**: GitHub (Code repository).
*   **CI/CD Orchestrator**: Azure DevOps (Pipelines).
*   **Hosting**: Azure App Service (Web Apps).
*   **Environments**: Development (Dev), Production (Prod).
*   **Database Strategy**: Entity Framework Code First Automatic Migrations.

### 7.2 Build Pipeline (CI)
*   **Trigger**: Commit to `main` branch.
*   **Agent Pool**: Azure Pipelines (Windows-latest).
*   **Steps**:
    1.  **Get Sources**: Checkout code from GitHub repository.
    2.  **NuGet Restore**: Restore packages for the solution.
    3.  **Build**: Build solution in `Release` configuration.
    4.  **Test**: Run Unit/Integration tests (if applicable).
    5.  **Publish**: Package the Web Forms application into a `.zip` artifact (Web Deploy package).
    6.  **Artifact Upload**: Publish the build artifact for the Release pipeline.

### 7.3 Release Pipeline (CD)
*   **Artifact Source**: Output from Build Pipeline.
*   **Stages**:

    #### Stage 1: Development (Dev)
    *   **Trigger**: Automatic (After successful Build).
    *   **Steps**:
        1.  **Deploy App**: Deploy web package to **Azure App Service (Dev Slot/Resource)**.
        2.  **Database Migration**:
            *   Use `Migrate.exe` (shipped with EF6) or a PowerShell script to run `Update-Database`.
            *   Target: Dev Database.

    #### Stage 2: Production (Prod)
    *   **Trigger**: Pre-deployment approval required (Manual Gate).
    *   **Steps**:
        1.  **Deploy App**: Deploy web package to **Azure App Service (Prod Resource)**.
        2.  **Database Migration**:
            *   Execute migrations against Production Database.
            *   *Safety Check*: Ensure backups are taken before migration.

### 7.4 Configuration Management
*   **Connection Strings**:
    *   Do **not** store secrets in `web.config` in the repository.
    *   Use **Azure App Service Configuration** settings to override `connectionStrings` at runtime.
    *   Dev connection string -> Dev App Service Configuration.
    *   Prod connection string -> Prod App Service Configuration.
*   **Variable Groups**: Store non-secret environment variables in Azure DevOps Library.

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

### 6.3 Doctor Management (`AddDoctor.aspx` - Single Page with Two Sections)

**Section 1: Doctor Registration**
*   **Controls**: 
    *   `asp:TextBox` (Doctor Name)
    *   `asp:DropDownList` (Specialization)
    *   `asp:TextBox` (Consultation Fee)
    *   `asp:CheckBoxList` (Assign Hospitals - for many-to-many relationship)
    *   `asp:Button` (Register Doctor)
*   **Database**: `DoctorRepository.InsertDoctor()` → `context.SaveChanges()`.
*   **Many-to-Many**: Doctor-Hospital relationship managed via navigation properties and CheckBoxList selection.
*   **Validation**: RequiredFieldValidator for Name, Specialization, Fee; RangeValidator for Fee (1-100,000).

**Section 2: Doctor Availability Settings** (Below Registration Section)
*   **Controls**: 
    *   `asp:DropDownList` (Select Doctor - populated from registered doctors)
    *   `asp:DropDownList` (Select Hospital - filtered by selected doctor's hospitals)
    *   `asp:CheckBoxList` (Days of Week - Monday to Sunday)
    *   `asp:DropDownList` (Start Time - 12:00 AM to 11:45 PM, 15-minute intervals)
    *   `asp:DropDownList` (End Time - 12:00 AM to 11:45 PM, 15-minute intervals)
    *   `asp:TextBox` (Max Bookings Per Slot - default: 3, range: 1-10)
    *   `asp:Button` (Add Availability)
    *   `asp:GridView` (Display and manage availability schedules)
*   **Database**: `AvailabilityRepository.InsertAvailability()` → `context.SaveChanges()`.
*   **GridView Columns**:
    1. Doctor Name (ReadOnly)
    2. Hospital Name (ReadOnly)
    3. Day of Week (ReadOnly)
    4. Time Slot (ReadOnly, format: "HH:MM AM/PM - HH:MM AM/PM")
    5. Max Bookings (Editable)
    6. Actions (Edit, Delete buttons)
*   **Workflow**: 
    1. Admin selects a doctor from dropdown (triggers AutoPostBack to load doctor's hospitals)
    2. Admin selects hospital, days of week, time range, and max bookings
    3. Click "Add Availability" creates availability records for each selected day
    4. GridView displays all availability schedules for selected doctor
    5. Admin can Edit max bookings or Delete availability records via GridView
    6. Time slots are generated in 15-minute intervals (e.g., 6:15 PM - 7:15 PM represents 1-hour slots)

### 6.4 Appointment Scheduling (`BookAppointment.aspx` - Two-Panel Layout)

**Left Panel: Appointment Details Selection**
*   **Controls**:
    *   `asp:DropDownList` (Select Patient)
    *   `asp:DropDownList` (Specialization - with `AutoPostBack="true"`)
    *   `asp:DropDownList` (Select Doctor - populated based on specialization)
    *   `asp:DropDownList` (Select Hospital - populated from selected doctor's hospitals)
    *   `asp:Calendar` (Select Date - with `OnDayRender` to disable past dates)
    *   `asp:Button` (Load Available Time Slots)
*   **Validation**: RequiredFieldValidator for Patient, Doctor, Hospital.

**Right Panel: Time Slot Selection**
*   **Controls**:
    *   `asp:Repeater` (Display color-coded time slots)
    *   `asp:HiddenField` (Store selected time slot value)
    *   `asp:Label` (Display selected slot confirmation)
    *   `asp:Button` (Confirm Booking)
*   **Time Slot Display**:
    *   **Format**: Grid layout with LinkButtons (e.g., "06:15 PM - 07:15 PM")
    *   **Color Coding** (CSS classes):
        *   **Green** (`time-slot-available`): No bookings, fully available
        *   **Amber** (`time-slot-almost-full`): Bookings < max limit, shows count (e.g., "Almost Full (2/3)")
        *   **Red** (`time-slot-unavailable`): Bookings >= max limit or doctor not available, disabled
    *   **Interaction**: Clickable slots trigger `ItemCommand` event, selected slot highlighted with blue border

**Workflow**:
1.  Select Patient from dropdown
2.  Select Specialization (AutoPostBack loads matching doctors)
3.  Select Doctor (AutoPostBack loads doctor's hospitals)
4.  Select Hospital
5.  Select Date from Calendar (past dates are grayed out and disabled)
6.  Click "Load Available Time Slots" button
7.  System queries `DoctorAvailability` for selected day of week
8.  System queries existing `Appointments` for selected date/doctor/hospital
9.  Time slots generated in 15-minute intervals (1-hour slots: 6:15PM-7:15PM)
10. Each slot color-coded based on booking count vs. max limit
11. User clicks desired time slot (stored in HiddenField)
12. Click "Confirm Booking" to create appointment

**LINQ Logic**:
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
    var bookingsInSlot = existingAppointments.Count(a => a.AppointmentTime == slot.StartTime);
    
    slot.Status = bookingsInSlot == 0 ? "Available" : 
                  bookingsInSlot < availability.MaxBookingsPerSlot ? $"Almost Full ({bookingsInSlot}/{availability.MaxBookingsPerSlot})" : 
                  "Unavailable";
    slot.CssClass = bookingsInSlot == 0 ? "time-slot-available" :
                    bookingsInSlot < availability.MaxBookingsPerSlot ? "time-slot-almost-full" :
                    "time-slot-unavailable";
}

// In one time slot, there can be multiple bookings for different patients but the total bookings should not exceed the max bookings per slot.
// If the total bookings exceed the max bookings per slot, the slot should be marked as unavailable.

### 6.5 User Authentication & Authorization
*   **Login Page (`Login.aspx`)**:
    *   **Controls**: `asp:TextBox` (Username, Password), `asp:Button` (Login).
    *   **Logic**: Validate credentials against `Users` table. Store `User` object or ID/Role in Session.
*   **Registration Page (`Register.aspx`)**:
    *   **Controls**: `asp:TextBox` (Username, Password, Email), `asp:DropDownList` (Role - hidden/default for public users).
    *   **Logic**: Create new `User` entity. Hash password before saving.
*   **Role-Based Booking Logic**:
    *   **Admin**: When booking, the `Patient` dropdown is visible and enabled. Admin selects the patient.
    *   **Patient (User)**: When booking, the `Patient` dropdown is hidden or disabled. The system automatically assigns the logged-in user's Patient ID.

```csharp
// Example Logic in BookAppointment.aspx.cs
if (Session["UserRole"] == "Admin") {
    ddlPatient.Visible = true;
} else {
    ddlPatient.Visible = false;
    int currentUserId = (int)Session["UserId"];
    // Logic to find PatientId from UserId
}
```
**Admin Booking**: 
*   Admin can book appointments for patients
*   `BookingType` = "Admin"
*   `BookedBy` = Admin's login username (from session/authentication)
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
