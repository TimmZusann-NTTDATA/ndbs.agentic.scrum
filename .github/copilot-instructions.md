# Copilot Coding Instructions – Allgemein

Diese Regeln gelten für das gesamte Repository und werden durch die stack-spezifischen
Instructions (`frontend.instructions.md`, `backend.instructions.md`) ergänzt.

## Third-Party-Bibliotheken

- **So wenig wie möglich.** Plattform- und Framework-Bordmittel haben immer Vorrang.
- Bevor eine Bibliothek hinzugefügt wird, prüfen ob die Anforderung mit dem bestehenden
  Stack lösbar ist.
- Jede neue Abhängigkeit muss **quelloffen und kostenlos** (OSS) sein.
- Keine Bibliotheken mit proprietären Lizenzen, Dual-Licensing-Fallstricken (AGPL bei
  SaaS-Nutzung beachten) oder kommerziellen Addons.

## Projektstruktur – Feature Slices

Code wird nach **fachlichen Features** organisiert, nicht nach technischen Schichten.
Jedes Feature ist ein in sich geschlossener Slice mit eigenen Typen, Logik und Tests.
Querschnittliche Infrastruktur (Logging, Auth, DB-Kontext) liegt außerhalb der Slices.

```
# Prinzip gilt für beide Stacks:
features/
  <feature-name>/
    # alles was zu diesem Feature gehört: Modelle, Handler, Tests, UI
```

## Unit Tests

- **Code Coverage ist kein Qualitätsmaßstab** – keine Coverage-Schwellwerte erzwingen.
- Die **zentrale Business-Logik muss vollständig getestet** sein (Domain-Logik,
  Use-Case-Handler, Validierungen, Berechnungen).
- Technische Infrastruktur (Controller-Routing, DB-Migrationen, UI-Glue-Code) braucht
  keine vollständige Testabdeckung.
- Tests müssen:
  - **Klar** – ein Test prüft genau eine Sache
  - **Leicht verständlich** – kein Setup-Overhead, sprechende Namen
  - **Sinnvoll** – kein Testen von Gettern/Settern, Framework-Code oder Trivialitäten

## Sprache

- **Code:** Englisch (Variablen, Methoden, Klassen, Kommentare im Code)
- **Dokumentation / Commit Messages / PR-Beschreibungen:** Deutsch

## Commits

Conventional Commits Format:
```
feat: kurze Beschreibung (#<issue-nr>)
fix: kurze Beschreibung (#<issue-nr>)
test: kurze Beschreibung
chore: kurze Beschreibung
docs: kurze Beschreibung
```
