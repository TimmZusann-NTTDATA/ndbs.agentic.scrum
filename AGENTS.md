# AGENTS.md – Projektkontext für Copilot

Dieses Dokument liefert den Kontext, den Copilot benötigt, um Issues eigenständig abzuarbeiten.

---

## Projektbeschreibung

Demo-Anwendung zur Veranschaulichung eines Agentic-Scrum-Workflows mit GitHub Copilot.
Der Workflow zeigt, wie Menschen schlanke Epics/Stories/Bugs erstellen und Copilot daraus
vollständig ausgearbeitete Tasks (Sub-Issues) generiert und abarbeitet.

---

## Tech Stack

### Backend
- **Plattform:** .NET (C#)
- **Architektur:** Clean Architecture (Domain / Application / Infrastructure / API Layer)
- **API-Stil:** REST (ASP.NET Core Web API)
- **ORM:** Entity Framework Core
- **Tests:** xUnit, Moq, FluentAssertions
- **Projektstruktur:**
  ```
  src/
    Backend.Domain/
    Backend.Application/
    Backend.Infrastructure/
    Backend.API/
  tests/
    Backend.Domain.Tests/
    Backend.Application.Tests/
    Backend.API.Tests/
  ```

### Frontend
- **Framework:** React (mit TypeScript)
- **Styling:** TailwindCSS
- **State Management:** React Query (Server State) + Zustand (Client State)
- **Tests:** Vitest + React Testing Library
- **Projektstruktur:**
  ```
  frontend/
    src/
      components/    # Wiederverwendbare UI-Komponenten
      features/      # Feature-Slices (je Feature eigener Ordner)
      hooks/         # Custom Hooks
      services/      # API-Aufrufe
      types/         # TypeScript-Typen
    tests/
  ```

---

## Coding Conventions

- **Sprache:** Deutsch für Kommentare/Doku, Englisch für Code (Variablen, Methoden, Klassen)
- **Commits:** Conventional Commits (`feat:`, `fix:`, `chore:`, `test:`, `docs:`)
- **Branches:** kebab-case Feature-Branches
- **.NET:** Nullable Reference Types aktiviert, Records für Value Objects, keine primitiven Obsession
- **React:** Functional Components + Hooks, keine Class Components; Props immer typisiert

---

## Agentic Scrum Workflow

### Issue-Hierarchie

```
Epic (Mensch erstellt)
  └── Story / Bug (Mensch erstellt)
        └── Task (Copilot generiert & arbeitet ab)
```

### Labels

| Label                  | Bedeutung                                                   |
|------------------------|-------------------------------------------------------------|
| `epic`                 | Übergeordnetes Ziel                                         |
| `story`                | User-sichtbares Feature                                     |
| `bug`                  | Defekt / unerwartetes Verhalten                             |
| `task`                 | Technische Aufgabe (von Copilot generiert)                  |
| `status: backlog`      | Neu erstellt, noch nicht verfeinert (Default)               |
| `status: refinement`   | Bereit für Task-Generierung durch Copilot                   |
| `status: ready`        | Vollständig ausgearbeitet, bereit für Dev/Agent             |
| `status: in-progress`  | Wird gerade bearbeitet                                      |
| `status: done`         | Abgeschlossen                                               |
| `status: blocked`      | Blockiert (Grund im Issue)                                  |

### Prozess

1. **Mensch erstellt** Epic / Story / Bug → erhält automatisch `status: backlog`
2. **Mensch setzt** Label auf `status: refinement` wenn das Issue inhaltlich klar genug für Task-Generierung ist
3. **Copilot wird beauftragt**, Tasks zu generieren → erstellt Sub-Issues mit vollständiger technischer Ausarbeitung → setzt Parent-Issue auf `status: ready`
4. **Dev / Agent** arbeitet Tasks ab (Branch → Code → Tests → PR) → `status: in-progress`
5. **Mensch reviewed** den PR und merged → `status: done`

### Task-Qualitätskriterien

Ein Task ist ausreichend spezifiziert, wenn Copilot:
- Die betroffenen Dateien / Module kennt
- Den technischen Ansatz versteht
- Die Testfälle definiert hat
- Die Akzeptanzkriterien des Parent-Issues kennt
- Abhängigkeiten zu anderen Tasks kennt

---

## Definition of Done (DoD)

- [ ] Code implementiert und kompilierbar
- [ ] Unit Tests geschrieben und grün
- [ ] Keine neuen Linting-Fehler
- [ ] PR erstellt mit Beschreibung
- [ ] Akzeptanzkriterien der Parent-Story erfüllt
