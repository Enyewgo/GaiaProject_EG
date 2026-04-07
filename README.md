#  GaiaProject - Full Stack Calculator & Text Processor

</p><img width="389" height="953" alt="screenshot" src="https://github.com/user-attachments/assets/8043fd30-785d-484f-b4fa-0729bac6bc4a" />


**Developer:** E.G. - Software Practical Engineer
**Tech Stack:** React (Frontend) | .NET 8 Web API (Backend) | SQL Server (Database)

---

##  About the Project
**Gaia Project** is a comprehensive **Full-Stack** application that features a smart calculator and a text processing engine. This project demonstrates end-to-end development capabilities: from a dynamic User Interface in the browser to server-side logic in C#, and persistent data storage in a relational database.

##  System Architecture

The project follows an **N-Tier Architecture** to ensure clean code and Separation of Concerns:

### 1. Client Side (React)
* **Components:** Functional components built for reusability (Calculator, History List, etc.).
* **State Management:** Using `useState` to update results in real-time without page refreshes (**SPA**).
* **Fetch API:** Handling asynchronous communication (**Async/Await**) with the server.

### 2. Server Side (ASP.NET Core Web API)
* **Controllers:** Acting as the "Traffic Managers" that receive HTTP Requests and return JSON Responses.
* **Models:** Strongly-typed C# Classes defining the data structure in memory.
* **Logic & Algorithms:** Core business logic for mathematical operations and String Reversal algorithms.

### 3. Database Layer (SQL Server & EF Core)
* **DbContext:** The bridge between the C# code and the SQL Database.
* **Entity Framework Core:** Utilizing **Code-First** migrations to automatically generate database tables.
* **Persistence:** Every operation is logged into a SQL table for history tracking and data durability.

---

##  Setup and Installation

1. **Database:**
   - Update the `Connection String` in `appsettings.json` to point to your local SQL Server.
   - Run the command `Update-Database` in the Package Manager Console to create the schema.

2. **Backend:**
   - Run the project via Visual Studio or use the command `dotnet run`.

3. **Frontend:**
   - Navigate to the Client folder.
   - Run `npm install` to install dependencies.
   - Run `npm start` to launch the application in your browser.

---


 Professional Highlights
* **Separation of Concerns:** Distinct boundaries between UI, Logic, and Data.
* **Asynchronous Programming:** Leveraging Async/Await to ensure non-blocking server performance.
* **Clean Code:** Adhering to naming conventions, structured classes, and readable code practices.
* **Data Integrity:** Using SQL Primary Keys and C# Type Safety to prevent data errors.

---
**Developed by E.G. - 2026**
