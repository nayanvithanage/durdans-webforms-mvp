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
| **Frontend** | ASP.NET Server Controls + AdminLTE 3 | Server controls for logic, AdminLTE 3 for professional admin dashboard theme. |
| **UI Theme** | AdminLTE 3 (Bootstrap 4 based) | Free, open-source admin dashboard theme with modern design and comprehensive components. |
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

### 3.5 AdminLTE 3 Theme Integration

**Theme Selection:**
*   **AdminLTE 3** - Free, open-source admin dashboard theme
*   **Source**: https://adminlte.io/
*   **Version**: 3.2.0 (or latest stable)
*   **License**: MIT License (free for commercial use)
*   **Base Framework**: Bootstrap 4.x

**Implementation Approach:**
1. **Download and Setup**:
    * Download AdminLTE 3 from GitHub: https://github.com/ColorlibHQ/AdminLTE/releases
    * Extract CSS, JS, and plugin files to project folders
    * Place CSS files in `Content/adminlte/css/`
    * Place JS files in `Scripts/adminlte/js/`
    * Place plugins in `Scripts/adminlte/plugins/`

2. **File Structure**:
    ```
    Content/
    ├── adminlte/
    │   ├── css/
    │   │   ├── adminlte.min.css
    │   │   └── adminlte.min.css.map
    │   └── dist/
    │       └── img/ (AdminLTE images)
    Scripts/
    ├── adminlte/
    │   ├── js/
    │   │   └── adminlte.min.js
    │   └── plugins/
    │       ├── bootstrap/
    │       ├── chart.js/
    │       ├── datatables/
    │       └── ... (other plugins as needed)
    ```

3. **Master Page Updates**:
    * Update `Site.Master` to include AdminLTE CSS and JS files
    * Replace Bootstrap navbar with AdminLTE sidebar navigation
    * Implement AdminLTE layout structure (wrapper, main-header, sidebar, content-wrapper, footer)
    * Add AdminLTE-specific classes and structure

4. **Layout Structure**:
    * **Main Wrapper**: `<body class="hold-transition sidebar-mini layout-fixed">`
    * **Sidebar**: Left navigation menu with collapsible sections
    * **Content Wrapper**: Main content area with breadcrumbs
    * **Header**: Top navigation bar with user menu and notifications
    * **Footer**: Footer section

5. **Component Integration**:
    * **Cards**: Use AdminLTE card classes (`card`, `card-primary`, `card-header`, `card-body`)
    * **Tables**: Use AdminLTE table classes with DataTables plugin support
    * **Forms**: Use AdminLTE form styling with validation states
    * **Buttons**: Use AdminLTE button classes and variants
    * **Modals**: Use AdminLTE modal components
    * **Alerts**: Use AdminLTE alert components

6. **Navigation Menu**:
    * Implement sidebar navigation with role-based menu items
    * Use AdminLTE menu structure with icons (Font Awesome or Bootstrap Icons)
    * Collapsible menu sections for better organization
    * Active menu item highlighting

7. **Color Scheme**:
    * Default AdminLTE color scheme (blue/primary)
    * Customizable via CSS variables or theme files
    * Support for light/dark mode (optional)

8. **Responsive Design**:
    * Mobile-friendly sidebar (collapsible on small screens)
    * Responsive tables and cards
    * Touch-friendly navigation

9. **JavaScript Plugins**:
    * **DataTables**: For enhanced table functionality (sorting, searching, pagination)
    * **Chart.js**: For dashboard charts and graphs (if needed)
    * **Select2**: For enhanced dropdowns
    * **DatePicker**: For date selection (if not using Calendar control)

10. **Customization**:
    * Override AdminLTE styles in custom CSS file
    * Maintain project-specific branding
    * Custom color scheme matching Durdans Clinic branding
    * Logo and favicon updates

**Benefits:**
* Professional, modern appearance
* Comprehensive component library
* Responsive design out-of-the-box
* Active community support
* Free and open-source
* Easy to customize
* Well-documented

**Migration Steps:**
1. Download AdminLTE 3 files
2. Add CSS/JS files to project
3. Update `Site.Master` with AdminLTE structure
4. Update existing pages to use AdminLTE components
5. Test responsive behavior
6. Customize colors and branding
7. Add icons (Font Awesome or Bootstrap Icons)

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

