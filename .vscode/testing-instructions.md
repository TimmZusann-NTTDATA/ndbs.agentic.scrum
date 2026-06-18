# Testing Instructions

Purpose: Define project-wide testing expectations and merge gates.

## Testing Philosophy

### MUST
- Unit tests are mandatory.
- Tests must cover critical business logic.
- Tests must be understandable and maintainable.

### SHOULD
- Prefer meaningful test scenarios over broad but shallow coverage.
- Keep tests deterministic and fast.
- Name tests by behavior and expectation.

### CAN
- Add integration and end-to-end tests for high-risk paths.

## Coverage Policy

### MUST
- No fixed global coverage percentage gate.
- Critical business rules must have explicit tests.

### SHOULD
- Track coverage trends and investigate large drops.

## Backend Testing

### MUST
- Test domain/core logic in isolation.
- Test API boundary behavior for input validation and auth constraints.

### SHOULD
- Use SQLite/in-memory approaches for local/CI testability where appropriate.

## Frontend Testing

### MUST
- Test feature-level behavior and key user flows.
- Test component-library behavior for critical reusable components.

### SHOULD
- Keep tests close to feature/components to preserve context.

## CI Gates

### MUST
- CI must execute tests on pull requests.
- Pull requests with failing tests cannot be merged.

### SHOULD
- Add minimal smoke checks for critical workflows in CI.
