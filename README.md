# TimCo Retail Manager

A retail management system built by TimCo Enterprise Solutions.

In this simulation, we work for TimCo Enterprise Solutions. We build solutions that we can sell to clients. Right now,
we have been assigned to build a retail management system. Overtime, we will get new requirements, or new work entirely,
just like in a real job.

### Table of contents

Looking for [source code examples](#source-code-examples)? No problem!

1. [Introduction](#timco-retail-manager)
2. [Task](#task)
3. [System design and flow](#system-design-and-flow)
    1. [Project folder structure](#project-structure)
    2. [UML Diagram](#uml-diagram)
    3. [User interface showcase](#user-interface)
4. [Development progress](#development-progress)
5. [Tools and Frameworks Used](#tools-and-frameworks-used)

## Task

**Purpose**
1. To provide a clear demonstration on how to build a real-world application.
2. To demonstrate how various topics fit together.
3. To create a learning platform for future demonstrations.

**Goals**
1. Demonstrate modern development techniques.
2. Model how an application grows over time.
3. Create a platform that needs to be upgraded to newer technologies.
4. Create a hands-on learning tool for developers use to simulate a work environment.

## System Design and Flow

We need to build a desktop app that runs a cash register, handles inventory, and manages the entire store. In order to allow
it to grow, though, we will be creating a WebAPI layer. As the initial project requires a desktop application, the customer will
definiately want to move the system online later down the line, without a WebAPI layer, a whole new project would need to be created.

### Project Structure
* **TRMDesktopUI:** WPF project which holds the user interface pages in a MVVM setup.
* **TRMDesktopUI.Library:** Holds the UI related helper classes and user interface models.
* **TRMAPI:** The API project of the application in a MVC setup.
* **TRMDataManager.Library:** Holds the business logic of the API layer.
* **TRMData:** Holds the tables, stored procedures and data publisher for the TRM application.
* **Documentation:** Holds the UML diagrams and any project docs.

---

#### UML Diagram

<img src="https://github.com/hbux/TimCoRetailManager/blob/main/Documentation/trm_db_design_v1.png" />

### User Interface

**Login Page**
<p float="left">
  <img src="https://github.com/hbux/TimCoRetailManager/blob/main/Documentation/login_page.png" />
</p>

**Sales Page**
<p float="left">
  <img src="https://github.com/hbux/TimCoRetailManager/blob/main/Documentation/sales_page.png" />
</p>

**Admin Page**
<p float="left">
  <img src="https://github.com/hbux/TimCoRetailManager/blob/main/Documentation/admin_page.png" />
</p>

## Development Progress

Below is each phase of development, this will include what was done on each phase.

### Phase 1

* Initialised GitHub repository for the project
* Planned out the project using UML's, user interface diagrams and established goals of the project.
* Created several .NET Framework projects, including:
    * UI (WPF)
    * UI Library (business logic for UI)
    * Backend API
    * Backend API Library (business logic for the API)
    * Data project (houses tables and stored procedures)
* Created an MVP (minimum viable product) and achieved a working application, allowing users to:
    * Login to the sales page
    * Display product data
    * Add products to basket and calculate sub-totals, tax and totals
    * Allow users to checkout, saving data to the database
    * Establish multiple roles; cashier, admin and manager
    * Admins can execute CRUD functions on each user's roles
* Transferred from .NET Framework to .NET Core, involving:
    * Moving the UI, and UI Library to .NET Core
    * Unloading the .NET Framework API, creating a new .NET Core API and transferring code to the new API

### Phase 2

No items to currently display

### Phase 3

No items to currently display

## Tools and Frameworks Used

Technologies and frameworks that have been implemented into this application.
* .NET Framework
* .NET Core
* WPF
* WebAPI
* Blazor WebAssembly
* Logging
* Unit testing and mocking
* Entity and Identity Framework
* Git
* Azure DevOps
* Azure for hosting
* SSDT & Dapper
* Dependency Injection
* SOLID Principles

## Source Code Examples

DataAccess code for retrieving and saving different types of data as well as implementing C# SQL transactions.
<p float="left">
  <img src="https://github.com/hbux/TimCoRetailManager/blob/main/Documentation/trm_data_access_code.png" width="50%" height="50%" />
</p>

---

WebAPI sale controller code for posting and getting sale data.
<p float="left">
  <img src="https://github.com/hbux/TimCoRetailManager/blob/main/Documentation/trm_controller_sale_code.png" width="50%" height="50%" />
</p>

--- 

WebAPI user controller for posting data and retrieving user related information.
<p float="left">
  <img src="https://github.com/hbux/TimCoRetailManager/blob/main/Documentation/trm_controller_user_code.png" width="50%" height="50%" />
</p>

---

WPF UI Library endpoint code for commication between the UI application and the WebAPI application.
<p float="left">
  <img src="https://github.com/hbux/TimCoRetailManager/blob/main/Documentation/trm_endpoint_code.png" width="50%" height="50%" />
</p>

---

A UI Library API helper method which aids accessing and communicating with the WebAPI.
<p float="left">
  <img src="https://github.com/hbux/TimCoRetailManager/blob/main/Documentation/trm_helper_code.png" width="50%" height="50%" />
</p>