### 6.1 Patient Management (`ManagePatient.aspx` - Single Page with Two Sections)

**Section 1: Patient Registration Form**
*   **Controls**: 
    *   `asp:TextBox` (Name, Contact Number)
    *   `asp:TextBox` with `TextMode="Date"` (Date of Birth)
    *   `asp:Button` (Register Patient)
*   **Validation**: 
    *   `asp:RequiredFieldValidator` for Name, Date of Birth, Contact Number
    *   Business logic validation in code-behind (date must be in past, no duplicate contact numbers)
*   **Database**: `PatientService.RegisterPatient()` → `PatientRepository.InsertPatient()` → `context.SaveChanges()`.

**Section 2: Patient List with Pagination, Update, and Delete**
*   **Controls**:
    *   `asp:Repeater` or `asp:GridView` (Display patient list in table/card format)
    *   `asp:TextBox` (Search filter - by name or contact number)
    *   `asp:Button` (Search/Filter)
    *   Pagination controls (First, Previous, Next, Last, Page size selector)
    *   Edit button/link for each patient row
    *   Delete button with confirmation dialog for each patient row
*   **Display Fields**:
    *   Patient ID
    *   Patient Name
    *   Date of Birth (formatted as "dd MMM yyyy")
    *   Contact Number
    *   Linked User (if applicable - show username or "Not linked")
    *   Actions (Edit, Delete buttons)
*   **Pagination**:
    *   Server-side pagination using LINQ `.Skip()` and `.Take()`
    *   Default page size: 10 patients per page
    *   Page size options: 10, 25, 50, 100
    *   Display total count: "Showing 1-10 of 45 patients"
    *   Page navigation: First, Previous, Next, Last buttons
    *   Current page indicator: "Page 1 of 5"
*   **Search/Filter Functionality**:
    *   Search by patient name (partial match, case-insensitive)
    *   Search by contact number (partial match)
    *   Filters persist during pagination
    *   Clear filter button to reset search
*   **Update Functionality**:
    *   Edit button opens inline edit mode or modal dialog
    *   Can update: Name, Date of Birth, Contact Number
    *   Validation: Same as registration (required fields, date validation)
    *   Uses `PatientService.UpdatePatient()` → `PatientRepository.UpdatePatient()`
    *   Success/error message display after update
*   **Delete Functionality**:
    *   Delete button with confirmation dialog: "Are you sure you want to delete this patient?"
    *   **Important**: Check if patient has appointments before deletion
    *   If patient has appointments: Show warning message, prevent deletion
    *   If no appointments: Proceed with deletion
    *   Uses `PatientService` delete method (if exists) or `PatientRepository.DeletePatient()`
    *   Success/error message display after deletion
    *   Refresh list after successful deletion
*   **Authorization**:
    *   Admin-only access (inherits from `AdminBasePage`)
    *   All operations require admin role
*   **User Experience**:
    *   Responsive table/card layout
    *   Loading indicator during operations (optional)
    *   Success/error messages displayed prominently
    *   Form validation feedback
    *   Confirmation dialogs for destructive actions
*   **Implementation Details**:
    *   Use `PatientService.GetAllPatients()` to load all patients
    *   Apply search filter using LINQ `.Where()` before pagination
    *   Store current page and page size in ViewState
    *   Separate pagination logic from data loading
    *   Handle appointment check before deletion (query `AppointmentRepository` or use navigation property)
    *   Refresh patient list after successful update/delete operations

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

#### 6.5.1 User-Patient Linking

**Problem Statement:**
Users need to be linked to their patient records to enable self-booking functionality. Currently, there is no formal database relationship between the `User` and `Patient` entities.

**Solution: Add UserId Foreign Key to Patient Table**

**Database Schema Change:**
```sql
ALTER TABLE Patients
ADD UserId INT NULL,
CONSTRAINT FK_Patient_User FOREIGN KEY (UserId) REFERENCES Users(Id);

-- Create index for performance
CREATE INDEX IX_Patient_UserId ON Patients(UserId);
```

