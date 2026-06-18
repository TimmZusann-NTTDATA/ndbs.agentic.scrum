# Testing Standards & Philosophy

This document defines testing requirements and best practices for the ndbs.agentic.scrum project.

---

## Philosophy: Meaningful Coverage, Not Metrics

### Core Principle

```
Quality of tests > Quantity of tests > Code coverage percentage
```

We test **critical business logic**, not "everything". A well-designed test suite with 30% coverage that validates core functionality is better than a bloated suite with 100% coverage of trivial code.

---

## What is "Critical Business Logic"?

### Always Test These

1. **Business Rules & Calculations**
   - Discount logic
   - Tax calculations
   - Permission checks
   - State transitions

2. **Data Validation**
   - Email format validation
   - Password strength requirements
   - Unique constraints (duplicate emails)
   - Field length/type validation

3. **Error Handling**
   - Invalid input handling
   - Edge cases
   - Boundary conditions
   - Error messages

4. **Security-Critical Operations**
   - Password hashing
   - Token generation and validation
   - Permission enforcement
   - Authentication/authorization

5. **Integrations**
   - External API calls
   - Database operations
   - Email/message sending
   - Payment processing

### Don't Need Tests For

- Trivial getters and setters
- Framework/library internals (trust the library!)
- Pure rendering code (UI-specific frameworks have their own tools)
- Logging (unless logging is a business requirement)
- Comments and documentation

### Example: What NOT to Test

```python
# ❌ DON'T test this (trivial getter)
def test_get_user_name():
    user = User(name="John")
    assert user.name == "John"

# ✅ DO test this (business logic)
def should_reject_email_with_invalid_format():
    with pytest.raises(ValidationError):
        User(email="not-an-email")

# ❌ DON'T test this (framework responsibility)
def test_json_serialization():
    # Flask/FastAPI handles this, don't test it
    pass

# ✅ DO test this (your business logic)
def should_return_user_data_in_json_response():
    response = client.get("/users/123")
    assert response.status_code == 200
    assert response.json()["id"] == 123
```

---

## Test Structure: AAA Pattern

Every test should follow the **Arrange-Act-Assert (AAA)** pattern for clarity:

```python
def should_reject_duplicate_email_registration():
    # ===== ARRANGE =====
    # Set up test data and preconditions
    existing_user = User.create(
        email="john@example.com",
        password="SecurePass123"
    )
    
    # ===== ACT =====
    # Perform the action being tested
    result = register_user(
        email="john@example.com",
        password="NewPassword123"
    )
    
    # ===== ASSERT =====
    # Verify the expected result
    assert result.status_code == 409
    assert result.json()["error"] == "Email already registered"
    assert User.count() == 1  # Still only 1 user
```

### Why AAA?
- **Clear structure**: Anyone reading the test understands what it does
- **Maintainable**: Easy to update when requirements change
- **Focused**: Each test does ONE thing
- **Readable**: Like a story: setup → action → verification

---

## Test Naming Convention

Test names should **clarify the requirement**, not be cryptic.

### Format

```
should_[expected_behavior]_[when_condition]

or

should_[expected_behavior]_if_[condition]

or

should_[expected_behavior]_with_[scenario]
```

### Examples

**✅ Good Test Names (Requirement is Clear)**
```python
should_reject_email_with_invalid_format()
should_hash_password_with_bcrypt()
should_expire_reset_token_after_24_hours()
should_prevent_email_duplication_on_registration()
should_send_confirmation_email_after_signup()
should_rate_limit_password_reset_requests()
should_reject_password_less_than_8_characters()
should_allow_user_login_with_correct_credentials()
should_invalidate_old_reset_token_when_new_one_requested()
```

**❌ Bad Test Names (Requirement is Unclear)**
```python
test_email()
test_password()
test_reset()
test_register()
test_auth()
test_validate()
test_1()
test_email_password_auth_flow()  # Too vague, multiple things
```

### Multi-Language Examples

**Python/pytest:**
```python
def should_reject_duplicate_email():
    ...

def should_hash_password_correctly():
    ...
```

**JavaScript/Jest:**
```javascript
it('should reject duplicate email', () => {
    ...
});

it('should hash password correctly', () => {
    ...
});
```

**C#/xUnit:**
```csharp
public void Should_Reject_Duplicate_Email()
{
    ...
}

public void Should_Hash_Password_Correctly()
{
    ...
}
```

---

## Unit Tests: Testing Individual Functions

### Definition
Unit tests verify a single function/method in isolation, mocking external dependencies.

### Example: Email Validation Unit Test

