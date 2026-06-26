# Backend Setup

## Überblick

Initiales Aufsetzen der .NET-Backend-Solution `ndbs.shopping` gemäß Clean Architecture.
Enthält alle Schichtprojekte, Testkonfiguration sowie einen Demo-Endpunkt für das aktuelle Datum/Uhrzeit.

## Fachliche Anforderungen

Verweis: Issue #6 – [Technical] Backend Solution aufsetzen

## Technischer Ansatz

### Solution-Struktur

```
backend/
  ndbs.shopping.slnx
  src/
    Backend.Domain/          # Entities, Value Objects – keine Abhängigkeiten nach außen
    Backend.Application/     # Use Cases, CQRS Handler – verweist nur auf Domain
    Backend.Infrastructure/  # EF Core, Repositories – verweist auf Domain + Application
    Backend.API/             # Minimal API Endpoints, DI – verweist auf Application + Infrastructure
  tests/
    Backend.Domain.Tests/
    Backend.Application.Tests/
    Backend.API.Tests/
```

**Abhängigkeitsregel:** Domain ← Application ← Infrastructure ← API

### API / Schnittstellen

| Methode | Pfad             | Beschreibung                     |
|---------|------------------|----------------------------------|
| GET     | `/api/system/now` | Gibt aktuelles Datum/Uhrzeit (UTC) zurück |

**Response-Beispiel:**

```json
{
  "timestamp": "2024-01-15T10:30:00.000+00:00"
}
```

### OpenAPI / Scalar

Die Scalar API Reference ist unter `/scalar/v1` erreichbar (nur in Development).

## Bekannte Einschränkungen / offene Punkte

- Keine Datenbankverbindung eingerichtet (EF Core Migrations folgen mit dem ersten Domain-Feature)
- HTTPS-Umleitung ist deaktiviert (lokale Entwicklung über HTTP)
