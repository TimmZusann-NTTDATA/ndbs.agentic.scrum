---
name: Copilot Instructions
description: Root-level instructions for Copilot across the project
---

# Copilot Instructions for ndbs.agentic.scrum

This file provides the overarching instructions for Copilot when working on this project. Copilot operates in two main modes depending on context.

---

## Operating Modes

### 🔄 Refinement Mode

**When**: Working with GitHub Issues, planning, or discussing user story refinement  
**File**: [`.vscode/refinement-instructions.md`](.vscode/refinement-instructions.md)

**Key Activities**:
- Help Product Owner clarify user stories
- Break down user stories into concrete sub-tasks
- Define testing requirements for each sub-task
- Create and refine GitHub issues

**Typical Triggers**:
- Creating new issues in `.github/ISSUE_TEMPLATE/`
- Discussing issue refinement
- Planning work for upcoming sprints
- Clarifying acceptance criteria

---

### 💻 Development Mode

**When**: Working with code files, branches, pull requests, or implementing features  
**File**: [`.vscode/development-instructions.md`](.vscode/development-instructions.md)

**Key Activities**:
- Implement sub-tasks from refined user stories
- Write meaningful tests for critical business logic
- Create feature branches and PRs
- Follow code conventions

**Typical Triggers**:
- Creating or editing code files
- Working on feature branches
- Preparing pull requests
- Debugging or refactoring code

---

## Key Project Principles

### 1. Scrum-Oriented Workflow
- **Product Owner** (you): Creates user stories with acceptance criteria
- **Copilot** (refinement): Helps break down stories into implementable sub-tasks
- **Agents** (development): Implement all sub-tasks in a single feature branch
- **Result**: Single PR per user story, minimal merge conflicts

### 2. Branch Strategy: 1 Story = 1 Branch
```
User Story #42 (feature/US-#42-registration)
  ├── Sub-Task #42-1 ✅ (in same branch)
  ├── Sub-Task #42-2 ✅ (in same branch)
  └── Sub-Task #42-3 ✅ (in same branch)
  
Single PR with all changes → Code Review → Merge
```

**Never**: Create one branch per sub-task  
**Always**: All sub-tasks of a user story in the same feature branch

### 3. Testing: Meaning Over Metrics
- Focus on **critical business logic**, not coverage percentages
- Test names should clarify requirements: `should_reject_duplicate_email()`
- Each test validates an important requirement
- Reference: [`.vscode/development-instructions.md`](./vscode/development-instructions.md#testing-standards)

### 4. Code Conventions
Follow the [`.editorconfig`](.editorconfig):
- Charset: UTF-8
- Line endings: LF
- Indentation: 2 spaces
- Trim trailing whitespace
- Final newline

### 5. Architecture & Dependency Principles
- Project starts with two components: Frontend and Backend.
- Both components can be split into multiple projects (for example, backend with API/Core/Infrastructure, frontend with app + component library).
- Follow Vertical Slice organization where possible (feature-first structure over technical-only folders).
- Third-party libraries must be minimized. Prefer platform/native capabilities first.
- If third-party libraries are used, they must be open source and free, with permissive licenses (MIT, Apache-2.0, BSD).
- Do not introduce copyleft dependencies (GPL/AGPL/LGPL) without explicit Product Owner approval.
- Enforce standards using MUSS/SOLL/KANN rules in the dedicated guideline files.

---

## Labels & Issue States

### Status Labels
- `refinement` — Issue is being refined with Product Owner
- `ready-for-dev` — Issue is ready for agent development
- `in-progress` — Agent is working on this issue
- `review` — PR is under code review
- `done` — Issue completed and merged

### Type Labels
- `user-story` — User-facing requirement
- `sub-task` — Technical task within a user story
- `bug` — Defect or unexpected behavior
- `feature` — New feature or enhancement
- `tech-debt` — Refactoring or infrastructure work

### Size Labels
- `small` — ~1-3 days for an agent
- `medium` — ~3-5 days for an agent
- `large` — ~1-2 weeks for an agent

---

## Useful Documents

| Document | Purpose |
|----------|---------|
| [`.vscode/refinement-instructions.md`](.vscode/refinement-instructions.md) | Detailed refinement guidance |
| [`.vscode/development-instructions.md`](.vscode/development-instructions.md) | Detailed development guidance |
| [`.vscode/backend-instructions.md`](.vscode/backend-instructions.md) | Backend architecture and coding guidelines |
| [`.vscode/frontend-instructions.md`](.vscode/frontend-instructions.md) | Frontend architecture and coding guidelines |
| [`.vscode/testing-instructions.md`](.vscode/testing-instructions.md) | Testing policy and quality gates |
| [`.vscode/security-instructions.md`](.vscode/security-instructions.md) | Security defaults and secure coding rules |
| [`docs/WORKFLOW.md`](docs/WORKFLOW.md) | Complete project workflow (diagram & process) |
| [`docs/ISSUE-STANDARDS.md`](docs/ISSUE-STANDARDS.md) | Best practices for user stories & sub-tasks |
| [`docs/TESTING.md`](docs/TESTING.md) | Testing standards & philosophy |
| [`.github/labels.yml`](.github/labels.yml) | Label definitions |
| [`.github/ISSUE_TEMPLATE/user-story.md`](.github/ISSUE_TEMPLATE/user-story.md) | User story template |
| [`.github/ISSUE_TEMPLATE/sub-task.md`](.github/ISSUE_TEMPLATE/sub-task.md) | Sub-task template |

---

## Quick Checklist: Am I in the Right Mode?

| Scenario | Mode | File |
|----------|------|------|
| Writing user stories with PO | Refinement | `refinement-instructions.md` |
| Breaking down stories into tasks | Refinement | `refinement-instructions.md` |
| Defining test requirements | Refinement | `refinement-instructions.md` |
| Implementing code/features | Development | `development-instructions.md` |
| Writing tests | Development | `development-instructions.md` |
| Creating pull requests | Development | `development-instructions.md` |
| Backend architecture decisions | Development | `backend-instructions.md` |
| Frontend architecture decisions | Development | `frontend-instructions.md` |
| Security and dependency checks | Development | `security-instructions.md` |
| Discussing project standards | Either | Relevant docs |

---

## Getting Started

1. **First Time?** Read [`docs/WORKFLOW.md`](docs/WORKFLOW.md) for the complete process overview
2. **Refining a Story?** Use [`refinement-instructions.md`](.vscode/refinement-instructions.md)
3. **Implementing Code?** Use [`development-instructions.md`](.vscode/development-instructions.md)
4. **Backend Work?** Use [`backend-instructions.md`](.vscode/backend-instructions.md)
5. **Frontend Work?** Use [`frontend-instructions.md`](.vscode/frontend-instructions.md)
6. **Need Testing/Security Gates?** Use [`testing-instructions.md`](.vscode/testing-instructions.md) and [`security-instructions.md`](.vscode/security-instructions.md)
7. **Need Process Standards?** Check [`docs/ISSUE-STANDARDS.md`](docs/ISSUE-STANDARDS.md) and [`docs/TESTING.md`](docs/TESTING.md)

---

## Questions or Clarifications?

When in doubt:
1. **Refinement context?** → Check `.vscode/refinement-instructions.md`
2. **Development context?** → Check `.vscode/development-instructions.md`
3. **Project-wide standards?** → Check `docs/` folder
4. **Code conventions?** → Check `.editorconfig`
