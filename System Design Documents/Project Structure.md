# Project Structure - Durdans Hospital Clinic Management System

This document outlines the physical file structure of the ASP.NET Web Forms application, organized to support the **N-Tier Architecture**.

## Directory Tree

```text
DurdansClinic/
│
├── 📁 App_Data/                 # Database files (.mdf) or XML data stores
│
├── 📁 App_Start/                # Startup configuration
│   ├── BundleConfig.cs          # CSS/JS bundling logic
│   └── RouteConfig.cs           # URL routing configuration
│
├── 📁 Content/                  # Static assets (CSS, Images)
│   ├── 📁 css/
│   │   ├── bootstrap.min.css    # Bootstrap framework
│   │   └── site.css             # Custom application styles
│   └── 📁 images/
│       └── logo.png
│
├── 📁 Scripts/                  # JavaScript files
│   ├── bootstrap.js
│   ├── jquery.js
│   └── app.js                   # Custom client-side logic
│
├── 📁 BLL/                      # Business Logic Layer (C# Classes)
│   ├── AppointmentService.cs    # Logic for booking/validation
│   ├── DoctorService.cs         # Logic for doctor management
│   └── PatientService.cs        # Logic for patient registration
│
├── 📁 DAL/                      # Data Access Layer (C# Classes)
│   ├── SqlHelper.cs             # Database connection utility
│   ├── AppointmentRepository.cs # Database operations for appointments
│   └── PatientRepository.cs     # Database operations for patients
│
├── 📁 Models/                   # Domain Objects / DTOs
│   ├── Appointment.cs
│   ├── Doctor.cs
│   └── Patient.cs
│
├── 📁 UserControls/             # Reusable UI Components (.ascx)
│   ├── Header.ascx
│   └── PatientDetails.ascx
│
├── 📁 Pages/                    # Web Forms (.aspx)
│   ├── 📁 Admin/                # Secured Admin Pages
│   │   ├── AddDoctor.aspx
│   │   └── ManageUsers.aspx
│   │
│   ├── 📁 Patient/              # Patient-facing Pages
│   │   ├── BookAppointment.aspx
│   │   └── MyHistory.aspx
│   │
│   ├── Login.aspx               # Authentication Page
│   └── Default.aspx             # Home Page
│
├── Global.asax                  # Application-level events (Start, Error)
├── Site.Master                  # Master Page (Layout template)
└── Web.config                   # Application configuration (DB connection, Security)
```

## Key Components Explained

### 1. Core Folders
*   **`App_Data`**: Secure folder for data files. IIS prevents direct web access to files here.
*   **`BLL` & `DAL`**: These folders physically separate the C# logic code from the UI code, enforcing the N-Tier architecture within a single project structure.
*   **`Content` & `Scripts`**: Standard locations for client-side libraries (Bootstrap, jQuery).

### 2. Configuration Files
*   **`Web.config`**: The brain of the application. It contains:
    *   **Connection Strings**: Credentials to connect to SQL Server.
    *   **Authentication**: Rules for who can access which folders (e.g., protecting `/Admin`).
    *   **Custom Errors**: Settings to show friendly error pages.
*   **`Global.asax`**: Handles global events like `Application_Start` (to configure routes) and `Application_Error` (global exception logging).

### 3. UI Components
*   **`Site.Master`**: Acts as the "template" for the site. It contains the `<html>`, `<head>`, and the main Menu. All other pages (`.aspx`) are injected into its `ContentPlaceHolder`.
*   **`UserControls`**: Partial pages (like React Components) that can be embedded in multiple pages to reuse UI logic.
