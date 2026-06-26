---
applyTo: "src/**,tests/**"
---

# Copilot Coding Instructions – Backend (.NET / C#)

## Stack

| Bereich           | Technologie                                      |
|-------------------|--------------------------------------------------|
| Plattform         | **.NET 10** (LTS)                               |
| Sprache           | C# (latest language version)                    |
| Architektur       | Clean Architecture                              |
| API               | ASP.NET Core Web API (Minimal APIs bevorzugt)   |
| ORM               | Entity Framework Core 10                        |
| Tests             | xUnit, FluentAssertions, NSubstitute            |
| Validation        | Built-in DataAnnotations oder FluentValidation  |

## Third-Party-Bibliotheken

- **.NET-Bordmittel haben immer Vorrang** vor NuGet-Paketen.
- Keine Bibliotheken mit kommerziellen Lizenzen oder proprietären Addons.
- Bevor ein Paket hinzugefügt wird, prüfen ob `System.*`, ASP.NET Core oder EF Core
  die Anforderung bereits abdecken.
- Erlaubte Ausnahmen: FluentValidation, FluentAssertions, NSubstitute, Serilog.

## Projektstruktur (Clean Architecture + Feature Slices)

```
src/
  Backend.Domain/
    # Entities, Value Objects, Domain Events, Interfaces
    # Kein Verweis auf andere Projekte
    Features/
      <Feature>/
        <Entity>.cs
        I<Feature>Repository.cs

  Backend.Application/
    # Use Cases, CQRS Handler, DTOs, Validation
    # Verweist nur auf Domain
    Features/
      <Feature>/
        Commands/
          Create<Feature>Command.cs
          Create<Feature>Handler.cs
        Queries/
          Get<Feature>Query.cs
          Get<Feature>Handler.cs
        Validators/
          Create<Feature>Validator.cs

  Backend.Infrastructure/
    # EF Core, externe Services, Repository-Implementierungen
    # Verweist auf Domain + Application
    Persistence/
      Configurations/
      Migrations/
      Repositories/

  Backend.API/
    # Controller / Minimal API Endpoints, Middleware, DI-Registrierung
    # Verweist auf Application + Infrastructure
    Features/
      <Feature>/
        <Feature>Endpoints.cs   # Minimal API
        # oder
        <Feature>Controller.cs  # Controller API

tests/
  Backend.Domain.Tests/
  Backend.Application.Tests/
  Backend.API.Tests/
```

**Abhängigkeitsregel:** Domain ← Application ← Infrastructure ← API  
Pfeile zeigen die erlaubte Verweisrichtung. Niemals umgekehrt.

## C# Coding Standards

- **Nullable Reference Types** sind aktiviert (`<Nullable>enable</Nullable>`).  
  Kein `!` (null-forgiving operator) ohne expliziten Kommentar warum.
- **Records** für Value Objects und unveränderliche DTOs:
  ```csharp
  public record EmailAddress(string Value)
  {
      public static EmailAddress Create(string value) =>
          string.IsNullOrWhiteSpace(value) ? throw new DomainException("...") : new(value);
  }
  ```
- **Keine primitive Obsession** – fachliche Konzepte als eigene Typen (Value Objects).
- **`result`-Pattern** statt Exceptions für erwartbare Fehler in der Application-Schicht:
  Entweder `OneOf`, `Result<T>`, oder eigene Union-Typen – konsistent im Projekt.
- `var` verwenden wenn der Typ aus dem Kontext eindeutig hervorgeht.
- `async/await` durchgängig – kein `.Result` oder `.Wait()`.
- Kein `static` für Services – Dependency Injection verwenden.

## CQRS

- **Commands** ändern Zustand, geben kein Datenobjekt zurück (höchstens eine ID).
- **Queries** sind rein lesend und haben keine Seiteneffekte.
- Handler sind schlank – Business-Logik gehört in Domain-Objekte, nicht in Handler.
- Kein MediatR ohne Diskussion – einfache Handler-Interfaces reichen für diese Demo.

## Entity Framework Core

- **Code First** mit Migrations.
- Konfiguration über `IEntityTypeConfiguration<T>` (Fluent API), keine DataAnnotations
  auf Entities.
- Kein Lazy Loading – explizit mit `Include()` laden.
- Repositories für komplexe Queries; für einfache CRUD darf der DbContext direkt in
  Handlern genutzt werden.
- Keine rohen SQL-Strings ohne zwingenden Grund – LINQ bevorzugen.

## API-Design (Minimal APIs)

```csharp
// Endpoints als Extension Methods gruppiert nach Feature
public static class UserEndpoints
{
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/users").WithTags("Users");
        group.MapGet("/", GetAllUsers);
        group.MapPost("/", CreateUser);
        return app;
    }
}
```

- HTTP-Statuscodes korrekt verwenden: `200`, `201`, `204`, `400`, `404`, `422`, `500`.
- Validierungsfehler als `ValidationProblemDetails` (RFC 7807).
- Versionierung über URL-Präfix (`/api/v1/`) wenn mehrere Versionen koexistieren.

## Tests (xUnit + FluentAssertions + NSubstitute)

```csharp
// Gutes Beispiel: Arrange / Act / Assert, sprechender Name
[Fact]
public void CreateOrder_WithInvalidQuantity_ThrowsDomainException()
{
    // Arrange
    var product = Product.Create("Test", Money.Euro(10));

    // Act
    Action act = () => Order.Create(product, quantity: 0);

    // Assert
    act.Should().Throw<DomainException>()
       .WithMessage("*quantity*");
}
```

- **Domain-Logik vollständig testen** (Entities, Value Objects, Domain Services).
- **Application-Handler** mit gemockten Repositories testen (NSubstitute, kein Moq).
- **Integrationstests** für API-Endpoints mit `WebApplicationFactory<Program>`.
- Kein Testen von EF Core Migrations, reinen DTOs oder trivialen Properties.
- Test-Klassen: eine Datei pro zu testende Klasse, `<Klasse>Tests.cs`.
- Keine `[Theory]` mit mehr als 5–6 Fällen – lieber sprechende `[Fact]`-Methoden.

## Fehlerbehandlung

- **Domain-Exceptions** für Verletzungen fachlicher Invarianten (`DomainException`).
- **Globaler Exception Handler** in der API-Schicht (Problem Details Middleware).
- Keine `try/catch` in Handlern für erwartbare Fehler – Result-Pattern verwenden.
- Logging mit `ILogger<T>` (Microsoft.Extensions.Logging) – kein `Console.WriteLine`.
