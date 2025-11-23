# High Level Architecture Diagram

This diagram illustrates the **N-Tier Architecture** proposed for the Durdans Hospital Clinic Management System.

```mermaid
graph TD
    subgraph "Client Side"
        Browser["Web Browser (Chrome/Edge)"]
    end

    subgraph "Web Server (IIS)"
        subgraph "Presentation Layer"
            WebForms["ASP.NET Web Forms (.aspx)"]
            UI_Logic["Code-Behind (.aspx.cs)"]
        end

        subgraph "Business Logic Layer (BLL)"
            Services["Business Services"]
            Models["Domain Models"]
            Validations["Validation Rules"]
        end

        subgraph "Data Access Layer (DAL)"
            Repo["Repository Pattern"]
            ADO["ADO.NET / SqlHelper"]
        end
    end

    subgraph "Database Server"
        SQL["SQL Server Database"]
        SPs["Stored Procedures"]
    end

    %% Interactions
    Browser -- "HTTP/HTTPS Request" --> WebForms
    WebForms -- "Events" --> UI_Logic
    UI_Logic -- "Calls" --> Services
    Services -- "Process Data" --> Models
    Services -- "Request Data" --> Repo
    Repo -- "Execute Query" --> ADO
    ADO -- "T-SQL / TDS" --> SQL
    SQL -- "Return Result" --> ADO
    ADO -- "DataTable / DataSet" --> Repo
    Repo -- "Domain Objects" --> Services
    Services -- "Business Objects" --> UI_Logic
    UI_Logic -- "Update View" --> WebForms
    WebForms -- "HTML Response" --> Browser

    %% Styling
    style Browser fill:#f9f,stroke:#333,stroke-width:2px
    style SQL fill:#ccf,stroke:#333,stroke-width:2px
    style WebForms fill:#ff9,stroke:#333,stroke-width:2px
```

## Key Components
1.  **Presentation Layer**: Handles user interaction and renders HTML.
2.  **Business Logic Layer**: Contains core business rules (e.g., checking doctor availability).
3.  **Data Access Layer**: Abstracts database interactions using ADO.NET.
4.  **Database**: SQL Server storing persistent data and business logic in Stored Procedures.
