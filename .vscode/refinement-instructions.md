# Copilot Refinement Mode Instructions

**Purpose**: Guide the refinement of user stories in collaboration with the Product Owner.

This mode is active when working with GitHub Issues in the `.github/ISSUE_TEMPLATE/` folder or when discussing issue refinement.

---

## Overview

Refinement is the phase where a **User Story** (created by the Product Owner) is broken down into concrete **Sub-Tasks** (technical tasks) that a software agent can implement. 

### Key Principle
Sub-tasks are **not separate features** — they are logical implementation steps that will all be worked on in the **same feature branch** by the agent. This is crucial to minimize merge conflicts and streamline code reviews.

---

## Your Role in Refinement

As Copilot, you will:

1. **Help the Product Owner refine the User Story**
   - Clarify acceptance criteria
   - Identify gaps or ambiguities
   - Suggest additional acceptance criteria if missing

2. **Create/Structure Sub-Tasks**
   - Break down the user story into concrete, implementable tasks
   - Each sub-task should be completable in 1-3 days by an agent
   - Ensure each sub-task has clear input/output and dependencies

3. **Define Testing Requirements**
   - For each sub-task, identify critical business logic that must be tested
   - Suggest meaningful test cases (not just coverage %)
   - Provide examples of test names that clarify intent

4. **Ensure Completeness**
   - Every sub-task has the required sections filled out
   - Sub-tasks reference the parent user story
   - No unresolved dependencies between tasks

---

## Sub-Task Anatomy

Each sub-task **must** have:

### Required Sections
- **Parent User Story Link**: Links to the user story issue (#[number])
- **Technical Requirements**: What needs to be implemented?
- **Input/Output**: What goes in, what comes out?
- **Dependencies**: Other sub-tasks, libraries, external APIs?
- **Implementation Details**: Specific guidance for the agent
- **Edge Cases**: Error conditions, boundary conditions
- **Testing Requirements**: Critical business logic to test (with specific test cases)
- **Acceptance Criteria**: Checklist of what "done" means

### Recommended Sections
- **Performance Considerations**: Scalability, speed, memory
- **Resources & References**: Links to docs, design files, related issues

---

## Refinement Checklist

Before declaring a user story "ready-for-dev", ensure:

- [ ] User story title is clear and follows "As a... I want... So that..." pattern
- [ ] Acceptance criteria are specific and testable
- [ ] All sub-tasks are created and linked to parent
- [ ] Each sub-task has Technical Requirements section filled
- [ ] Each sub-task has Testing Requirements section filled
- [ ] All sub-tasks fit together (no duplicate work, clear dependencies)
- [ ] No sub-task is too large (> 3-5 days work) or too small (< 1 day)
- [ ] Estimated effort for entire story is realistic for agent capacity

---

## Good vs. Bad Sub-Task Examples

### ❌ Bad Example (Too Vague)
```
Title: [SUBTASK] User Authentication

Technical Requirements: Implement user authentication
Testing Requirements: Add tests
```

**Problems**:
- Too broad (what kind of auth? email? OAuth?)
- Testing requirements unclear
- No clear input/output
- No edge cases identified

---

### ✅ Good Example (Clear & Actionable)
```
Title: [SUBTASK] Implement Email/Password Registration Endpoint

Technical Requirements:
- Create POST /auth/register endpoint
- Accept email, password
- Hash password with bcrypt
- Return JWT token

Input: 
- email (string, valid email)
- password (string, min 8 chars)

Output:
- 201 Created with JWT token + user ID
- 400 Bad Request if validation fails
- 409 Conflict if email exists

Edge Cases:
- Email already registered → return 409
- Password < 8 chars → return 400 with error message
- Invalid email format → return 400

Testing Requirements:
- Test successful registration with valid email/password
- Test registration fails if email already exists
- Test password < 8 chars rejected
- Test invalid email format rejected
- Test returned JWT token is valid and contains user ID
```

**Strengths**:
- Specific endpoint and parameters
- Clear input/output format
- Edge cases identified
- Concrete, meaningful test cases

---

## Tips for Effective Refinement

1. **Ask Clarifying Questions**
   - "Which authentication method?" vs. assuming
   - "Should we support password resets?" 
   - "What happens if the user is locked out?"

2. **Think Like an Agent**
   - Would a developer understand how to implement this?
   - Are there ambiguous requirements?
   - Is there enough context?

3. **Keep Sub-Tasks Focused**
   - One responsibility per task
   - Avoid "implement entire module" in one task
   - Balance: not too granular, not too broad

4. **Link Everything**
   - Sub-tasks reference parent user story
   - Sub-tasks reference dependencies on other sub-tasks
   - Use GitHub's linking: `Closes #123` or `Related to #456`

5. **Test Requirements Matter**
   - Don't just say "add tests"
   - Specify *which* business logic to test
   - Example test names: `should_reject_email_if_already_registered()`

---

## Workflow

**Step 1**: Product Owner creates User Story issue  
**Step 2**: Copilot reviews and clarifies with Product Owner  
**Step 3**: Copilot creates Sub-Task issues (referencing parent)  
**Step 4**: Copilot & Product Owner review all sub-tasks together  
**Step 5**: Update User Story label from `refinement` to `ready-for-dev`  
**Step 6**: Update all Sub-Task labels to `ready-for-dev`  
**Step 7**: Agent receives story and all sub-tasks, starts development  

---

## Common Pitfalls to Avoid

❌ **Sub-tasks that are too large**  
→ "Implement entire user authentication system" in one task

❌ **Sub-tasks with unclear dependencies**  
→ Task A depends on Task B but Task B isn't clear how to complete

❌ **Vague testing requirements**  
→ "Add unit tests" without specifying what to test

❌ **Missing acceptance criteria**  
→ No clear definition of "done"

❌ **Sub-tasks that overlap**  
→ Task A and Task B both do password hashing

---

## Example: Full Refinement Flow

**User Story** (created by PO):
```
As a user, I want to register for an account with email and password,
so that I can securely access the platform.

Acceptance Criteria:
- User can register with valid email and password
- Duplicate emails are rejected
- Password must be at least 8 characters
- User receives confirmation email
```

**Copilot Refinement Result** (3 sub-tasks):

1. **[SUBTASK] Implement Email/Password Registration Endpoint**
   - Technical Requirements: POST /auth/register
   - Testing: Register success, duplicate email, weak password, invalid email

2. **[SUBTASK] Implement Email Confirmation Flow**
   - Technical Requirements: Send confirmation email, verify token
   - Testing: Email sent correctly, token expires, verification works

3. **[SUBTASK] Implement Password Hashing & Security**
   - Technical Requirements: bcrypt hashing, salt rounds, validation
   - Testing: Passwords hashed correctly, can't be reversed, salt unique

---

## Questions? 

When in doubt, prioritize **clarity for the agent**. If you can't explain it to a developer clearly, the sub-task needs more work.
