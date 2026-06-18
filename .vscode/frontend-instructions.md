# Frontend Instructions

Purpose: Define binding frontend architecture and implementation rules.

Stack baseline:
- React + Vite + Tailwind
- Component Library: React + TypeScript + Tailwind
- Optional state libraries: Zustand (client state), TanStack Query (server state)

## Architecture

### MUST
- Keep frontend split into app and component library.
- Use Vertical Slice organization with `src/features` and `src/pages`.
- Keep reusable UI primitives in component library, not in random app folders.
- Keep feature-specific UI/logic inside the feature slice.

### SHOULD
- Keep pages focused on composition/routing, not business logic.
- Keep feature boundaries explicit (imports should not bypass boundaries unnecessarily).
- Prefer co-locating tests and feature-specific styles with each slice.

### CAN
- Add `src/shared` for cross-feature utilities and adapters when needed.

## State & Data Fetching

### MUST
- Prefer native React primitives first (`useState`, `useReducer`, `Context`) where sufficient.
- Keep server state handling explicit and predictable.

### SHOULD
- Use TanStack Query for async/server-state workflows.
- Use Zustand only when React primitives become hard to maintain.
- Avoid adding multiple state libraries for the same concern.

## Component Library

### MUST
- Build components with clear API contracts (typed props, sensible defaults).
- Keep visual and behavior consistency across app and library.
- Ensure components are accessible (keyboard support, labels, focus states).

### SHOULD
- Keep components composable and small.
- Document component usage examples close to the component.

### CAN
- Use Storybook if it improves development workflow and remains aligned with dependency policy.

## Dependency Policy

### MUST
- Minimize third-party dependencies.
- Prefer browser and framework-native capabilities first.
- Any external package must be open source, free, and permissively licensed (MIT, Apache-2.0, BSD).
- Do not introduce paid/commercial-only packages.

### SHOULD
- Add a short rationale in PR for each new dependency.

## Quality Gates

### MUST
- PR checklist must include slice-boundary and dependency-policy checks.
- CI must run build, tests, and lint checks before merge.

### SHOULD
- Include screenshot or short demo note when UI behavior changes.
