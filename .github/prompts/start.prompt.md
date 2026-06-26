---
mode: autopilot
description: Startet die Implementierung einer Story / eines Bugs – arbeitet alle Sub-Tasks sequenziell oder parallel ab und öffnet am Ende einen PR
---

Du bist ein erfahrener Softwareentwickler. Deine Aufgabe ist es, eine zugewiesene Story oder
einen Bug vollständig zu implementieren – vom Feature-Branch bis zum fertigen Pull Request.

Lies zunächst **vollständig** `AGENTS.md` um Konventionen, Tech Stack und den Prozess zu
verstehen, bevor du mit einem einzigen Schritt beginnst.

---

## Schritt 1: Kontext laden

### 1a – Offene Ready-Issues ermitteln

```bash
gh issue list --label "Ready" --assignee "@me" --json number,title,labels,body --limit 50
```

Filtere auf Issues mit Label `Story`, `Bug` oder `Technical` und zeige sie als Liste:
```
#<nr> [Story|Bug|Technical] – <title>
```

Falls keine Issues gefunden werden:
> Keine dir zugewiesenen Issues mit Label `Ready` gefunden.
> Bitte ein Issue auf `Ready` setzen und dir zuweisen.

Dann beende den Prozess.

### 1b – Issue auswählen

Wenn genau ein Issue gefunden wurde, wähle es automatisch.
Wenn mehrere gefunden wurden, frage den Nutzer:
> Welches Issue möchtest du implementieren? (Nummer eingeben)

Lade den vollständigen Issue-Body:
```bash
gh issue view <nummer> --json number,title,body,labels,assignees
```

### 1c – Sub-Tasks laden

Lade alle Sub-Issues (Tasks) der Story:
```bash
gh issue list --json number,title,body,labels \
  | jq '[.[] | select(.labels[].name == "Task")]'
```

Filtere auf Tasks deren Body `**Parent Story:** #<story-nummer>` enthält.

Zeige eine Übersicht:
```
Story  #<nr> – <titel>
Tasks:
  #<nr> – <titel>  [abhängig von: #X | unabhängig]
  #<nr> – <titel>  [abhängig von: #X | unabhängig]
  ...
```

> **Hinweis:** Falls keine Tasks vorhanden sind, brich ab und weise den Nutzer darauf hin,
> dass die Story zunächst mit `/refine` verfeinert werden muss.

---

## Schritt 2: Abhängigkeiten analysieren

Lies für jeden Task den Abschnitt `## 🔗 Abhängigkeiten` aus dem Issue-Body.

Baue eine Abhängigkeitsliste:
- **Unabhängige Tasks** (keine Abhängigkeiten oder alle Abhängigkeiten bereits `Done`):
  → können **parallel** implementiert werden
- **Abhängige Tasks** (blockiert von einem anderen noch nicht fertigen Task):
  → müssen **nach** ihrem Vorgänger implementiert werden

Erzeuge einen Ausführungsplan in Ebenen:

```
Ausführungsplan:
  Ebene 1 (parallel): #<nr>, #<nr>, #<nr>
  Ebene 2 (nach Ebene 1): #<nr>, #<nr>
  Ebene 3 (nach Ebene 2): #<nr>
```

---

## Schritt 3: Feature-Branch anlegen

Bestimme den Branch-Präfix anhand des Issue-Labels:
- `Story`    → `feature/<story-nr>--<kurztitel>`
- `Bug`      → `bugfix/<story-nr>--<kurztitel>`
- `Technical`→ `chore/<story-nr>--<kurztitel>`

```bash
git checkout main
git pull origin main
git checkout -b <branch-name>
git push -u origin <branch-name>
```

Story-Label auf `InProgress` setzen:
```bash
gh issue edit <story-nr> --remove-label "Ready" --add-label "InProgress"
```

---

## Schritt 4: Tasks abarbeiten

### Parallelisierungsstrategie

Für **jede Ebene** im Ausführungsplan gilt:

**Ebene mit einem Task** → direkt in dieser Session implementieren (Schritt 4a).

**Ebene mit mehreren unabhängigen Tasks** → Unteragenten starten:

Nutze das `orchestrate`-Skill um für jeden Task in der Ebene eine eigene Sub-Session
zu starten. Gib jeder Sub-Session folgende Anweisung:

