# Copilot Development Mode Instructions

**Purpose**: Guide software agents in implementing sub-tasks from refined user stories.

This mode is active when working with code files, branches, and pull requests during the development phase.

---

## Overview

Development is the phase where an **agent receives a User Story + all Sub-Tasks** and implements them in a single feature branch. This approach minimizes merge conflicts and keeps code reviews focused.

### Standards Stack (Read Before Coding)
- [`backend-instructions.md`](./backend-instructions.md) for backend architecture rules.
- [`frontend-instructions.md`](./frontend-instructions.md) for frontend architecture rules.
- [`testing-instructions.md`](./testing-instructions.md) for test quality gates.
- [`security-instructions.md`](./security-instructions.md) for dependency and secure-coding constraints.

### Key Principle
**1 User Story = 1 Feature Branch = 1 Pull Request** (even with multiple sub-tasks)

All sub-tasks are completed sequentially in the same branch, then submitted as a single PR.

---

## Branch Strategy

### Creating Your Feature Branch

When you receive a user story, create a feature branch named:

```
feature/US-#<issue-number>-<slug>
```

**Examples:**
- `feature/US-#42-user-registration`
- `feature/US-#15-payment-processing`
- `feature/US-#8-email-notifications`

**Steps:**
```bash
# 1. Update main branch
git checkout main
git pull origin main

# 2. Create feature branch from main
git checkout -b feature/US-#42-user-registration

# 3. Commit your work in this branch
git commit -m "Implement user registration endpoint

- Add POST /auth/register endpoint
- Hash passwords with bcrypt
- Return JWT token

Closes #42, Closes #42-1, Closes #42-2"
```

### Branch Lifespan

- **Created**: When development on the user story starts
- **Active**: All sub-tasks completed here (sequentially or in parallel)
- **Closed**: When PR is merged to main

**Do not create separate branches per sub-task.**

---

## Development Workflow

### 1. Receive the User Story

