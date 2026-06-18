---
name: User Story
about: Create a user story for product development
title: "[US] "
labels: ["user-story", "refinement"]
---

## User Story

As a **[role]**, I want **[action]**, so that **[benefit]**.

## Description

<!-- Provide additional context about this user story -->

## Acceptance Criteria

- [ ] Criterion 1
- [ ] Criterion 2
- [ ] Criterion 3

## Notes

<!-- Any additional notes, constraints, or dependencies -->

---

## Development Information

**Branch Convention**: `feature/US-#<issue-number>-<slug>`  
Example: `feature/US-#42-user-authentication`

This user story will be refined into **sub-tasks** in the refinement phase. All sub-tasks will be worked on in the same feature branch to minimize merge conflicts and streamline code reviews.

**Workflow**:
1. Product Owner creates this user story
2. Copilot assists in refinement (creation of sub-tasks with technical details)
3. Agent receives this story + all sub-tasks, checks out a single feature branch
4. Agent completes all sub-tasks in the same branch
5. Agent creates a single PR with all changes
