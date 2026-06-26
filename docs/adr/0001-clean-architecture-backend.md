# ADR-0001: Clean Architecture für das .NET Backend

## Status

Akzeptiert

## Kontext

Für das Demo-Projekt wird ein Backend benötigt, das als Vorlage für skalierbare .NET-Anwendungen
dienen soll. Es müssen eine klare Trennung der Verantwortlichkeiten und gute Testbarkeit
sichergestellt werden.

## Entscheidung

Das Backend wird nach dem **Clean Architecture**-Prinzip mit vier Schichten strukturiert:

- **Backend.Domain** – Entities, Value Objects, Domain Events (keine externen Abhängigkeiten)
- **Backend.Application** – Use Cases, CQRS Handler, DTOs (verweist nur auf Domain)
- **Backend.Infrastructure** – EF Core, externe Services, Repository-Implementierungen
- **Backend.API** – Minimal API Endpoints, Middleware, DI-Registrierung

Die Abhängigkeitsregel gilt strikt: Domain ← Application ← Infrastructure ← API.

Als API-Framework werden **ASP.NET Core Minimal APIs** verwendet.
Für die API-Dokumentation wird **Scalar.AspNetCore** eingesetzt (kein SwaggerUI).

## Konsequenzen

**Positiv:**
- Business-Logik ist isoliert testbar (Domain und Application ohne Infrastrukturabhängigkeiten)
- Klare Zuständigkeiten erleichtern Onboarding und Erweiterungen
- Scalar bietet eine moderne, wartungsarme API-Dokumentation

**Negativ:**
- Mehr Projektdateien und initiales Boilerplate im Vergleich zu einem monolithischen Ansatz
- Feature-übergreifende Änderungen erfordern Anpassungen in mehreren Schichten