**Model Update (Patient.cs):**
```csharp
public class Patient
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    public string Name { get; set; }

    [Required]
    public DateTime DateOfBirth { get; set; }

    [Required]
    [StringLength(20)]
    public string ContactNumber { get; set; }

    // NEW: Link to User account
    public int? UserId { get; set; }
    
    // Navigation properties
    public virtual User User { get; set; }
    public virtual ICollection<Appointment> Appointments { get; set; }
}
```

**User Model Update (User.cs):**
```csharp
public class User
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Username { get; set; }

    [Required]
    [StringLength(500)]
    public string PasswordHash { get; set; }

    [Required]
    [StringLength(50)]
    public string Role { get; set; }

    [StringLength(200)]
    public string Email { get; set; }

    public DateTime CreatedDate { get; set; }

    // NEW: Navigation property to Patient
    public virtual Patient Patient { get; set; }
}
```

**DbContext Configuration:**
```csharp
protected override void OnModelCreating(DbModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    // Configure User-Patient relationship (one-to-one optional)
    modelBuilder.Entity<User>()
        .HasOptional(u => u.Patient)
        .WithOptional(p => p.User)
        .Map(m => m.MapKey("UserId"));
}
```

#### 6.5.2 Auto-Create Patient Record During Registration

**Registration Flow Enhancement:**

When a user registers with role "Patient", automatically create their patient record:

```csharp
// Register.aspx.cs - Updated Registration Logic
protected void Register_Click(object sender, EventArgs e)
{
    if (IsValid)
    {
        try
        {
            // Create User account
            var user = new User
            {
                Username = txtUsername.Text,
                Email = txtEmail.Text,
                Role = "Patient", // Default role
                CreatedDate = DateTime.Now
            };

            int userId = _userService.RegisterUser(user, txtPassword.Text);

            // Auto-create Patient record for Patient role
            if (user.Role == "Patient")
            {
                var patient = new Patient
                {
                    Name = txtUsername.Text,
                    DateOfBirth = DateTime.Parse(txtDateOfBirth.Text),
                    ContactNumber = txtContactNumber.Text,
                    UserId = userId
                };

                _patientService.RegisterPatient(patient);
            }

            // Auto-login after registration
            Session["UserId"] = userId;
            Session["Username"] = user.Username;
            Session["UserRole"] = user.Role;

            Response.Redirect("~/Default.aspx");
        }
        catch (Exception ex)
        {
            lblMessage.Text = "Registration failed: " + ex.Message;
        }
    }
}
```

**Enhanced Registration Form Fields:**

Add patient-specific fields to `Register.aspx`:
- **Date of Birth** (required for patients)
- **Contact Number** (required for patients)
- **Email** (optional, for notifications)

**Updated BookAppointment Logic:**

With UserId link established, booking logic becomes simpler:

```csharp
// BookAppointment.aspx.cs - Updated Book_Click
protected void Book_Click(object sender, EventArgs e)
{
    int patientId;
    
    if (IsAdmin)
    {
        // Admin selects patient from dropdown
        patientId = int.Parse(ddlPatient.SelectedValue);
    }
    else
    {
        // Get patient ID from logged-in user
        int userId = (int)Session["UserId"];
        var patient = _patientService.GetPatientByUserId(userId);
        
        if (patient == null)
        {
            lblMessage.Text = "Patient record not found.";
            return;
        }
        
        patientId = patient.Id;
    }
    
    // Create appointment with patientId...
}
```

**New PatientService Method:**

```csharp
public Patient GetPatientByUserId(int userId)
{
    return _patientRepo.GetPatientByUserId(userId);
}
```

**New PatientRepository Method:**

```csharp
public Patient GetPatientByUserId(int userId)
{
    return _context.Patients.FirstOrDefault(p => p.UserId == userId);
}
```

**Benefits of User-Patient Linking:**

1. **Data Integrity**: Foreign key ensures referential integrity
2. **Performance**: Indexed lookup by UserId is fast
3. **Reliability**: No name-matching ambiguity
4. **Maintainability**: Clear relationship in database schema
5. **Scalability**: Supports future features (patient portal, medical records, etc.)

**Migration Steps:**

1. Add `UserId` column to Patients table (nullable initially)
2. Update Patient and User models
3. Create EF migration: `Add-Migration AddUserIdToPatient`
4. Update database: `Update-Database`
5. Manually link existing patients to users (if any)
6. Update registration logic to auto-create patient records
7. Update booking logic to use UserId lookup
8. Remove temporary `GetPatientByName` method

