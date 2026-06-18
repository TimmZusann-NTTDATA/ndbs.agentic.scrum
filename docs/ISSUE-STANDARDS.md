# Issue & Development Standards

This document defines the standards for writing user stories, creating sub-tasks, and managing the refinement and development process.

---

## User Story Standards

### Format

Every user story **must** follow the user story format:

```
As a [role/persona],
I want [action/feature],
so that [benefit/outcome].
```

### Examples

**✅ Good User Story:**
```
As a registered user,
I want to reset my password via email,
so that I can regain access if I forget my password.
```

**❌ Bad User Story:**
```
Implement password reset
```
(No context about who or why)

### Structure in GitHub Issue

Every user story issue should have:

1. **Title**: Clear, action-oriented
   - ✅ "User Password Reset System"
   - ❌ "Reset"

2. **User Story Format**: As a... I want... So that...

3. **Description**: Additional context
   - Why is this important?
   - What problems does it solve?
   - Any constraints or considerations?

4. **Acceptance Criteria**: Specific, testable conditions
   - Should be checkboxes
   - Should be measurable
   - Should be independent
   - Ideally 3-5 criteria

5. **Notes** (optional):
   - Dependencies on other stories
   - Constraints
   - Open questions

### Example Complete User Story

```markdown
# [US] User Password Reset System

As a registered user,
I want to reset my password via email,
so that I can regain access if I forget my password.

## Description

Currently, users who forget their password have no way to reset it.
This creates a poor user experience and support burden. We need a
password reset flow that:

- Sends a reset link via email
- Verifies the user's identity
- Allows setting a new password
- Expires links after a reasonable time (24 hours)
- Prevents password reuse (can't reuse last 3 passwords)

## Acceptance Criteria

- [ ] User can request password reset from login page
- [ ] Reset email sent to registered email address
- [ ] Reset link is valid for 24 hours
- [ ] User can set new password via reset link
- [ ] Reset link becomes invalid after use
- [ ] User receives confirmation email after successful reset
- [ ] Old password cannot be reused

## Notes

- Integration with email service required (SendGrid or similar)
- Consider rate limiting on reset requests (prevent abuse)
- Should this generate an audit log?
```

---

## Sub-Task Standards

### What is a Sub-Task?

A sub-task is a **concrete implementation task** that:
- Solves part of a user story
- Can be completed independently (but in the same branch)
- Has clear acceptance criteria
- Specifies what tests are needed

