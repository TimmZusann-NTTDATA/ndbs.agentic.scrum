# Security Instructions

Purpose: Define secure defaults, dependency constraints, and minimum security checks.

## Secure Defaults

### MUST
- Validate input at all external boundaries.
- Keep secrets out of source code, repository, and pull request text.
- Enforce OAuth2/OIDC with Entra ID for authentication/authorization flows.
- Use least-privilege scope and role assignments.

### SHOULD
- Keep CORS restrictive to known origins.
- Use security-focused defaults for headers and cookies where applicable.

## Dependency & License Security

### MUST
- Keep third-party packages to a minimum.
- Any dependency must be open source, free, and permissively licensed (MIT, Apache-2.0, BSD).
- Reject dependencies with unclear or incompatible licensing.

### SHOULD
- Capture dependency rationale and license in pull request notes.
- Periodically review direct dependencies for necessity.

## Logging & Traceability

### MUST
- Use structured logs.
- Include correlation ID in request lifecycle for traceability.
- Avoid logging secrets and personal sensitive data.

### SHOULD
- Use consistent event naming for auditability.

## Pull Request and CI Controls

### MUST
- PR checklist includes security and license checks.
- CI checks (build, tests, lint) are required before merge.

### SHOULD
- Add automated dependency/license review when workflow maturity increases.