### 6.6 Welcome Page & User Interface Enhancements

#### 6.6.1 Default.aspx - Welcome Dashboard
After successful login, users are redirected to `Default.aspx` which serves as a personalized welcome page.

**Requirements:**
*   **Welcome Message**: Display "Welcome, [Username]!" at the top of the page
*   **Role-Based Action Cards**: Show different action cards based on user role
*   **Responsive Layout**: Use Bootstrap cards in a grid layout

**Admin User Actions:**
1. **Manage Doctors** - Link to `ManageDoctors.aspx`
2. **Manage Hospitals** - Link to `ManageHospital.aspx`
3. **Manage Specializations** - Link to `ManageSpecializations.aspx`
4. **Register Patient** - Link to `RegisterPatient.aspx`
5. **Book Appointment** - Link to `BookAppointment.aspx` (for any patient)
6. **View All Appointments** - Link to `Dashboard.aspx` (view all appointments)

**Patient User Actions:**
1. **Book Appointment** - Link to `BookAppointment.aspx` (for themselves)
2. **My Appointments** - Link to `Dashboard.aspx` (view their own appointments)
3. **Update Profile** - Link to profile page (future implementation)

**Implementation Example:**
```csharp
// Default.aspx.cs - Page_Load
protected void Page_Load(object sender, EventArgs e)
{
    if (Session["UserId"] == null)
    {
        Response.Redirect("~/Pages/Login.aspx");
        return;
    }
    
    string username = Session["Username"]?.ToString();
    string userRole = Session["UserRole"]?.ToString();
    
    lblWelcome.Text = $"Welcome, {username}!";
    
    // Show/hide panels based on role
    if (userRole == "Admin")
    {
        pnlAdminActions.Visible = true;
        pnlPatientActions.Visible = false;
    }
    else
    {
        pnlAdminActions.Visible = false;
        pnlPatientActions.Visible = true;
    }
}
```

#### 6.6.2 Navigation Bar - User Display
The `Site.Master` navigation bar should display the logged-in user's information.

**Requirements:**
*   **Username Display**: Show the logged-in username in the navigation bar (right side)
*   **Logout Button**: Provide a logout link/button next to the username
*   **Conditional Rendering**: Show "Login/Register" links when not logged in, show "Username/Logout" when logged in
*   **Role Badge**: Optionally display a badge showing the user's role (Admin/Patient)

**Implementation Example:**
```csharp
// Site.Master.cs - Page_Load
protected void Page_Load(object sender, EventArgs e)
{
    if (Session["UserId"] != null)
    {
        // User is logged in
        string username = Session["Username"]?.ToString();
        string userRole = Session["UserRole"]?.ToString();
        
        lblUsername.Text = username;
        lblUserRole.Text = userRole;
        
        // Show logged-in user section
        pnlLoggedIn.Visible = true;
        pnlGuest.Visible = false;
    }
    else
    {
        // User is not logged in
        pnlLoggedIn.Visible = false;
        pnlGuest.Visible = true;
    }
}

protected void Logout_Click(object sender, EventArgs e)
{
    Session.Clear();
    Session.Abandon();
    Response.Redirect("~/Pages/Login.aspx");
}
```

**Site.Master HTML Structure:**
```html
<!-- Guest User (Not Logged In) -->
<asp:Panel ID="pnlGuest" runat="server">
    <ul class="navbar-nav">
        <li class="nav-item"><a class="nav-link" runat="server" href="~/Pages/Login.aspx">Login</a></li>
        <li class="nav-item"><a class="nav-link" runat="server" href="~/Pages/Register.aspx">Register</a></li>
    </ul>
</asp:Panel>

<!-- Logged In User -->
<asp:Panel ID="pnlLoggedIn" runat="server">
    <ul class="navbar-nav">
        <li class="nav-item">
            <span class="navbar-text text-white me-3">
                <i class="bi bi-person-circle"></i>
                <asp:Label ID="lblUsername" runat="server" />
                <span class="badge bg-secondary ms-2">
                    <asp:Label ID="lblUserRole" runat="server" />
                </span>
            </span>
        </li>
        <li class="nav-item">
            <asp:LinkButton ID="btnLogout" runat="server" CssClass="nav-link" OnClick="Logout_Click">
                <i class="bi bi-box-arrow-right"></i> Logout
            </asp:LinkButton>
        </li>
    </ul>
</asp:Panel>
```

