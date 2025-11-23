# End-to-End Project Flow: Technical Deep Dive

This document explains the lifecycle of a single user action (e.g., "Booking an Appointment") as it travels through the **N-Tier Architecture** we designed for Durdans Hospital.

## The Scenario
A patient clicks the **"Book Appointment"** button on the website.

---

## Step 1: The Request (Client-Side)
*   **Event**: The user clicks the button.
*   **Action**: The browser sends an **HTTP POST** request to the server.
*   **Payload**: The request contains the Form Data (DoctorID, PatientID, Date) and the **ViewState** (a hidden string containing the page's previous state).

## Step 2: The Entry Point (Presentation Layer)
*   **Technology**: ASP.NET Web Forms (`BookAppointment.aspx`)
*   **Lifecycle**: The server receives the request and starts the **Page Lifecycle**:
    1.  **Initialization**: The page object is created.
    2.  **Load**: `Page_Load` runs. We check `if (!IsPostBack)` to ensure we don't reload the dropdowns unnecessarily.
        *   *Note*: `PatientService` is used here to populate the Patient Dropdown.
    3.  **Event Handling**: The `Book_Click` event handler in the **Code-Behind** (`BookAppointment.aspx.cs`) is triggered.
*   **Role**: The UI collects the inputs from the DropDowns/TextBoxes and creates a `Appointment` model object.

## Step 3: Business Logic Processing (BLL)
*   **Component**: `AppointmentService.cs`
*   **Action**: The UI calls `_appointmentService.BookAppointment(appointment)`.
*   **Logic**:
    *   **Validation**: "Is the appointment date in the future?" (Implemented).
    *   **Business Rules**: "Is this doctor already booked for this slot?" (Omitted for MVP).
*   **Outcome**: If valid, it proceeds. If invalid, it throws an `Exception` back to the UI.

## Step 4: Data Persistence (DAL)
*   **Component**: `AppointmentRepository.cs`
*   **Action**: The Service calls `_appointmentRepo.InsertAppointment(appointment)`.
*   **Technology**: **In-Memory Data Simulation** (Simulating ADO.NET).
*   **Mechanism**:
    1.  Generates a new ID using `SqlHelper.GetNextId`.
    2.  Adds the appointment object to the static `SqlHelper.Appointments` list.
    3.  (In a full production app, this would execute a SQL Stored Procedure).

## Step 5: The Database (Storage Layer)
*   **Component**: In-Memory List (`SqlHelper.Appointments`)
*   **Action**: The object is stored in the static list in memory.
*   **Transaction**: N/A for In-Memory (Simulated).
*   **Result**: Returns the ID of the new appointment.

## Step 6: The Response (Round Trip)
1.  **DAL** returns the new ID to the **BLL**.
2.  **BLL** returns control to the **UI**.
3.  **UI** updates a Label control: `lblMessage.Text = "Appointment Booked Successfully!"`.
4.  **Rendering**: ASP.NET generates the new HTML (with the success message).
5.  **Response**: The server sends the HTML back to the browser.
6.  **Client**: The user sees the "Appointment Booked Successfully" message.

---

## Summary of Technical Terms
*   **PostBack**: The mechanism of sending the form back to the same page to process events.
*   **Code-Behind**: The C# file (`.aspx.cs`) separate from the HTML (`.aspx`).
*   **ADO.NET**: The core library for connecting .NET to SQL Server (Simulated here with `SqlHelper`).
*   **Stored Procedure**: Pre-compiled SQL code on the server (Simulated).
*   **N-Tier**: The separation of UI, Logic, and Data into distinct layers.
