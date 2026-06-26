---
applyTo: "frontend/**"
---

# Copilot Coding Instructions – Frontend (React / TypeScript)

## Stack

| Bereich           | Technologie                              |
|-------------------|------------------------------------------|
| Framework         | React 19+ mit TypeScript (strict mode)  |
| Package Manager   | **pnpm** (kein npm, kein yarn)          |
| Styling           | TailwindCSS v4                          |
| Server State      | TanStack Query (React Query)            |
| Client State      | Zustand                                 |
| Routing           | TanStack Router                         |
| Formulare         | TanStack Form                           |
| Tabellen          | TanStack Table (falls benötigt)         |
| Tests             | Vitest + React Testing Library          |
| UI-Komponenten    | shadcn/ui (TailwindCSS-kompatibel, OSS) |

> **TanStack-Bibliotheken sind explizit erlaubt** und bevorzugt gegenüber Alternativen.

## Third-Party-Bibliotheken

- Plattform-APIs (`fetch`, `URL`, `localStorage`) und React-Bordmittel zuerst.
- UI-Komponentenbibliotheken **nur wenn TailwindCSS-kompatibel** (z.B. shadcn/ui, Radix UI).
- Keine CSS-in-JS-Bibliotheken (emotion, styled-components) – Tailwind reicht.
- Keine vollständigen UI-Frameworks (Material UI, Ant Design, Chakra) ohne explizite Freigabe.
- Animationen: CSS-Transitions / Tailwind `animate-*` bevorzugen; Framer Motion nur bei
  komplexen Animationsanforderungen.

## Projektstruktur

```
frontend/
  src/
    features/          # Feature Slices – primäre Organisationsebene
      <feature>/
        components/    # Feature-spezifische Komponenten
        hooks/         # Feature-spezifische Hooks
        services/      # API-Calls dieses Features
        store/         # Zustand Stores dieses Features
        types/         # TypeScript-Typen dieses Features
        index.ts       # Öffentliche API des Features (barrel export)
    components/        # Wirklich wiederverwendbare UI-Komponenten (kein Feature-Bezug)
    hooks/             # Globale Custom Hooks
    lib/               # Hilfsfunktionen, Konfiguration
    types/             # Globale TypeScript-Typen
  tests/
```

- Imports zwischen Features **nur über `index.ts`** (barrel exports), nie direkte Pfade.
- Zirkuläre Feature-Abhängigkeiten sind verboten.

## Komponenten

- Ausschließlich **Functional Components** mit Hooks – keine Class Components.
- Props immer mit TypeScript-Interface typisieren:
  ```typescript
  interface UserCardProps {
    user: User;
    onSelect: (id: string) => void;
  }
  ```
- Keine `any` – stattdessen `unknown` mit Type Guard oder konkrete Typen.
- `React.FC` nicht verwenden – direkte Typisierung der Props-Parameter bevorzugen.
- Komponenten sind klein und haben eine einzige Verantwortung (Single Responsibility).

## State Management

- **Server State** (API-Daten): ausschließlich TanStack Query – kein manuelles Fetching
  in `useEffect`.
- **Client State** (UI-Zustand): Zustand Stores – kein prop drilling über mehr als 2 Ebenen.
- **Lokaler Komponenten-State**: `useState` / `useReducer` für kurzlebigen, isolierten State.
- Query Keys sind strukturierte Arrays und werden zentral pro Feature definiert:
  ```typescript
  export const userKeys = {
    all: ['users'] as const,
    detail: (id: string) => ['users', id] as const,
  };
  ```

## API-Kommunikation

- Alle API-Calls liegen in `features/<feature>/services/`.
- Kein `fetch` direkt in Komponenten oder Hooks – immer über Service-Funktionen.
- API-Basis-URL aus Umgebungsvariablen (`import.meta.env.VITE_API_URL`).
- Fehlerbehandlung immer explizit – kein stilles Schlucken von Fehlern.

## Styling

- Ausschließlich **TailwindCSS** für Styling.
- Keine Inline-Styles außer für dynamische Werte die Tailwind nicht abbilden kann.
- Komplexe, wiederverwendbare Klassen-Kombinationen mit `cva` (class-variance-authority)
  kapseln.
- Dark Mode über Tailwind `dark:` Klassen.

## Tests (Vitest + React Testing Library)

```typescript
// Gutes Beispiel: testet Verhalten, nicht Implementierung
it('zeigt Fehlermeldung wenn E-Mail ungültig ist', async () => {
  render(<LoginForm onSubmit={vi.fn()} />);
  await userEvent.type(screen.getByLabelText('E-Mail'), 'keine-email');
  await userEvent.click(screen.getByRole('button', { name: 'Anmelden' }));
  expect(screen.getByText('Bitte gültige E-Mail eingeben')).toBeInTheDocument();
});
```

- **Business-Logik in Hooks und Services vollständig testen.**
- Komponenten-Tests prüfen **Verhalten aus Nutzersicht** (RTL-Philosophie: `getByRole`,
  `getByLabelText` statt `getByTestId`).
- Kein Testen von reinem Rendering ohne Interaktion oder Zustandslogik.
- Mocks für externe API-Calls mit `msw` (Mock Service Worker).
- Test-Datei liegt neben der zu testenden Datei: `feature.ts` → `feature.test.ts`.