**Styling Considerations:**
*   Use Bootstrap utility classes for spacing and alignment
*   Add icons from Bootstrap Icons for better visual appeal
*   Ensure the username display is responsive on mobile devices
*   Use appropriate contrast for text visibility on dark navbar

### 6.7 URL Authorization & Page-Level Security

#### 6.7.1 Security Problem
Currently, all pages are accessible via direct URL navigation even without authentication. Users can bypass the UI and access admin pages or booking pages by typing the URL directly in the browser.

**Example Security Issues:**
- Guest user can access `/Pages/Admin/ManageDoctors.aspx` directly
- Unauthenticated user can access `/Pages/BookAppointment.aspx`
- Patient user can access admin-only pages

#### 6.7.2 Authorization Requirements

**Page Access Matrix:**

| Page | Guest Access | Patient Access | Admin Access |
|------|-------------|----------------|--------------|
| `Default.aspx` | ✅ Yes | ✅ Yes | ✅ Yes |
| `Login.aspx` | ✅ Yes | ❌ No (redirect to Default) | ❌ No (redirect to Default) |
| `Register.aspx` | ✅ Yes | ❌ No (redirect to Default) | ❌ No (redirect to Default) |
| `BookAppointment.aspx` | ❌ No (redirect to Login) | ✅ Yes | ✅ Yes |
| `ManageDoctors.aspx` | ❌ No (redirect to Login) | ❌ No (redirect to Default) | ✅ Yes |
| `ManageHospital.aspx` | ❌ No (redirect to Login) | ❌ No (redirect to Default) | ✅ Yes |
| `ManageSpecializations.aspx` | ❌ No (redirect to Login) | ❌ No (redirect to Default) | ✅ Yes |
| `RegisterPatient.aspx` | ❌ No (redirect to Login) | ❌ No (redirect to Default) | ✅ Yes |
| `Dashboard.aspx` | ❌ No (redirect to Login) | ✅ Yes | ✅ Yes |

#### 6.7.3 Implementation Approach

**Option 1: Page-Level Authorization (Recommended for Web Forms)**

Add authorization checks in `Page_Load` of each protected page:

```csharp
// For pages requiring authentication (any logged-in user)
protected void Page_Load(object sender, EventArgs e)
{
    // Check if user is logged in
    if (Session["UserId"] == null)
    {
        Response.Redirect("~/Pages/Login.aspx?ReturnUrl=" + Server.UrlEncode(Request.Url.PathAndQuery));
        return;
    }
    
    // Rest of Page_Load logic...
}
```

```csharp
// For admin-only pages
protected void Page_Load(object sender, EventArgs e)
{
    // Check if user is logged in
    if (Session["UserId"] == null)
    {
        Response.Redirect("~/Pages/Login.aspx?ReturnUrl=" + Server.UrlEncode(Request.Url.PathAndQuery));
        return;
    }
    
    // Check if user is admin
    string userRole = Session["UserRole"]?.ToString();
    if (userRole != "Admin")
    {
        Response.Redirect("~/Default.aspx");
        return;
    }
    
    // Rest of Page_Load logic...
}
```

**Option 2: Base Page Class (More Maintainable)**

Create a base page class for reusable authorization:

```csharp
// BasePage.cs
public class BasePage : System.Web.UI.Page
{
    protected virtual bool RequireAuthentication => true;
    protected virtual string[] AllowedRoles => null; // null = any authenticated user
    
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        
        if (RequireAuthentication)
        {
            if (Session["UserId"] == null)
            {
                Response.Redirect("~/Pages/Login.aspx?ReturnUrl=" + Server.UrlEncode(Request.Url.PathAndQuery));
                return;
            }
            
            if (AllowedRoles != null && AllowedRoles.Length > 0)
            {
                string userRole = Session["UserRole"]?.ToString();
                if (!AllowedRoles.Contains(userRole))
                {
                    Response.Redirect("~/Default.aspx");
                    return;
                }
            }
        }
    }
}

// AdminBasePage.cs
public class AdminBasePage : BasePage
{
    protected override string[] AllowedRoles => new[] { "Admin" };
}

// Usage in pages:
// ManageDoctors.aspx.cs
public partial class ManageDoctors : AdminBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Authorization already handled by base class
        // Page logic here...
    }
}
```