A sub-task is **NOT**:
- A separate feature (it's part of a larger story)
- Vague guidance ("implement authentication")
- A task without clear technical requirements

### Sub-Task Anatomy

Every sub-task must have these sections:

#### 1. Parent User Story Link
```
Closes #42
```
(Links to the parent user story)

#### 2. Technical Requirements
Clear description of what needs to be built:

```markdown
## Technical Requirements

Create a POST /auth/reset-password endpoint that:
- Accepts email address as input
- Verifies email exists in the system
- Generates a secure reset token (random 32-byte, base64 encoded)
- Stores token with 24-hour expiration
- Sends reset link via email service
- Returns 200 OK on success
```

#### 3. Input/Output Specification

```markdown
## Input

HTTP POST /auth/reset-password
Content-Type: application/json

{
  "email": "user@example.com"
}

## Output

Success (200 OK):
{
  "message": "Reset link sent to your email",
  "expiresIn": "24 hours"
}

Error (400 Bad Request):
{
  "error": "invalid_email"
}

Error (404 Not Found):
{
  "error": "email_not_found"
}
```

#### 4. Dependencies

```markdown
## Dependencies

Internal:
- User model must exist (assume it does)
- Email service configured (assume SendGrid is set up)

External:
- Sub-Task #42-2 (Password verification endpoint) must be complete first

Blockers:
- None currently
```

#### 5. Implementation Details

```markdown
## Implementation Details

### Token Generation
- Use cryptographically secure random generator
- Generate 32 bytes, encode as base64
- Example: 'K9m2pL7qX8nZ3vW5bY1cD4eF6gH'

### Storage
- Store in `password_resets` table with columns:
  - `id` (UUID)
  - `user_id` (FK to users)
  - `token` (bcrypt hashed)
  - `created_at` (timestamp)
  - `expires_at` (timestamp, 24h from now)
  - `used_at` (NULL until used)

### Email Template
- Subject: "Reset your password"
- Link format: `https://app.example.com/reset?token={token}`
- Include expiration time in email
```

#### 6. Edge Cases

```markdown
## Edge Cases

- **Invalid email**: User enters "not-an-email" → return 400
- **Non-existent email**: User enters valid email not in system → return 404
  (Don't reveal if email is registered, for security)
- **Already reset**: User has active reset token → invalidate old one first
- **Rate limiting**: User requests 10 resets in 1 minute → throttle
  (Prevent email bombing)
- **Account disabled**: User account is disabled → still allow reset
```

#### 7. Testing Requirements

```markdown
## Testing Requirements

### Unit Tests
- [ ] should_generate_valid_reset_token
  - Verify token is 32+ bytes, random, different each call
  - Verify token can be hashed and compared (bcrypt)

- [ ] should_reject_invalid_email
  - Input: "invalid-email"
  - Expected: 400 Bad Request

- [ ] should_reject_nonexistent_email
  - Input: "user-not-in-db@example.com"
  - Expected: 404 Not Found (don't reveal existence)

- [ ] should_store_reset_token_with_expiration
  - Verify token stored in DB
  - Verify expires_at is 24 hours from now

- [ ] should_send_email_with_reset_link
  - Mock email service
  - Verify email sent to correct address
  - Verify link includes token

### Integration Tests (optional)
- [ ] should_create_reset_token_and_send_email_end_to_end
  - Full flow: request reset → verify email sent → verify DB state
```

#### 8. Acceptance Criteria

```markdown
## Acceptance Criteria

- [ ] POST /auth/reset-password endpoint implemented
- [ ] Token generation secure (32+ bytes, random)
- [ ] Token stored hashed in database
- [ ] Email sent to user with reset link
- [ ] Reset link expires after 24 hours
- [ ] Invalid emails rejected with 400
- [ ] Non-existent emails rejected with 404
- [ ] Rate limiting prevents abuse
- [ ] All unit tests passing
- [ ] Code follows project conventions
- [ ] API documentation updated
```

#### 9. Resources & References (Optional)

```markdown
## Resources

- [OWASP Password Reset Best Practices](https://cheatsheetseries.owasp.org/cheatsheets/Forgot_Password_Cheat_Sheet.html)
- Related Sub-Task: #42-2 (Password verification)
- Email Service Docs: SendGrid API reference
- Database schema: `docs/database/password_resets.sql`
```

---

## Good vs. Bad Examples

### ❌ Bad Sub-Task

```
Title: Implement password reset

Technical Requirements: Let users reset their passwords

Testing Requirements: Add tests

Acceptance Criteria:
- [ ] Done
```

**Problems:**
- "Implement" is vague
- No technical details
- Testing requirement is useless
- Acceptance criteria is not testable
- Unclear input/output
- No edge cases identified

---

### ✅ Good Sub-Task

```
Title: Create Password Reset Email Endpoint

Parent: #42 User Password Reset System

Technical Requirements:
- POST /auth/reset-password endpoint
- Accept email, verify in system
- Generate secure reset token
- Store with 24-hour expiration
- Send email with reset link

Input/Output:
[See section above with full spec]

Dependencies:
- User model exists
- SendGrid configured
- No other blockers

Testing Requirements:
- should_generate_secure_reset_token()
- should_reject_invalid_email()
- should_send_email_with_valid_link()
- should_expire_token_after_24_hours()
- should_prevent_email_bombing()

Acceptance Criteria:
- [ ] Endpoint created and responds correctly
- [ ] Token generation is cryptographically secure
- [ ] Email sent with proper format
- [ ] All tests passing
- [ ] Code follows conventions
- [ ] Performance acceptable (< 500ms response)
```

**Strengths:**
- Specific endpoint and behavior
- Clear input/output format
- Edge cases identified
- Meaningful test names
- Verifiable acceptance criteria

---

## Refinement Checklist

Before marking a user story as `ready-for-dev`, verify:

- [ ] **Title is clear and action-oriented**
  - Not: "User stuff"
  - Yes: "User Password Reset System"

- [ ] **User story format is correct**
  - "As a [role], I want [action], so that [benefit]"
  - All three parts present

- [ ] **Acceptance criteria are specific and testable**
  - Not: "Works correctly"
  - Yes: "User can request reset from login page"

- [ ] **All sub-tasks created and linked**
  - Each sub-task references parent (#42)
  - All sub-tasks labeled `sub-task`, `ready-for-dev`

- [ ] **Each sub-task is complete**
  - Technical Requirements: Clear
  - Input/Output: Specified
  - Edge Cases: Identified
  - Testing Requirements: Concrete test cases
  - Acceptance Criteria: Testable

- [ ] **Sub-tasks are appropriately sized**
  - Not too large (> 3-5 days)
  - Not too small (< 1 day)

- [ ] **No ambiguity remains**
  - Could an agent implement this without asking questions?
  - Are all technical details specified?

- [ ] **Dependencies are clear**
  - If Sub-Task A depends on B, is it documented?
  - Are external dependencies identified?

---

## Testing Philosophy

### Focus on Critical Business Logic

**Test These:**
- Business rules (discount calculations, permissions)
- Data validation (email format, password strength)
- Error handling (what happens if email doesn't exist?)
- State changes (user creation, status transitions)

**Don't Need Tests For:**
- Trivial getters/setters
- Framework/library internals
- UI rendering (if using framework tools)

### Test Naming

Test names should clarify the requirement:

```python
# Good: Name describes what is being tested
should_reject_password_less_than_8_characters()
should_send_confirmation_email_after_registration()
should_prevent_duplicate_email_registration()

# Bad: Name is cryptic
test_password()
test_email()
test_register()
```

### Test Structure (AAA Pattern)

```python
def should_reject_duplicate_email_registration():
    # Arrange: Set up test data
    existing_user = User(email="john@example.com")
    db.save(existing_user)
    
    # Act: Perform the action
    result = register_user(
        email="john@example.com",
        password="SecurePass123"
    )
    
    # Assert: Verify the result
    assert result.status_code == 409
    assert result.error == "Email already registered"
```

---

## Common Mistakes to Avoid

### ❌ Sub-Task Too Large
"Implement entire authentication system including registration, login, logout, password reset, social auth..."

✅ **Fix**: Break into multiple sub-tasks for each piece

---

### ❌ Vague Acceptance Criteria
- [ ] "Works"
- [ ] "No errors"
- [ ] "Done"

✅ **Fix**: Specific, measurable criteria
- [ ] "Returns 201 Created with valid JWT token"
- [ ] "Rejects email if already registered"
- [ ] "Password hashed with bcrypt, not stored in plain text"

---

### ❌ No Testing Requirements
"Add tests" (vague)

✅ **Fix**: Specify which tests
- [ ] should_hash_password_correctly()
- [ ] should_reject_weak_password()
- [ ] should_validate_email_format()

---

### ❌ Missing Edge Cases
"User can register" (true, but what about edge cases?)

✅ **Fix**: Identify potential issues
- Invalid email format
- Duplicate email
- Weak password
- Special characters in email
- Very long email address

---

### ❌ Dependencies Not Listed
Sub-Task depends on another but doesn't mention it

✅ **Fix**: Be explicit
"Depends on Sub-Task #42-1 (User model must be created first)"

---

## Summary: The Standards in One Image

```
USER STORY (What to build, from user perspective)
├── As a [role] I want [action] so that [benefit]
├── Description (context, why, constraints)
├── Acceptance Criteria (3-5 specific, testable items)
└── Notes (dependencies, questions)

SUB-TASK 1 (How to build, part 1)
├── Parent Link (#42)
├── Technical Requirements (specific, detailed)
├── Input/Output (exact format)
├── Dependencies (what else is needed)
├── Edge Cases (error conditions)
├── Testing Requirements (specific test cases)
└── Acceptance Criteria (implementation is "done")

SUB-TASK 2 (How to build, part 2)
└── ... same structure ...

SUB-TASK 3 (How to build, part 3)
└── ... same structure ...
```

---

## Tools

All templates live in:
- `.github/ISSUE_TEMPLATE/user-story.md` — Use when creating user stories
- `.github/ISSUE_TEMPLATE/sub-task.md` — Use when creating sub-tasks

Reference this document when refining stories or creating sub-tasks!
