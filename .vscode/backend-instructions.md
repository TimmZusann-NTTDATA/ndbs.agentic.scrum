# Backend Instructions

Purpose: Define binding backend architecture and implementation rules.

Stack baseline:
- .NET 10 + ASP.NET Core
- EF Core
- OAuth2/OIDC with Entra ID
- REST + OpenAPI with Scalar

## Architecture

### MUST
- Use project split: API + Core + Infrastructure.
- Keep Core free from infrastructure details (database, external services, transport).
- Organize by feature (Vertical Slice) first, then by technical concern.
- Place each feature in its own folder with all required artifacts (for example command/query/handler/validator if used).
- Keep API layer thin (transport, auth boundary, input/output mapping only).
- Use structured logging with `ILogger`.
- Add/propagate correlation ID per request.

### SHOULD
- Use CQRS only when it improves clarity/maintainability.
- Keep feature handlers cohesive and small.
- Keep mapping/validation close to the feature boundary.
- Favor explicit code over generic abstractions.

### CAN
- Introduce shared primitives in SharedKernel when duplication across features becomes real and repeated.
- Use additional architecture checks if complexity grows.

## Data Access

### MUST
- Use EF Core as primary data access strategy.
- Production database target is PostgreSQL.
- Local development must work without Docker and without mandatory local DB installation.
- Provide SQLite-based local fallback for development.

### SHOULD
- Use EF Core InMemory only for tests and specific non-relational scenarios.
- Keep migrations and schema evolution reproducible in CI.

## Dependency Policy

### MUST
- Minimize third-party dependencies.
- Prefer .NET built-in capabilities first.
- Any external package must be open source, free, and permissively licensed (MIT, Apache-2.0, BSD).
- Do not use paid/commercial-only packages.
- Do not use MediatR.

### SHOULD
- Add a short dependency rationale in PR when introducing a package.
- Keep package count low per feature.

### CAN
- Use mapping/validation libraries when they clearly improve readability and maintainability and satisfy license policy.

## API & Security Boundary

### MUST
- Validate all input at API boundary.
- Enforce OAuth2/OIDC integration with Entra ID.
- Keep secrets out of code and repository.
- Expose OpenAPI documentation and keep endpoint contracts current.

### SHOULD
- Use consistent error responses and status code semantics.
- Version API contracts once breaking changes are introduced.

## Quality Gates

### MUST
- PR checklist must include backend architecture and dependency policy checks.
- CI must run build, tests, and lint checks before merge.

### SHOULD
- Include a small architecture impact note for larger feature slices.
