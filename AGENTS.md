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
- **Tests:** xUnit, FluentAssertions, NSubstitute
- **Projektstruktur:**
  ```
  backend/
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

- **Branch-Name:** `feature/<issue-nummer>--<kurztitel>` z.B. `feature/42--user-login` (Story) oder `bugfix/<issue-nummer>--<kurztitel>` z.B. `bugfix/7--login-crash` (Bug)
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
- [ ] Task-Issue auf `Done` gesetzt

### Story-Ebene (nach allen Tasks)
- [ ] Alle Tasks der Story auf `Done`
- [ ] Feature-Dokumentation erstellt / aktualisiert (`docs/features/<feature>.md`)
- [ ] ADR angelegt falls eine Architekturentscheidung getroffen wurde (`docs/adr/`)
- [ ] `architecture.md` aktualisiert falls sich Struktur oder Kontext geändert hat
- [ ] Ein PR gegen `main` geöffnet (Titel: `feat: [#<story-nr>] <Story-Titel>`)
- [ ] Akzeptanzkriterien der Story im PR dokumentiert
- [ ] Story-Issue auf `ForReview` gesetzt (→ nach Merge auf `Done`)

---

## Agent-Startup-Guide

Wenn dem Agent eine Story zugewiesen wird, arbeitet er **diese Schritte der Reihe nach ab**.

### 1. Kontext laden

```bash
# Zugewiesene Story laden
gh issue view <story-nr> --json number,title,body,labels,assignees

# Alle Sub-Tasks der Story laden
gh issue list --json number,title,body,labels \
  | jq '[.[] | select(.parent.number == <story-nr>)]'
```

Lies die Story und alle Tasks vollständig. Stelle sicher dass du die Akzeptanzkriterien
und die technischen Anforderungen jedes Tasks verstehst bevor du anfängst.

### 2. Feature-Branch erstellen

```bash
git checkout main
git pull origin main
git checkout -b feature/<story-nr>--<kurztitel>
git push -u origin feature/<story-nr>--<kurztitel>
```

Story-Label auf `InProgress` setzen:
```bash
gh issue edit <story-nr> --remove-label "Ready" --add-label "InProgress"
```

### 3. Tasks sequenziell abarbeiten

Für jeden Task (in Abhängigkeitsreihenfolge):

1. Task-Label auf `InProgress` setzen:
   ```bash
   gh issue edit <task-nr> --remove-label "Ready" --add-label "InProgress"
   ```

2. Code implementieren gemäß Task-Beschreibung

3. Tests ausführen und grün stellen (siehe Build & Test-Befehle)

4. Committen mit Task-Referenz:
   ```bash
   git add .
   git commit -m "feat: <beschreibung> (closes #<task-nr>)"
   git push
   ```

5. Task-Label auf `Done` setzen:
   ```bash
   gh issue edit <task-nr> --remove-label "InProgress" --add-label "Done"
   ```

### 4. Fehlerbehandlung während der Implementierung

- **Tests schlagen fehl:** Fehler beheben bevor der nächste Task begonnen wird. Niemals
  einen Task als `Done` markieren wenn Tests rot sind.
- **Code kompiliert nicht:** Niemals committen. Problem zuerst beheben.
- **Anforderung unklar:** Kontext aus dem Task-Body und dem Parent-Story-Body holen.
  Wenn beides nicht ausreicht, in einem Issue-Kommentar nachfragen und auf Antwort warten.
- **Abhängigkeit zu einem anderen Task nicht erfüllt:** Abhängigen Task zuerst abschließen.

### 5. Pull Request öffnen

Nachdem **alle Tasks** der Story auf `Done` sind:

```bash
gh pr create \
  --title "feat: [#<story-nr>] <Story-Titel>" \
  --body "$(cat <<'EOF'
## Zusammenfassung

Schließt #<story-nr>

<Kurze Beschreibung was implementiert wurde>

## Akzeptanzkriterien

- [x] <Kriterium 1>
- [x] <Kriterium 2>

## Enthaltene Tasks

- #<task-nr> – <task-titel>
- #<task-nr> – <task-titel>

## Technische Hinweise

<Architekturentscheidungen, Besonderheiten, bekannte Einschränkungen>

## Dokumentation

- [ ] `docs/features/<feature>.md` aktualisiert
- [ ] ADR angelegt (falls zutreffend)
EOF
)" \
  --base main
```

Story-Label auf `ForReview` setzen:
```bash
gh issue edit <story-nr> --remove-label "InProgress" --add-label "ForReview"
```

---

## Build & Test-Befehle

### Backend

```bash
# Bauen
dotnet build backend/src/

# Alle Tests ausführen
dotnet test backend/tests/

# Einzelnes Testprojekt
dotnet test backend/tests/Backend.Application.Tests/

# EF Core Migration anlegen
dotnet ef migrations add <MigrationName> \
  --project backend/src/Backend.Infrastructure \
  --startup-project backend/src/Backend.API

# Datenbank aktualisieren
dotnet ef database update \
  --project backend/src/Backend.Infrastructure \
  --startup-project backend/src/Backend.API
```

### Frontend

```bash
# Abhängigkeiten installieren
pnpm install

# Dev-Server starten (http://localhost:5173)
pnpm dev

# Tests ausführen
pnpm test

# Tests einmalig (CI-Modus)
pnpm test --run

# Build prüfen
pnpm build
```

### Vor dem PR: Vollständige Verifikation

```bash
# Backend
dotnet build backend/src/ && dotnet test backend/tests/

# Frontend
cd frontend && pnpm build && pnpm test --run
```

Beide müssen fehlerfrei durchlaufen bevor ein PR geöffnet wird.

---

## Umgebung & Konfiguration

### Backend (`backend/src/Backend.API/appsettings.Development.json`)

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=ndbs-dev.db"
  },
  "Cors": {
    "AllowedOrigins": ["http://localhost:5173"]
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.EntityFrameworkCore": "Warning"
    }
  }
}
```

- **Datenbank:** SQLite für lokale Entwicklung (`ndbs-dev.db` im API-Projektverzeichnis)
- **CORS:** Frontend-Origin `http://localhost:5173` ist in Development erlaubt
- **Ports:** Backend läuft auf `http://localhost:5000` (Development)

### Frontend (`frontend/.env.development`)

```
VITE_API_URL=http://localhost:5000
```

- Nie `.env`-Dateien mit Secrets in Git committen
- `.env.development` ist für lokale Entwicklung und darf committet werden (keine Secrets)

### CORS-Konfiguration (Backend)

In `Program.cs` muss CORS für Development explizit konfiguriert sein:

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("Development", policy =>
        policy.WithOrigins(builder.Configuration
                  .GetSection("Cors:AllowedOrigins").Get<string[]>()!)
              .AllowAnyHeader()
              .AllowAnyMethod());
});

// Nach builder.Build():
if (app.Environment.IsDevelopment())
    app.UseCors("Development");
```

### OpenAPI / Scalar

Das Backend stellt in Development automatisch die **Scalar API Reference** bereit:
- URL: `http://localhost:5000/scalar`
- NuGet-Paket: `Scalar.AspNetCore` (OSS, kostenlos)
- Konfiguration in `Program.cs`:

```csharp
builder.Services.AddOpenApi();

// Nach builder.Build():
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}
```

Kein `AddSwaggerGen` / `UseSwaggerUI` – ausschließlich Scalar verwenden.

