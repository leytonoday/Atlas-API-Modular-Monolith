# 11. Business Rules

## Status

Accepted

## Context

We want our invariant business rules to be in one area of the code-base, and immediately obvious when looking at the code-base.

## Decision

Business rules must be added to the system in the form of an implementation of either the `IBusinessRule` or the `IAsyncBusinessRule` interfaces.
Then, in Rich Domain Models when performing some business logic, appropriate business rules must be checked to ensure they aren't broken. If they are, then
exceptions should be thrown.

## Consequences
- We need to have a *BusinessRuleException* to indicate that a business rule has been broken.
- Throwing exceptions has a performance impact in most languages (especially C#).
- We don't have confusing if/else chains and statements to enforce business logic.