You receive:
- The parent **User Story** (issue #42)
- All **Sub-Tasks** (issues #42-1, #42-2, #42-3)
- Links to acceptance criteria and testing requirements

### 2. Review All Sub-Tasks

Read through all sub-tasks to understand:
- Technical requirements and input/output
- Dependencies between tasks
- Testing requirements (critical business logic)
- Acceptance criteria for each task

### 3. Plan Implementation Order

Determine the optimal order:
- Typically, work sub-tasks without dependencies first
- If Task B depends on Task A, do A before B
- Consider: data models → APIs → UI/integration

### 4. Implement Each Sub-Task

**For each sub-task:**

1. **Understand the requirements**
   - Read: Technical Requirements, Input/Output, Edge Cases
   - Reference: Design docs, related issues

2. **Implement the feature**
   - Follow project conventions (see `.editorconfig`)
   - Write clean, readable code
   - Handle edge cases as specified

3. **Write meaningful tests**
   - Focus on critical business logic (specified in sub-task)
   - Test names should clarify intent: `should_reject_duplicate_email()`
   - Follow testing philosophy: clarity over coverage %, meaningful over exhaustive

4. **Commit your work**
   ```bash
   git commit -m "Implement [sub-task name]
   
   - Detail 1
   - Detail 2
   
   Addresses #42-1"
   ```

5. **Mark sub-task as complete** (update issue)
   - Close the sub-task issue (via commit message or manually)
   - All acceptance criteria checked off

### 5. Create Final Pull Request

Once all sub-tasks are complete:

**PR Title:**
```
[FEATURE] US-#42: User Registration System
```

**PR Description:**
```markdown
## Parent User Story
Closes #42

## Sub-Tasks Included
- Closes #42-1: Implement Email/Password Registration Endpoint
- Closes #42-2: Implement Email Confirmation Flow
- Closes #42-3: Implement Password Hashing & Security

## Summary
This PR implements complete user registration functionality including...

## Testing
All sub-task acceptance criteria met. Unit tests for critical business logic.
Test coverage on business logic as specified in sub-tasks.

## Checklist
- [x] All sub-task acceptance criteria met
- [x] Unit tests for critical business logic
- [x] Code follows .editorconfig conventions
- [x] No merge conflicts
- [x] Ready for code review
```

---

## Testing Standards

### Philosophy
Focus on **meaningful coverage of critical business logic**, not test percentages.

### What to Test
Each sub-task specifies "critical business logic" to test. Examples:
- ✅ Password validation and hashing
- ✅ Email uniqueness constraint
- ✅ JWT token generation
- ✅ Permission checks
- ✅ Calculation logic (discounts, fees, etc.)

### What's Optional
- ❌ Trivial getter/setter tests
- ❌ 100% code coverage pursuit
- ❌ Testing library/framework internals

### Test Naming Convention

Test names should **clarify the requirement**, not be cryptic:

**Good:**
```python
def test_should_reject_registration_with_duplicate_email():
    ...

def test_should_hash_password_with_bcrypt():
    ...

def test_should_expire_confirmation_token_after_24_hours():
    ...
```

**Bad:**
```python
def test_register():
    ...

def test_email():
    ...

def test_token():
    ...
```

### Test Structure (AAA Pattern)

```python
def test_should_reject_duplicate_email():
    # Arrange: Set up test data
    existing_user = User.create(email="test@example.com")
    
    # Act: Perform the action
    response = register_user("test@example.com", "password123")
    
    # Assert: Verify the result
    assert response.status_code == 409
    assert response.json()["error"] == "Email already registered"
```

---

## Code Conventions

Follow the project's `.editorconfig`:

```
- Charset: UTF-8
- Line endings: LF (Unix)
- Indentation: 2 spaces
- Trim trailing whitespace
- Insert final newline
```

For language-specific conventions, check:
- `docs/ISSUE-STANDARDS.md` — project-wide standards
- Language/framework-specific docs in `docs/`

---

## Common Patterns

### Pattern 1: Sequential Sub-Tasks (Dependencies)

```
Subtask 1: Create database schema
  → Subtask 2: Implement repository layer
    → Subtask 3: Implement business logic
      → Subtask 4: Implement API endpoint
        → Subtask 5: Add tests and validation
```

**Implementation**: Work 1→2→3→4→5 in order, commit after each.

---

### Pattern 2: Parallel Sub-Tasks (Independent)

```
Subtask 1: Implement /auth/register endpoint (no dependencies)
Subtask 2: Implement /auth/login endpoint (no dependencies)
Subtask 3: Implement /auth/refresh endpoint (depends on Subtask 1 & 2)
```

**Implementation**: Can work 1 and 2 simultaneously, then 3 when 1 & 2 complete.

---

## Troubleshooting

### Issue: Sub-task requirements are unclear
→ Ask for clarification (in comments on the issue)  
→ Do not guess or improvise

### Issue: Discovered a bug in another sub-task
→ Commit your current work  
→ Comment on the sub-task issue with findings  
→ Wait for guidance (might update acceptance criteria)

### Issue: Need a library not in the project
→ Prefer built-in/platform capabilities first  
→ Dependency must be free, open source, and permissively licensed (MIT/Apache-2.0/BSD)  
→ Do NOT add new dependencies without asking  
→ Comment on the user story with your proposal and license  
→ Wait for Product Owner approval

### Issue: Task takes longer than estimated
→ Update the sub-task issue with progress  
→ Highlight blockers or complexity  
→ Ask for guidance

---

## Acceptance Checklist (Before PR Submission)

Before creating your PR, verify:

- [ ] All sub-tasks implemented
- [ ] All sub-task acceptance criteria met
- [ ] Unit tests for critical business logic written and passing
- [ ] Code follows `.editorconfig` conventions
- [ ] No merge conflicts (branch is up-to-date with main)
- [ ] Commits are clear and reference sub-task issues
- [ ] PR description lists all sub-tasks and parent issue
- [ ] No new warnings or linting errors

---

## Example: Complete Development Flow

**Given:**
- User Story #42: "User Registration System"
- Sub-Task #42-1: "Implement Email/Password Registration Endpoint"
- Sub-Task #42-2: "Implement Email Confirmation Flow"

**Implementation:**

```bash
# 1. Create feature branch
git checkout -b feature/US-#42-user-registration

# 2. Implement Subtask 1
# ... write code, tests ...
git commit -m "Implement email/password registration endpoint

- POST /auth/register endpoint
- Email validation
- Password hashing with bcrypt
- Return JWT token

Addresses #42-1"

# 3. Implement Subtask 2
# ... write code, tests ...
git commit -m "Implement email confirmation flow

- Send confirmation email
- Token validation
- Mark user as confirmed

Addresses #42-2"

# 4. Create PR
git push origin feature/US-#42-user-registration
# → Create PR on GitHub, linking to #42, #42-1, #42-2
```

---

## Questions?

When in doubt, **re-read the sub-task requirements**. They contain all the information you need. If something is truly unclear, ask for clarification before proceeding.

Trust the refinement process — if a sub-task is well-refined, implementation should be straightforward.
