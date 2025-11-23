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
| **Database** | Microsoft SQL Server | Enterprise standard for relational data. |
| **Data Access** | ADO.NET + Stored Procedures | **Critical for Durdans**: High performance and security. |

---

## 3. Detailed Implementation Strategy

### 3.1 Database Design (SQL Server)
*   **Tables**: `Patients`, `Doctors`, `Appointments`, `Users`.
*   **Stored Procedures**:
    *   `sp_InsertPatient`: Handles patient registration.
    *   `sp_GetDoctorsBySpecialization`: Optimized search query.
    *   `sp_BookAppointment`: Transactional booking logic.

### 3.2 Data Access Layer (DAL)
*   Use the **Repository Pattern**.
*   **`SqlHelper` Class**: A utility class to manage `SqlConnection`, `SqlCommand`, and connection strings centrally.
*   Example Method:
    ```csharp
    public DataTable GetDoctors() {
        // Calls sp_GetAllDoctors using SqlHelper
    }
    ```

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
*   **Input Sanitization**: Prevent SQL Injection (via Stored Procedures) and XSS (via Anti-XSS library).

## 5. Deployment
*   **Server**: IIS (Internet Information Services).
*   **Configuration**: Connection strings stored encrypted in `web.config`.

---

## 6. Feature Implementation Details (Web Forms + MSSQL)

### 6.1 Patient Management
*   **Registration Form (`RegisterPatient.aspx`)**
    *   **Controls**: `asp:TextBox` (Name, Contact), `asp:Calendar` (DOB), `asp:Button` (Submit).
    *   **Validation**: `asp:RequiredFieldValidator` (Name), `asp:RegularExpressionValidator` (Phone Number).
    *   **Database**: `sp_InsertPatient` (Stored Procedure).
*   **Patient Search (`PatientList.aspx`)**
    *   **Controls**: `asp:GridView` (Display list), `asp:TextBox` (Search filter).
    *   **Logic**: Filter GridView using `DataView.RowFilter`.

### 6.2 Doctor Management
*   **Doctor Registry (`AddDoctor.aspx`)**
    *   **Controls**: `asp:DropDownList` (Specialization), `asp:TextBox` (Fee).
    *   **Database**: `sp_InsertDoctor`.
*   **Availability Settings**
    *   **Controls**: `asp:CheckBoxList` (Days of Week), `asp:TextBox` (Time Slots).

### 6.3 Appointment Scheduling
*   **Booking Interface (`BookAppointment.aspx`)**
    *   **Workflow**:
        1.  Select Specialization (`AutoPostBack="true"` triggers reload).
        2.  Select Doctor (Populated based on Specialization).
        3.  Select Date (`asp:Calendar` with `OnDayRender` to disable past dates).
    *   **Concurrency Control**: Database Transaction in `sp_BookAppointment` to prevent double-booking.
*   **Appointment Dashboard (`Dashboard.aspx`)**
    *   **Controls**: `asp:Repeater` or `asp:ListView` for custom card layout of upcoming visits.

### 6.4 System Features
*   **Authentication**: Login page using `asp:Login` control backed by SQL `Users` table.
*   **Master Page (`Site.Master`)**: Contains the Navigation Menu (`asp:Menu`) and Footer.
*   **Error Handling**: Global `Application_Error` in `Global.asax` to log errors to a text file or database.
