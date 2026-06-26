# ndbs.agentic.scrum

Demo-Anwendung zur Veranschaulichung eines **Agentic Scrum Workflows** mit GitHub Copilot.
Der Workflow zeigt, wie ein kleines Team mit minimalem Aufwand Features entwickeln kann –
Menschen definieren *was* gebaut werden soll, Copilot übernimmt die technische Ausarbeitung
und Implementierung.

---

## 🗺️ Der Workflow auf einen Blick

```
Mensch                        Copilot
──────                        ───────
Epic erstellen
  └── Story / Bug erstellen
        │
        │  (Story ist klar genug?)
        ▼
  Label → ForRefinement
        │
        │                     /refine aufrufen
        │                     Fragen stellen & beantworten
        │                     Sub-Tasks (Issues) anlegen
        ▼
  Label → Ready   ◄───────────────────────────────────────┘
        │
        │  (Story dem Agent zuweisen)
        ▼
        │                     Feature-Branch erstellen
        │                     Tasks sequenziell abarbeiten
        │                     Tests schreiben
        │                     Dokumentation aktualisieren
        ▼
  Label → ForReview ◄──────── PR öffnen
        │
        │  (Review & Merge)
        ▼
  Label → Done
```

---

## 🏷️ Labels

### Typen

| Label | Bedeutung |
|---|---|
| 🟣 `Epic` | Übergeordnetes Ziel, bündelt mehrere Stories |
| 🟢 `Story` | User-sichtbares Feature aus Nutzerperspektive |
| 🔵 `Task` | Technischer Sub-Task – wird von Copilot generiert |
| 🔴 `Bug` | Defekt oder unerwartetes Verhalten |
| 🟡 `Spike` | Analyse- oder Rechercheaufgabe |

### Status

| Label | Bedeutung | Wer setzt es? |
|---|---|---|
| `New` | Neu erstellt, noch nicht bewertet | Automatisch (Template) |
| `ForRefinement` | Bereit für technische Ausarbeitung durch Copilot | Mensch |
| `Ready` | Vollständig ausgearbeitet, bereit für den Agent | Copilot (/refine) |
| `InProgress` | Wird gerade implementiert | Agent |
| `ForReview` | PR offen, wartet auf Review | Agent |
| `Done` | Abgeschlossen und gemerged | Mensch (nach Merge) |

---

## 👣 Schritt-für-Schritt

### 1. Epic erstellen *(optional)*
Ein Epic bündelt zusammengehörige Stories unter einem gemeinsamen Ziel.
→ [Neues Epic erstellen](../../issues/new?template=epic.yml)

### 2. Story oder Bug erstellen
Beschreibe *was* du bauen möchtest – schlank und aus Nutzersicht.
Das Template führt dich durch die nötigen Felder (User Story + Akzeptanzkriterien).

→ [Neue Story erstellen](../../issues/new?template=story.yml)
→ [Neuen Bug melden](../../issues/new?template=bug.yml)

### 3. Story für Refinement freigeben
Wenn die Story inhaltlich klar ist: Label von `New` auf `ForRefinement` ändern.

> Das ist das Signal an Copilot: *„Diese Story ist bereit für die technische Ausarbeitung."*

### 4. Refinement mit Copilot durchführen
Im Copilot Chat den Prompt aufrufen:

```
#refine
```

Copilot zeigt alle Stories mit `ForRefinement`, du wählst eine aus.
Copilot stellt gezielte technische Fragen und legt danach vollständig ausgearbeitete
**Task-Issues** als Sub-Issues an. Die Story wechselt automatisch auf `Ready`.

### 5. Story dem Agent zuweisen
Die Story ist auf `Ready` – weise sie dem Copilot Agent zu.
Der Agent:
- erstellt einen Feature-Branch (`story/<nr>-<titel>`)
- arbeitet alle Tasks sequenziell ab
- schreibt Tests
- aktualisiert die Dokumentation in `docs/`
- öffnet am Ende **einen einzigen PR** für die gesamte Story

### 6. PR reviewen und mergen
Du bekommst einen PR mit allen Änderungen der Story.
Nach dem Merge: Story-Label auf `Done` setzen.

---

## 🌿 Branch-Strategie

> **Ein Branch pro Story – nicht pro Task.**

- Branch-Name: `story/<issue-nr>-<kurztitel>` (z.B. `story/12-user-login`)
- Alle Tasks einer Story landen auf demselben Branch
- Ein einziger PR pro Story gegen `main`
- Commits referenzieren den jeweiligen Task: `feat: add login endpoint (closes #13)`

---

## 📁 Projektstruktur

```
.
├── src/                        # Backend (.NET 10, Clean Architecture)
│   ├── Backend.Domain/
│   ├── Backend.Application/
│   ├── Backend.Infrastructure/
│   └── Backend.API/
├── tests/                      # Backend-Tests (xUnit)
├── frontend/                   # Frontend (React + TypeScript)
│   └── src/
│       └── features/           # Feature Slices
├── docs/                       # Dokumentation (Docs-as-Code)
│   ├── architecture.md
│   ├── adr/                    # Architecture Decision Records
│   ├── diagrams/               # C4-Diagramme (Mermaid)
│   └── features/               # Feature-Dokumentation
└── .github/
    ├── prompts/                # Copilot-Prompts (/refine)
    ├── instructions/           # Copilot Coding Instructions
    └── ISSUE_TEMPLATE/         # Issue-Templates
```

---

## 📖 Weitere Ressourcen

- [`AGENTS.md`](./AGENTS.md) – Vollständiger Projektkontext für den Copilot Agent
- [`docs/architecture.md`](./docs/architecture.md) – Systemarchitektur
- [`docs/adr/`](./docs/adr/) – Architecture Decision Records
