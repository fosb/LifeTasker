# LifeTasker - Task Management Application

![LifeTasker Screenshot](![alt text](image.png)

A terminal-based task management application built with C# and Terminal.GUI, following the MVVM (Model-View-ViewModel) pattern. Manage tasks with priorities, deadlines, and categories in a clean CLI interface.

## Features

- 🗃️ **Task Management**
  - Add/edit/delete tasks with titles, priorities, categories, and deadlines
  - Priority-based coloring (Extreme → Bright Red, Low → Blue, etc.)
  - Automatic deadline escalation for overdue tasks

- 📊 **Priority System**
  - Intelligent priority scoring algorithm
  - Top 5 priority tasks highlighted
  - Auto-downgrade for missed deadlines

- 🖥️ **Terminal UI**
  - Dual-pane view (Tasks list + Priority tasks)
  - Keyboard shortcuts (Ctrl+N: New, Ctrl+S: Save, Ctrl+Q: Quit)
  - Auto-refresh every minute

## Tech Stack

- **Language**: C# (.NET Core)
- **UI Framework**: [Terminal.GUI](https://github.com/gui-cs/Terminal.Gui)
- **Architecture**: MVVM (Model-View-ViewModel)
- **Data Storage**: JSON serialization

## Project Structure

LifeTasker/
├── Core/               # Core utilities
│   └── Command.cs      # Command pattern implementation
├── Models/             # Data models
│   └── LifeTask.cs     # Task model and enums
├── Services/           # Business logic
│   ├── PriorityManager.cs
│   └── Storage.cs
├── ViewModels/         # ViewModels
│   └── TaskViewModel.cs
├── Views/              # UI Components
│   ├── MainWindow.cs
│   └── TaskDialog.cs
└── Program.cs          # Entry point

### Prerequisites
- [.NET 6 SDK](https://dotnet.microsoft.com/download)
- Terminal with 256-color support (recommended)

### Installation
1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/LifeTasker.git
   cd LifeTasker
   dotnet run --project LifeTasker