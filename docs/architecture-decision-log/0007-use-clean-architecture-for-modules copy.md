# 7. Use Clean Architecture for Modules

## Status

Accepted

## Context

Atlas is in the early stages of development, and we aim to establish a robust architecture from the outset. We need to implement a clean and maintainable architecture that can evolve with the project's needs.

## Decision

We choose to adopt Clean Architecture principles to guide our project's architecture. This approach emphasizes separation of concerns and dependency inversion to ensure maintainability, testability, and scalability.

There are 4 layers: 
- **API Layer** - This is the presentation layer, the entry point. This is where all the Controllers live, and it's how the outside world interacts with the Module.
- **Application Layer** - This is the orchestrator for business logic. This layer defines service contracts, and stores all CQRS commands, queries, and their handlers.  
- **Infrastructure Layer** - This stores the implementation of abstractions that are defined within the Domain and Application layers. This is used for integration with any external serivce (DB, Email, etc).
- **Domain Layer** - The core of the module, storing the business logic and core entities.

## Consequences

- Improved Maintainability: Clear separation of concerns and defined boundaries make the codebase easier to understand and modify.
- Enhanced Testability: Isolating business logic facilitates thorough unit testing, leading to higher code quality and fewer bugs.
- Framework Independence: Decoupling from specific frameworks allows for flexibility in adopting or upgrading technologies.