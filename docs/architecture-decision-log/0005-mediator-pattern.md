# 5. The Mediator Pattern

## Status

Accepted

## Context

As a result of using the CQRS pattern, there's a class (or record) for each Command or Query, and another class for it's associated handler. Wiring all the commands/queries up with their associated
handlers manually is a comborsome, error-prone task that will be hard to maintain. Therefore, the Mediator pattern can be employed to simplify this process. Also, it entirely decouples the module from the 
query or command handler logic. They communicate indirectly.

## Decision

The mediator pattern shall be implemented.

## Consequences

- Reduced coupling.
- Simplified CQRS.
- More maintianable system.