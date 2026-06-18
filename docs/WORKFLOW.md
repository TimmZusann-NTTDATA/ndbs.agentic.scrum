# ndbs.agentic.scrum: Complete Workflow Guide

This document describes the complete process of building features using Scrum principles with GitHub Copilot as your refinement partner and AI agents as your development team.

---

## Overview: The Three Phases

```
┌─────────────────────┐
│  Product Owner      │ You: Create User Stories in GitHub
│  (Refinement Phase) │
└──────────┬──────────┘
           │
           ▼
┌─────────────────────┐
│ Product Owner +     │ Copilot helps break down stories
│ Copilot             │ into implementable sub-tasks
│ (Refinement Phase)  │
└──────────┬──────────┘
           │
           ▼
┌─────────────────────┐
│  AI Agent           │ Agent implements all sub-tasks
│  (Development Phase)│ in a single feature branch
└──────────┬──────────┘
           │
           ▼
┌─────────────────────┐
│  Code Review        │ Single PR with all changes
│  (Deployment)       │
└─────────────────────┘
```

---

## Phase 1: Product Owner Creates User Story

### What You Do
1. **Create a new GitHub Issue** using the **User Story template**
2. **Fill in the template**:
   - Title: Clear, concise
   - User story format: "As a [role], I want [action], so that [benefit]"
   - Description: Context about why this matters
   - Acceptance criteria: Testable conditions for "done"

### Example User Story

**Title**: [US] User Registration with Email & Password

```
As a new user, I want to register with my email and password,
so that I can create a secure account and access the platform.

## Description
Users currently have no way to create an account. We need a registration 
system with:
- Email validation
- Password strength requirements
- Confirmation email

## Acceptance Criteria
- [x] User can register with valid email and password
- [x] Duplicate emails are rejected
- [x] Passwords must be at least 8 characters
- [x] User receives confirmation email
- [x] Account is created after email confirmation
```

### What Happens After
- GitHub automatically labels it as `user-story` + `refinement`
- Copilot picks this up for refinement in Phase 2

---

## Phase 2: Copilot Refines with Product Owner

### What Copilot Does
Copilot analyzes the user story and asks clarifying questions:
- "Should we support OAuth in the future?" (design for it?)
- "What's the password hashing strategy?"
- "How long are confirmation links valid?"
- "Any rate limiting on registration attempts?"

### Sub-Task Creation
Together with you, Copilot creates **Sub-Tasks** that break down the user story into implementable pieces:

**Sub-Task #42-1: Implement Email/Password Registration Endpoint**
- Technical Requirements: POST /auth/register endpoint
- Input: `{ email, password }`
- Output: JWT token + User ID
- Edge Cases: Duplicate email, weak password, invalid email
- Testing: 5 specific unit tests listed
- Acceptance Criteria: All checklist items

**Sub-Task #42-2: Implement Email Confirmation Flow**
- Technical Requirements: Send confirmation email, verify link
- Testing: Email sent correctly, token validates, expires after 24h
- Acceptance Criteria: ...

**Sub-Task #42-3: Implement Password Hashing & Validation**
- Technical Requirements: bcrypt with salt rounds, password validation
- Testing: Hash correctness, salt uniqueness, validation rules
- Acceptance Criteria: ...

### Sub-Task Quality Checklist

For each sub-task, Copilot ensures:

- ✅ Clear Technical Requirements
- ✅ Specific Input/Output Format
- ✅ Identified Edge Cases
- ✅ Concrete Testing Requirements (not "add tests", but which tests)
- ✅ Listed Dependencies on other sub-tasks
- ✅ Reasonable scope (1-3 days for an agent)
- ✅ Links to parent user story

### When Refinement is Done

Once all sub-tasks are created and validated:
- Parent user story label: `refinement` → `ready-for-dev`
- All sub-task labels: already `ready-for-dev`
- Agent is ready to start development

---

## Phase 3: AI Agent Implements All Sub-Tasks

### Agent Receives