```python
import pytest
from auth.validators import validate_email

def should_accept_valid_email_addresses():
    """Valid emails should pass validation"""
    valid_emails = [
        "user@example.com",
        "john.doe@company.co.uk",
        "test+tag@domain.org"
    ]
    
    for email in valid_emails:
        assert validate_email(email) == True

def should_reject_invalid_email_formats():
    """Invalid email formats should be rejected"""
    invalid_emails = [
        "not-an-email",
        "@example.com",
        "user@",
        "user @example.com",  # space
        ""
    ]
    
    for email in invalid_emails:
        with pytest.raises(ValidationError):
            validate_email(email)

def should_reject_email_longer_than_255_characters():
    """Email exceeding max length should be rejected"""
    long_email = "a" * 250 + "@example.com"
    
    with pytest.raises(ValidationError):
        validate_email(long_email)
```

### Unit Test Checklist

- [ ] Test passes with valid input
- [ ] Test fails with invalid input
- [ ] Test covers edge cases (empty, too long, special chars)
- [ ] Test is isolated (mocks external dependencies)
- [ ] Test name clearly states the requirement
- [ ] Test uses AAA pattern

---

## Integration Tests: Testing Multiple Components

### Definition
Integration tests verify that multiple components work together correctly.

### Example: User Registration Integration Test

```python
import pytest
from fastapi.testclient import TestClient
from app import app
from models import User
from email_service import MockEmailService

client = TestClient(app)

def should_register_user_and_send_confirmation_email():
    """
    Full registration flow:
    1. User submits registration form
    2. User created in database
    3. Confirmation email sent
    """
    # Arrange
    email_service = MockEmailService()
    registration_data = {
        "email": "newuser@example.com",
        "password": "SecurePass123"
    }
    
    # Act
    response = client.post("/auth/register", json=registration_data)
    created_user = User.find_by_email("newuser@example.com")
    
    # Assert
    assert response.status_code == 201
    assert created_user is not None
    assert created_user.email == "newuser@example.com"
    assert email_service.emails_sent[-1]["to"] == "newuser@example.com"
    assert "confirm" in email_service.emails_sent[-1]["body"].lower()
```

### Integration Test Checklist

- [ ] Tests multiple components together
- [ ] Uses real database or appropriate test fixtures
- [ ] Mocks external services (email, payment APIs)
- [ ] Tests the happy path
- [ ] Tests error scenarios
- [ ] Verifies side effects (email sent, DB updated)

---

## Test Coverage: The Right Way

### Wrong Approach: Chase the Percentage

```
"We need 80% coverage" → Run tests, measure coverage → Add tests 
for untested code → Celebrate hitting 80%

Result: Many tests, some meaningless
```

### Right Approach: Test Critical Logic

```
"What could go wrong in production?" → Test those scenarios → 
Coverage is a byproduct

Result: Fewer tests, all meaningful
```

### Coverage Tiers

**Tier 1: Must Test (Critical)**
- Business logic
- Data validation
- Error handling
- Security operations

**Tier 2: Should Test (Important)**
- Integrations
- State changes
- Boundary conditions

**Tier 3: Nice to Test (Optional)**
- Getter/setter methods
- UI rendering
- Logging

### Sample Coverage by Tier

```
Critical business logic:    90-100% coverage ✅
Important flows:            70-80% coverage  ✅
Framework/library code:     Not tested       ✅ (trust the library)
Getters/setters:            0-10% coverage   ✅ (unnecessary)

Overall coverage:           ~40-60% coverage ✅ (FINE! Quality over %!)
```

**Better to have:**
- 60% coverage with all critical logic tested ✅
- Than 90% coverage with trivial code inflating the number ❌

---

## Mocking & Fixtures: Testing in Isolation

### When to Mock

Mock **external dependencies**, not your own code:

```python
# ✅ DO: Mock external email service
@patch('email_service.send_email')
def should_send_confirmation_email(mock_send_email):
    register_user("user@example.com", "password")
    assert mock_send_email.called

# ❌ DON'T: Mock your own functions
@patch('auth.register_user')
def should_register_user(mock_register):
    # This test doesn't test anything meaningful!
    pass
```

### Fixture Example (Reusable Test Data)

```python
import pytest

@pytest.fixture
def valid_user_data():
    """Reusable user registration data"""
    return {
        "email": "testuser@example.com",
        "password": "SecurePass123",
        "name": "Test User"
    }

@pytest.fixture
def existing_user(db):
    """Create a user in the test database"""
    user = User.create(
        email="existing@example.com",
        password="HashedPassword"
    )
    yield user
    user.delete()  # Cleanup after test

# Use fixtures in tests
def should_register_new_user(valid_user_data):
    result = register_user(**valid_user_data)
    assert result.status_code == 201

def should_reject_duplicate_email(existing_user, valid_user_data):
    valid_user_data["email"] = existing_user.email
    result = register_user(**valid_user_data)
    assert result.status_code == 409
```

