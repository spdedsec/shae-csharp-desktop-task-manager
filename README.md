# shae-csharp-desktop-task-manager

SHAE Task Manager — a console-based task manager written in C# (.NET).

Built to the **SHAE Field Manual Design System** (applied to a text UI):
mono-spaced aligned task tables, ink-on-paper concept mapped to the terminal,
one oxide-red signal for headers/actions, no decorative noise. See `DESIGN.md`.

## Features

- Add task (title, description, due date, priority)
- List tasks (filter by status: todo / done / all)
- Mark task complete / incomplete (`toggle`)
- Delete task (`delete`)
- Save/load to JSON (`~/.shae-task-manager/tasks.json`)
- Formatted console output

## Requirements

- .NET SDK 6.0+ (`dotnet`)

> **Note:** The .NET SDK is not installed in this environment (only the .NET
> runtime is present), so the app cannot be compiled or tested locally here.
> See `scripts/install.sh`.

## Install

```bash
git clone https://github.com/spdedsec/shae-csharp-desktop-task-manager.git
cd shae-csharp-desktop-task-manager
./scripts/install.sh
```

## Build & run

```bash
cd src
dotnet build
dotnet run
```

## Usage

```
SHAE Task Manager v0.1.0
========================================

Commands:
  list          List all tasks
  todo          List incomplete tasks
  done          List completed tasks
  add           Add a new task (interactive)
  toggle <id>   Mark a task complete/incomplete
  delete <id>   Delete a task
  help          Show this help
  quit / exit   Exit the task manager

> list

 ID  TITLE                          DUE         PRIORITY  STATUS
 --  -----                          ---         --------  ------
  1  Finish SHAE tier-3 projects    2026-11-20  High      [todo]

> add
Title: Review release
Description: Check release assets
Due date (YYYY-MM-DD) [blank for none]: 2026-11-19
Priority (low/medium/high) [medium]: high
Task added (id 2): Review release

> toggle 1
Task 1 toggled.

> delete 2
Task 2 deleted.
```

Tasks persist to `~/.shae-task-manager/tasks.json`.

## Project structure

```
src/
  TaskManager.cs  # TaskItem, Priority, TaskManager (JSON persistence)
  Program.cs      # Console UI with formatted output
scripts/install.sh
Makefile
README.md
DESIGN.md
.github/workflows/release.yml
docs/DE.md
docs/ES.md
LICENSE
```

## Release

Releases are created from git tags (`v*`) via
`.github/workflows/release.yml` and packaged as a zip of `src/`.

## License

MIT. See `LICENSE`.
