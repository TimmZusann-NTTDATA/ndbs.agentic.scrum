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

| Label           | Bedeutung                                                   |
|-----------------|-------------------------------------------------------------|
| `Epic`          | Übergeordnetes Ziel                                         |
| `Story`         | User-sichtbares Feature                                     |
| `Bug`           | Defekt / unerwartetes Verhalten                             |
| `Task`          | Technische Aufgabe (von Copilot generiert)                  |
| `Spike`         | Analyse- / Rechercheaufgabe                                 |
| `New`           | Neu erstellt, noch nicht verfeinert (Default)               |
| `ForRefinement` | Bereit für Task-Generierung durch Copilot                   |
| `Ready`         | Vollständig ausgearbeitet, bereit für Dev/Agent             |
| `InProgress`    | Wird gerade bearbeitet                                      |
| `ForReview`     | Wartet auf Review, Test oder Abnahme                        |
| `Done`          | Abgeschlossen und akzeptiert                                |

### Prozess

1. **Mensch erstellt** Epic / Story / Bug → erhält automatisch `New`
2. **Mensch setzt** Label auf `ForRefinement` wenn das Issue inhaltlich klar genug für Task-Generierung ist
3. **Copilot wird beauftragt**, Tasks zu generieren → erstellt Sub-Issues mit vollständiger technischer Ausarbeitung → setzt Parent-Issue auf `Ready`
4. **Mensch weist** die Story / den Bug **dem Agent zu** → Agent erstellt einen einzigen Feature-Branch für die gesamte Story
5. **Agent arbeitet alle Tasks sequenziell** auf demselben Branch ab → setzt Tasks auf `Done` wenn erledigt → setzt Story auf `InProgress`
6. **Agent öffnet einen PR** wenn alle Tasks der Story abgeschlossen sind → setzt Story auf `ForReview`
7. **Mensch reviewed** den PR und merged → setzt Story auf `Done`

### Branch-Strategie

> ⚠️ **Ein Branch pro Story – nicht pro Task.**

- **Branch-Name:** `story/<issue-nummer>-<kurztitel>` z.B. `story/42-user-login`
- **Alle Tasks** einer Story werden auf demselben Branch implementiert
- **Commits** folgen Conventional Commits und referenzieren den jeweiligen Task: `feat: add login endpoint (closes #43)`
- **Ein einziger PR** pro Story gegen `main` → reduziert Review-Aufwand
- Der PR-Titel referenziert die Story: `feat: [#42] User Login`

### Task-Qualitätskriterien

Ein Task ist ausreichend spezifiziert, wenn Copilot:
- Die betroffenen Dateien / Module kennt
- Den technischen Ansatz versteht
- Die Testfälle definiert hat
- Die Akzeptanzkriterien des Parent-Issues kennt
- Abhängigkeiten zu anderen Tasks kennt

---

## Definition of Done (DoD)

### Task-Ebene
- [ ] Code implementiert und kompilierbar
- [ ] Unit Tests geschrieben und grün
- [ ] Keine neuen Linting-Fehler
- [ ] Commit auf dem Story-Branch mit Referenz auf den Task (`closes #<task-nr>`)
- [ ] Task-Issue auf `status: done` gesetzt

### Story-Ebene (nach allen Tasks)
- [ ] Alle Tasks der Story auf `Done`
- [ ] Feature-Dokumentation erstellt / aktualisiert (`docs/features/<feature>.md`)
- [ ] ADR angelegt falls eine Architekturentscheidung getroffen wurde (`docs/adr/`)
- [ ] `architecture.md` aktualisiert falls sich Struktur oder Kontext geändert hat
- [ ] Ein PR gegen `main` geöffnet (Titel: `feat: [#<story-nr>] <Story-Titel>`)
- [ ] Akzeptanzkriterien der Story im PR dokumentiert
- [ ] Story-Issue auf `ForReview` gesetzt (→ nach Merge auf `Done`)