**Option 3: Web.config Authorization (Limited in Web Forms)**

While ASP.NET Web Forms supports `<location>` tags in web.config, this approach works best with Forms Authentication and membership providers. Since we're using custom session-based authentication, page-level checks are more appropriate.

#### 6.7.4 Return URL Handling

When redirecting unauthenticated users to login, preserve the original URL:

```csharp
// In Login.aspx.cs after successful login
protected void Login_Click(object sender, EventArgs e)
{
    if (IsValid)
    {
        var user = _userService.ValidateUser(txtUsername.Text, txtPassword.Text);
        if (user != null)
        {
            Session["UserId"] = user.Id;
            Session["Username"] = user.Username;
            Session["UserRole"] = user.Role;
            
            // Check for return URL
            string returnUrl = Request.QueryString["ReturnUrl"];
            if (!string.IsNullOrEmpty(returnUrl))
            {
                Response.Redirect(returnUrl);
            }
            else
            {
                Response.Redirect("~/Default.aspx");
            }
        }
        else
        {
            lblMessage.Text = "Invalid username or password.";
        }
    }
}
```

#### 6.7.5 Navigation Menu Conditional Display

Update `Site.Master` to hide admin links from non-admin users:

```csharp
// Site.Master.cs
protected void Page_Load(object sender, EventArgs e)
{
    string userRole = Session["UserRole"]?.ToString();
    
    // Show/hide admin menu items
    if (userRole == "Admin")
    {
        // Show all menu items
        liManageDoctors.Visible = true;
        liManageHospital.Visible = true;
        liManageSpecializations.Visible = true;
        liRegisterPatient.Visible = true;
    }
    else
    {
        // Hide admin menu items
        liManageDoctors.Visible = false;
        liManageHospital.Visible = false;
        liManageSpecializations.Visible = false;
        liRegisterPatient.Visible = false;
    }
    
    // Rest of existing logic...
}
```

#### 6.7.6 Security Best Practices

1. **Always Check Server-Side**: Never rely on UI hiding alone
2. **Validate on Every Request**: Check authorization in `Page_Load` or `OnInit`
3. **Use HTTPS**: Ensure all authentication happens over HTTPS in production
4. **Session Timeout**: Configure appropriate session timeout in web.config
5. **Prevent Session Fixation**: Regenerate session ID after login
6. **Log Security Events**: Log unauthorized access attempts

**Web.config Session Configuration:**
```xml
<system.web>
    <sessionState 
        mode="InProc" 
        timeout="30" 
        cookieless="false" 
        cookieSameSite="Strict" />
</system.web>
```

#### 6.7.7 Implementation Priority

**Phase 1 (Critical - Immediate):**
1. Add authentication checks to all admin pages
2. Add authentication checks to `BookAppointment.aspx`
3. Implement return URL handling in login

**Phase 2 (Important - Short Term):**
1. Create `BasePage` and `AdminBasePage` classes
2. Refactor existing pages to use base classes
3. Hide admin navigation items from non-admin users

**Phase 3 (Enhancement - Medium Term):**
1. Add audit logging for security events
2. Implement session timeout warnings
3. Add CSRF protection for forms

**Admin Booking**: 
*   Admin can book appointments for patients
*   `BookingType` = "Admin"
*   `BookedBy` = Admin's login username (from session/authentication)
        2. A Patient can Book an appointment for themselves using the `BookAppointment.aspx` page. BookingType will be patient, and the booked by will be the patient's login name.
    *   **Concurrency Control**: Use EF transactions (`context.Database.BeginTransaction()`) to prevent double-booking.
    *   **Validation**: Check for conflicts before `SaveChanges()`.
