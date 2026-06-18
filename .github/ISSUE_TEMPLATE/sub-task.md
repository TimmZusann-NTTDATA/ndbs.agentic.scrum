---
name: Sub-Task (Refinement)
about: Create a technical sub-task during refinement phase
title: "[SUBTASK] "
labels: ["sub-task", "ready-for-dev"]
---

## Parent User Story

<!-- Link to the parent user story -->
Closes #[parent-issue-number]

## Technical Requirements

<!-- Describe what needs to be implemented from a technical perspective -->

### Input
<!-- What data/information is needed as input? -->

### Output
<!-- What should the implementation produce? -->

### Dependencies
<!-- Internal: other sub-tasks, external: libraries, services, APIs -->

## Implementation Details

<!-- Specific details that help the agent implement this correctly -->

### Edge Cases
<!-- What edge cases or error conditions should be handled? -->

### Performance Considerations
<!-- Any performance or scalability requirements? -->

## Testing Requirements

<!-- What critical business logic must be tested? -->

### Unit Tests
- [ ] Test case 1: [description]
- [ ] Test case 2: [description]

### Integration Tests (if applicable)
- [ ] Test case 1: [description]

### Manual Testing (if applicable)
- [ ] Verification step 1
- [ ] Verification step 2

**Testing Philosophy**: Focus on clarity and meaningful coverage of critical business logic, not test count. Each test should validate an important requirement.

## Acceptance Criteria

- [ ] Implementation complete
- [ ] Tests written and passing
- [ ] Code follows project conventions (`.editorconfig`)
- [ ] PR comments addressed

## Resources & References

<!-- Links to documentation, design documents, related issues, etc. -->

---

## Notes for Agent

- This sub-task is part of a larger user story
- Work on the feature branch created for the parent user story: `feature/US-#<parent-issue>-<slug>`
- All sub-tasks of the same user story share the same feature branch
- Do not create a PR per sub-task; commit all changes to the same branch
- The final PR will include all sub-tasks from the parent user story
