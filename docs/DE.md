# shae-csharp-desktop-task-manager — Installations- und Nutzungsanleitung (Deutsch)

SHAE Task Manager — ein konsolenbasierter Task-Manager in C# (.NET).

Entsprechend dem **SHAE Field Manual Design System** (auf eine Text-UI
angewendet): monospace-ausgerichtete Task-Tabellen, Tinte auf Papier
konzeptuell auf das Terminal übertragen, ein Oxid-Rot-Signal für
Header/Aktionen, keine dekorativen Elemente. Siehe `DESIGN.md`.

## Features

- Task hinzufügen (Titel, Beschreibung, Fälligkeitsdatum, Priorität)
- Tasks auflisten (gefiltert nach Status: offen / erledigt / alle)
- Task als erledigt / offen markieren (`toggle`)
- Task löschen (`delete`)
- Speichern/Laden als JSON (`~/.shae-task-manager/tasks.json`)
- Formatierte Konsolenausgabe

## Voraussetzungen

- .NET SDK 6.0+ (`dotnet`)

> **Hinweis:** Das .NET SDK ist in dieser Umgebung nicht installiert (nur
> die .NET Runtime), sodass die App hier nicht kompiliert oder getestet
> werden kann. Siehe `scripts/install.sh`.

## Installation

```bash
git clone https://github.com/spdedsec/shae-csharp-desktop-task-manager.git
cd shae-csharp-desktop-task-manager
./scripts/install.sh
```

## Build & Ausführen

```bash
cd src
dotnet build
dotnet run
```

## Nutzung

```
Commands:
  list          Alle Tasks auflisten
  todo          Offene Tasks auflisten
  done          Erledigte Tasks auflisten
  add           Neuen Task hinzufügen (interaktiv)
  toggle <id>   Task als erledigt/offen markieren
  delete <id>   Task löschen
  help          Hilfe anzeigen
  quit / exit   Beenden
```

Tasks werden in `~/.shae-task-manager/tasks.json` gespeichert.

## Lizenz

MIT. Siehe `LICENSE`.