*   **Appointment Dashboard (`Dashboard.aspx`)**
    *   **Controls**: `asp:Repeater` or `asp:ListView` for custom card layout of appointments.
    *   **Data**: LINQ query with `.Include()` for eager loading: `context.Appointments.Include(a => a.Doctor).Include(a => a.Patient).Include(a => a.Hospital)`.
    
    **Normal User (Patient) View:**
    1. Shows only the logged-in user's appointments (filtered by PatientId from UserId).
    2. Displays both upcoming and past appointments (separate sections or tabs).
    3. Upcoming appointments sorted by date/time (ascending).
    4. Past appointments sorted by date/time (descending).
    5. **Read-only view** - patients cannot edit or delete appointments.
    6. Display fields: Patient Name, Doctor Name, Hospital Name, Appointment Date, Appointment Time, Booking Type, Booked By.
    7. Color coding: Upcoming appointments in green/blue, Past appointments in gray.
    
    **Admin User View:**
    1. Shows all appointments from all patients.
    2. Displays both upcoming and past appointments (separate sections or tabs).
    3. **Filtering capabilities** (all filters optional):
        * Filter by Patient Name (dropdown or text search)
        * Filter by Doctor Name (dropdown)
        * Filter by Hospital Name (dropdown)
        * Filter by Date Range (from date and to date)
        * Filter by Appointment Status (Upcoming/Past/All)
    4. **Edit functionality** - Admin can edit appointments:
        * Edit button/link for each appointment card
        * Edit modal or separate page (`EditAppointment.aspx`)
        * Can modify: Doctor, Hospital, Date, Time
        * Validation: Same as booking (check availability, max bookings per slot)
        * Update using `AppointmentService.UpdateAppointment()`
    5. **Delete functionality** - Admin can cancel/delete appointments:
        * Delete button with confirmation dialog
        * Uses `AppointmentService.CancelAppointment()`
    6. Display fields: Patient Name, Doctor Name, Hospital Name, Appointment Date, Appointment Time, Booking Type, Booked By, Actions (Edit/Delete).
    
    **Common Display Format:**
    * Card-based layout using Bootstrap cards
    * Each appointment card shows:
        * Header: Appointment ID and Status badge (Upcoming/Past)
        * Body: Patient, Doctor, Hospital, Date, Time
        * Footer: Booking Type, Booked By, Action buttons (admin only)
    * Responsive grid: 1 column on mobile, 2-3 columns on desktop
    
    **Implementation Details:**
    * Use `AppointmentService.GetAppointmentsByPatient(patientId)` for normal users
    * Use `AppointmentService.GetAllAppointments()` for admin (with optional filtering)
    * Filter logic in code-behind using LINQ
    * Edit functionality redirects to `EditAppointment.aspx?Id={appointmentId}` or uses modal
    * Authorization check: Only admin can access edit/delete functionality
    
    **Pagination Requirements:**
    1. **Purpose**: Improve performance and user experience when displaying large numbers of appointments
    2. **Implementation**: Server-side pagination using LINQ `.Skip()` and `.Take()` methods
    3. **Page Size**: Configurable (default: 10 appointments per page)
    4. **Controls**: 
        * Page size selector (dropdown: 10, 25, 50, 100 items per page)
        * Previous/Next navigation buttons
        * Page number indicators (e.g., "Page 1 of 5")
        * First/Last page buttons for easy navigation
    5. **Behavior**:
        * Pagination applies separately to Upcoming and Past appointment tabs
        * Filters are preserved when navigating between pages
        * Page state maintained during postbacks (using ViewState or query parameters)
        * Total count displayed (e.g., "Showing 1-10 of 45 appointments")
    6. **Performance Considerations**:
        * Only load data for current page (not all appointments)
        * Use `.Skip((currentPage - 1) * pageSize).Take(pageSize)` for efficient data retrieval
        * Calculate total pages: `Math.Ceiling((double)totalCount / pageSize)`
    7. **User Experience**:
        * Disable Previous button on first page
        * Disable Next button on last page
        * Highlight current page number
        * Show loading indicator during data fetch (optional)
    8. **Code Structure**:
        * Create pagination helper class or methods in code-behind
        * Store current page index and page size in ViewState
        * Separate pagination logic for Upcoming and Past appointments

### 6.5 System Features
*   **Authentication**: Login page using `asp:Login` control backed by EF `User` entity (`context.Users`).
*   **Master Page (`Site.Master`)**: Contains the Navigation Menu (`asp:Menu`) and Footer.
*   **Error Handling**: Global `Application_Error` in `Global.asax` to log errors to database via EF or text file.
