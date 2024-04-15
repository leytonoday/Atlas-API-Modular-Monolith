# 1. Adopting Modular Monolith Architecture

## Status

Accepted

## Context

Considering that the business logic for this application has the potential to be relatively complex, it could be reasonable to implement patterns from Domain-Driven-Design. There may be several separate sub-domains within the system.

If we start off with a monolithic API and don't separate things out properly, there will be high coupling between various aspects of the API, and therefore it'll be significantly harder to migrate to a microservice based architecture in the future (if necessary).

## Decision

Using a modular monolithic architecture.

## Consequences

- Modules cannot be deployed separately; they must be deployed and ran as a single process.
- Modules are loosely coupled, and must communicate asynchronously as much as possible.
- Monolith will be easier to split into microservices in the future.
- Atlas will be split into 3 separate modules (or subdomains): Law (core domain), Plans (supporting domain) and Users (generic domain). 