1. **User Story #42** with acceptance criteria
2. **All Sub-Tasks** (#42-1, #42-2, #42-3) with technical details
3. **Branch convention**: `feature/US-#42-user-registration`

### Agent's Workflow

**Step 1: Create Feature Branch**
```bash
git checkout -b feature/US-#42-user-registration
```

**Step 2: Review All Sub-Tasks**
- Read all requirements
- Understand dependencies
- Plan implementation order

**Step 3: Implement Sub-Tasks Sequentially**

For each sub-task:
```bash
# Implement
# ... write code ...

# Write meaningful tests
# ... test for critical business logic ...

# Commit
git commit -m "Implement email/password registration endpoint

- POST /auth/register
- Email validation
- Password hashing with bcrypt
- JWT token generation

Addresses #42-1"
```

**Step 4: Create Single Pull Request**

When all sub-tasks are complete in the branch:

```
PR Title: [FEATURE] US-#42: User Registration System

PR Description:
## Parent User Story
Closes #42

## Sub-Tasks
- Closes #42-1: Email/Password Registration
- Closes #42-2: Email Confirmation Flow
- Closes #42-3: Password Hashing

All acceptance criteria met. Unit tests for critical business logic.
```

### Key Constraint: 1 Branch, 1 PR

**Don't Do This:**
```
US-42-subtask-1-branch → PR #100
US-42-subtask-2-branch → PR #101
US-42-subtask-3-branch → PR #102
```

**Do This:**
```
feature/US-#42-user-registration → PR #99 (with all 3 sub-tasks)
```

---

## Phase 4: Code Review & Deployment

### Code Review
- Reviewer checks: Does it implement the user story?
- PR references all sub-tasks
- All tests passing
- Code follows `.editorconfig` conventions

### Approval
- Labels updated: `review` → `done`
- PR merged to main

### Deployment
- Feature deployed to production
- User story marked complete

---

## Visual Flow (Detailed)

```
┌──────────────────────────────────────────────────────────────────┐
│                        PHASE 1: PLANNING                         │
├──────────────────────────────────────────────────────────────────┤
│                                                                  │
│  You (Product Owner) create issue:                               │
│  "User Registration with Email & Password"                       │
│                                                                  │
│  GitHub Issue #42 created with:                                  │
│  - Title, Description, Acceptance Criteria                       │
│  - Labels: user-story, refinement                                │
│                                                                  │
└────────────────────┬─────────────────────────────────────────────┘
                     │
                     ▼
┌──────────────────────────────────────────────────────────────────┐
│                   PHASE 2: REFINEMENT                            │
├──────────────────────────────────────────────────────────────────┤
│                                                                  │
│  You + Copilot refine #42:                                        │
│  - Clarify requirements                                           │
│  - Ask "what ifs"                                                 │
│  - Design technical approach                                      │
│                                                                  │
│  Copilot creates sub-tasks:                                       │
│  - #42-1: Register Endpoint                                       │
│  - #42-2: Email Confirmation                                      │
│  - #42-3: Password Security                                       │
│                                                                  │
│  Each sub-task has:                                               │
│  - Technical Requirements                                         │
│  - Input/Output Specification                                     │
│  - Testing Requirements (specific tests)                          │
│  - Acceptance Criteria                                            │
│                                                                  │
│  Status Update:                                                   │
│  - #42 label: refinement → ready-for-dev                          │
│  - #42-1, #42-2, #42-3: already ready-for-dev                     │
│                                                                  │
└────────────────────┬─────────────────────────────────────────────┘
                     │
                     ▼
┌──────────────────────────────────────────────────────────────────┐
│                  PHASE 3: DEVELOPMENT                            │
├──────────────────────────────────────────────────────────────────┤
│                                                                  │
│  Agent receives:                                                  │
│  - User Story #42 (acceptance criteria)                           │
│  - All Sub-Tasks #42-1, #42-2, #42-3 (details)                    │
│                                                                  │
│  Agent creates SINGLE feature branch:                             │
│  git checkout -b feature/US-#42-user-registration                 │
│                                                                  │
│  Agent works sequentially:                                        │
│                                                                  │
│  1. Implement #42-1 (Register Endpoint)                           │
│     - Code                                                        │
│     - Tests                                                       │
│     - Commit with "Addresses #42-1"                               │
│                                                                  │
│  2. Implement #42-2 (Email Confirmation)                          │
│     - Code                                                        │
│     - Tests                                                       │
│     - Commit with "Addresses #42-2"                               │
│     (Same branch!)                                                │
│                                                                  │
│  3. Implement #42-3 (Password Security)                           │
│     - Code                                                        │
│     - Tests                                                       │
│     - Commit with "Addresses #42-3"                               │
│     (Same branch!)                                                │
│                                                                  │
│  Result: One feature branch with 3 commits                        │
│                                                                  │
└────────────────────┬─────────────────────────────────────────────┘
                     │
                     ▼
┌──────────────────────────────────────────────────────────────────┐
│                 PHASE 4: CODE REVIEW                             │
├──────────────────────────────────────────────────────────────────┤
│                                                                  │
│  Agent creates ONE pull request:                                  │
│  Title: [FEATURE] US-#42: User Registration System                │
│  Description: Closes #42, Closes #42-1, Closes #42-2, etc.       │
│                                                                  │
│  Reviewer checks:                                                 │
│  ✓ All acceptance criteria met                                    │
│  ✓ All sub-tasks completed                                        │
│  ✓ Tests for critical business logic present                      │
│  ✓ Code conventions followed                                      │
│                                                                  │
│  Status Update:                                                   │
│  - #42, #42-1, #42-2, #42-3: review → done                        │
│  - PR: approved and merged                                        │
│                                                                  │
└────────────────────┬─────────────────────────────────────────────┘
                     │
                     ▼
┌──────────────────────────────────────────────────────────────────┐
│                     ✅ DEPLOYED                                  │
│                                                                  │
│  Feature live in production                                       │
│  User Story complete                                              │
│                                                                  │
└──────────────────────────────────────────────────────────────────┘
```

---

## Key Principles Explained

### Principle 1: One Story = One Branch = One PR

**Why?**
- Reduces merge conflicts (everything in one branch)
- Simplifies code review (single context)
- Makes rollback easier (one PR to revert if needed)
- Keeps history clean (related commits together)

### Principle 2: Refinement Happens BEFORE Development

**Why?**
- Agent doesn't have to guess what to build
- Sub-tasks are concrete and implementable
- Testing requirements are pre-defined
- No back-and-forth during development

### Principle 3: Sub-Tasks are Tasks, Not Features

**Difference:**
- **Task**: "Implement password hashing" (1 subtask)
- **Feature**: "Entire authentication system" (multiple subtasks)

Sub-tasks are always **part of** a user story, worked on in the same branch.

### Principle 4: Testing = Critical Business Logic

**Not:**
- Every function must have a test
- 100% code coverage required
- Test every edge case

**Instead:**
- Test password validation rules
- Test permission checks
- Test calculation logic
- Test business constraints

---

## Status Labels Throughout the Flow

| Phase | Issue | Labels | Next Action |
|-------|-------|--------|-------------|
| Planning | User Story #42 | `user-story`, `refinement` | Start refinement |
| Refinement | User Story #42 | Remove `refinement`, add `ready-for-dev` | Hand to agent |
| Refinement | Sub-Task #42-1 | `sub-task`, `ready-for-dev` | Agent picks up |
| Development | User Story #42 | Add `in-progress` | Agent working |
| Development | Sub-Task #42-1 | Change to `in-progress` | Agent working |
| Development | All sub-tasks | Change to `review` (in PR) | Code review |
| Review | PR with #42, #42-1, etc. | `review` label | Reviewer approves |
| Done | All issues | Change to `done` | Celebrate! 🎉 |

---

## Common Questions

### Q: What if a sub-task needs a new library?
**A**: Sub-tasks should avoid new dependencies if possible. If needed, request in the sub-task and get approval from Product Owner before starting.

### Q: What if a sub-task takes longer than expected?
**A**: Update the issue comment with progress. Communication is key. Don't rush and cut corners.

### Q: Can sub-tasks be done in parallel?
**A**: If they have no dependencies on each other, yes. But they still go in the same branch and same PR.

### Q: What if I find a bug in a refined sub-task?
**A**: Comment on the sub-task. Do NOT make up a solution. Wait for guidance. Refinement might need adjustment.

### Q: What if a sub-task is too small or too large?
**A**: This should be caught in refinement. If discovered during development, notify the Product Owner.

---

## Tools & Templates

All templates and instructions are in the repository:

| File | Purpose |
|------|---------|
| `.github/ISSUE_TEMPLATE/user-story.md` | Create user stories |
| `.github/ISSUE_TEMPLATE/sub-task.md` | Create sub-tasks |
| `.vscode/refinement-instructions.md` | Detailed refinement guide |
| `.vscode/development-instructions.md` | Detailed development guide |
| `docs/ISSUE-STANDARDS.md` | Best practices for issues |
| `docs/TESTING.md` | Testing standards |

---

## Next Steps

1. **Create your first user story** using the template
2. **Work with Copilot** to refine it into sub-tasks
3. **Hand off to an agent** with all sub-tasks
4. **Review the PR** when complete
5. **Deploy and celebrate!** 🚀

---

## Additional Resources

- **Scrum Reference**: https://www.scrum.org/
- **GitHub Issues**: https://docs.github.com/en/issues
- **Git Branching Model**: https://git-scm.com/book/en/v2/Git-Branching-Branching-Workflows
- **Best Practices**: See `docs/ISSUE-STANDARDS.md`