---

## Testing by Feature Type

### Testing an API Endpoint

```python
def should_return_user_profile_with_200_ok():
    """GET /users/123 returns user data"""
    # Arrange
    user = User.create(id=123, name="John", email="john@example.com")
    
    # Act
    response = client.get("/users/123")
    
    # Assert
    assert response.status_code == 200
    assert response.json()["name"] == "John"
    assert response.json()["email"] == "john@example.com"

def should_return_404_for_nonexistent_user():
    """GET /users/999 returns 404"""
    response = client.get("/users/999")
    assert response.status_code == 404
```

### Testing Business Logic

```python
from billing.discount import calculate_discount

def should_apply_10_percent_discount_for_bulk_orders():
    """Orders over 100 units get 10% discount"""
    price = 100  # $100 per unit
    quantity = 150
    discount_rate = calculate_discount(quantity)
    
    assert discount_rate == 0.10
    assert price * quantity * (1 - discount_rate) == 13500

def should_not_apply_discount_for_small_orders():
    """Orders under 100 units get no discount"""
    quantity = 50
    discount_rate = calculate_discount(quantity)
    
    assert discount_rate == 0.0
```

### Testing Validation

```python
from auth.validators import validate_password

def should_accept_strong_passwords():
    strong_passwords = [
        "SecurePass123!",
        "MyP@ssw0rd",
        "ComplexPassword2024"
    ]
    
    for password in strong_passwords:
        assert validate_password(password) == True

def should_reject_weak_passwords():
    weak_passwords = [
        "123456",           # Too simple
        "password",         # Common word
        "Pass1",            # Too short
        "aaaaaaaaaa",       # No variety
    ]
    
    for password in weak_passwords:
        with pytest.raises(ValidationError):
            validate_password(password)
```

---

## Test Maintenance

### Keep Tests Readable

```python
# ❌ Bad: Magic numbers, unclear
def test_discount():
    result = calculate_discount(150)
    assert result == 0.1

# ✅ Good: Named variables, clear intent
def should_apply_10_percent_discount_for_bulk_orders():
    bulk_order_quantity = 150  # units
    expected_discount_rate = 0.10  # 10%
    
    actual_discount = calculate_discount(bulk_order_quantity)
    
    assert actual_discount == expected_discount_rate
```

### Update Tests When Requirements Change

```python
# Original test
def should_reject_password_less_than_8_characters():
    with pytest.raises(ValidationError):
        validate_password("short")  # < 8 chars

# If requirement changes to 12 characters, update:
def should_reject_password_less_than_12_characters():
    with pytest.raises(ValidationError):
        validate_password("short1234")  # < 12 chars, still not enough
```

### Remove Obsolete Tests

If a feature is removed or business logic changes, delete old tests. Outdated tests are confusing and maintenance burden.

---

## Testing Checklist for Sub-Tasks

When a sub-task is being developed, verify:

- [ ] All testing requirements from the sub-task are implemented
- [ ] Tests use meaningful, descriptive names
- [ ] Tests follow AAA pattern (Arrange-Act-Assert)
- [ ] Critical business logic is tested
- [ ] Edge cases are covered
- [ ] Tests are isolated (mocks for external deps)
- [ ] Tests pass locally before PR submission
- [ ] No flaky tests (tests that pass/fail randomly)
- [ ] Each test tests ONE thing
- [ ] Test code is as clean as production code

---

## Tools & Frameworks

Choose testing frameworks based on your tech stack:

**Python:**
- `pytest` — Simple, powerful, great for unit tests
- `unittest` — Standard library, more verbose

**JavaScript/Node:**
- `Jest` — All-in-one: test runner, assertion, mocking
- `Vitest` — Fast, ESM-first

**C#:**
- `xUnit` — Modern, flexible
- `NUnit` — More features
- `Moq` — Mocking library

**Java:**
- `JUnit` — Standard
- `Mockito` — Mocking
- `TestNG` — Enhanced features

---

## Summary

| Aspect | Standard |
|--------|----------|
| Focus | Critical business logic, not code coverage % |
| Structure | AAA Pattern (Arrange-Act-Assert) |
| Naming | Descriptive, requirement-based |
| Coverage | 40-60% is often sufficient if critical |
| Mocking | External dependencies, not your code |
| Size | Fast, isolated, independent tests |
| Maintenance | Keep tests clean and update with requirements |

---

## Quick Reference

```python
# Good test template
def should_[requirement]_[scenario]():
    """One-line description of what this tests"""
    # Arrange
    test_data = ...
    
    # Act
    result = function_under_test(test_data)
    
    # Assert
    assert result == expected
```

Remember: **A good test is a document that explains what the code should do.**
