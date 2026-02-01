# 🚗 CarTracker

CarTracker is a comprehensive solution for vehicle expense management, combining a **Telegram Bot** interface with a **REST API**. It helps car owners track maintenance costs, fuel expenses, and repair history for multiple vehicles.

Built with **.NET 8**, featuring a **DI-driven Modular Architecture** that allows the same business logic to serve both the Telegram Bot and the Web API.

## ✨ Features

*   **Hybrid Architecture:** A single ASP.NET Core application running both a REST API and a Telegram polling background service.
*   **Smart Bot Interface:**
    *   **Interactive Menus:** Navigation using Inline Keyboards (Callback Queries).
    *   **Stateful Dialogs:** Wizard-style data input (e.g., Brand -> Year -> Save) using an In-Memory Session Service.
    *   **No "Giant Switch":** Commands and button clicks are handled using the **Strategy Pattern** and DI injection.
*   **Fleet Management:** Support for multiple vehicles per user (One-to-Many relation).
*   **Expense Tracking:** Logging costs with descriptions and automatic total calculation via Entity Framework Core.
*   **Database:** Fully integrated with **MySQL** using Entity Framework Core (Code-First).

## 🛠 Tech Stack

*   **Core:** .NET 8, ASP.NET Core Web API
*   **Bot Framework:** Telegram.Bot (Polling via `BackgroundService`)
*   **Database:** MySQL, Entity Framework Core 8
*   **Architecture:** Modular Monolith, Dependency Injection, Strategy Pattern
*   **State Management:** In-Memory Concurrent Dictionary Session
*   **Docs:** Swagger UI (for the REST API part)

## 🚀 Getting Started

### Prerequisites

*   .NET 8 SDK
*   MySQL Server (local or Docker)
*   Telegram Bot Token (from [@BotFather](https://t.me/BotFather))

### Installation

1.  Clone the repository:
    ```bash
    git clone https://github.com/YOUR_USERNAME/CarTracker.git
    ```

2.  Configure `appsettings.json`:
    *   Update `ConnectionStrings` with your MySQL credentials.
    *   Insert your `BotToken`.

    ```json
    "ConnectionStrings": {
      "DefaultConnection": "Server=127.0.0.1;Database=cartrackerdb;User=root;Password="
    },
    "BotConfiguration": {
      "BotToken": "YOUR_TOKEN_HERE"
    }
    ```

3.  Apply database initialization (Code-First):
    *   The project uses `db.Database.EnsureCreated()` on startup for quick prototyping.
    *   *Optional:* Run migrations if enabled.

4.  Run the application:
    ```bash
    dotnet run
    ```

## 🤖 Bot Commands

*   `/start` - Open the main menu.
*   **Inline Buttons:** Use the GUI to add expenses, view graphs, and add/delete vehicles.

---
*Created as a PET-project to demonstrate advanced .NET 8 patterns, Dependency Injection, and Telegram Bot architecture.*