```
Implementiere Task #<task-nr> der Story #<story-nr> auf Branch <branch-name>.

Kontext:
- Task-Body: <vollständiger task-body>
- Story-Akzeptanzkriterien: <akzeptanzkriterien>
- Branch: <branch-name> (bereits erstellt, bitte auschecken mit `git checkout <branch-name>`)

Vorgehen:
1. Task-Label auf InProgress setzen: gh issue edit <task-nr> --remove-label "Ready" --add-label "InProgress"
2. Code gemäß Task-Beschreibung implementieren (Tech Stack und Conventions aus AGENTS.md beachten)
3. Tests ausführen und grün stellen
4. Committen: git add . && git commit -m "<typ>: <beschreibung> (closes #<task-nr>)" && git push
5. Task-Label auf Done setzen: gh issue edit <task-nr> --remove-label "InProgress" --add-label "Done"

Wichtig:
- Niemals committen wenn Tests fehlschlagen
- Niemals niemals auf main pushen
- Branch: <branch-name>
```

Warte bis alle Sub-Sessions der Ebene abgeschlossen sind, bevor du mit der nächsten
Ebene weitermachst.

### 4a – Einzelnen Task implementieren

Für jeden Task (oder in dieser Session bei Einzeltasks):

1. Task-Label auf `InProgress` setzen:
   ```bash
   gh issue edit <task-nr> --remove-label "Ready" --add-label "InProgress"
   ```

2. Code implementieren gemäß Task-Beschreibung (`## 🎯 Aufgabe`, `## 🔧 Technischer Ansatz`,
   `## 📁 Betroffene Dateien / Module`).

3. Tests ausführen:

   **Backend:**
   ```bash
   dotnet build backend/src/ && dotnet test backend/tests/
   ```

   **Frontend:**
   ```bash
   cd frontend && pnpm test --run
   ```

   Bei Fehlern: beheben bevor weitergemacht wird. Niemals einen Task als `Done` markieren
   wenn Tests rot sind.

4. Committen mit Task-Referenz:
   ```bash
   git add .
   git commit -m "<typ>: <kurzbeschreibung> (closes #<task-nr>)"
   git push
   ```

5. Task-Label auf `Done` setzen:
   ```bash
   gh issue edit <task-nr> --remove-label "InProgress" --add-label "Done"
   ```

### 4b – Nächste Ebene

Wiederhole Schritt 4 für jede Ebene im Ausführungsplan, bis alle Tasks `Done` sind.

---

## Schritt 5: Abschluss-Verifikation

Führe die vollständige Verifikation durch:

```bash
# Backend
dotnet build backend/src/ && dotnet test backend/tests/

# Frontend
cd frontend && pnpm build && pnpm test --run
```

Beide müssen fehlerfrei durchlaufen. Bei Fehlern: beheben und erneut verifizieren.

Prüfe ob `docs/features/<feature>.md` aktualisiert oder erstellt wurde.
Falls nicht, erstelle oder aktualisiere die Dokumentation gemäß `docs.instructions.md`.

Falls eine wesentliche Architekturentscheidung getroffen wurde, lege ein ADR unter
`docs/adr/` an.

---

## Schritt 6: Pull Request öffnen

```bash
gh pr create \
  --title "<typ>: [#<story-nr>] <Story-Titel>" \
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

## Schritt 7: Abschlussmeldung

```
✅ Implementierung abgeschlossen für Story #<nr>: <titel>

Branch:   <branch-name>
PR:       #<pr-nr> – <pr-titel>

Abgearbeitete Tasks:
  ✓ #<nr> – <titel>
  ✓ #<nr> – <titel>
  ...

Die Story wurde auf `ForReview` gesetzt.
👉 Der PR wartet auf deinen Review.
```

---

## Fehlerbehandlung

| Situation | Vorgehen |
|-----------|----------|
| Tests schlagen fehl | Fehler beheben, **niemals** mit rotem Build committen |
| Code kompiliert nicht | Problem zuerst lösen, niemals committen |
| Anforderung unklar | Task-Body und Story-Body konsultieren; falls unklar, Kommentar im Issue und warten |
| Abhängigkeit nicht erfüllt | Abhängigen Task zuerst abschließen |
| Sub-Session schlägt fehl | Task in dieser Session nachimplementieren (Schritt 4a) |
| Merge-Konflikt beim Push | `git pull --rebase origin <branch-name>` und Konflikt auflösen |
