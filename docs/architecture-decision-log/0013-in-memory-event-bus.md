# 13. In-Memory Event Bus

## Status

Accepted.

## Context

We need some event bus to act as a message broker for the event based communication between modules.

## Possible solutions

### 1. External Service

An external service (e.g. RabbitMQ) that is another cost for the project. Harder to integrate into the program. Probably higher performance when there are many events being communicated. There are network connectivity issues to handle.

### 2. Local, in-memory Event Bus

Easier to implement. No-added cost. Probably slower at high-load.

## Decision

Solution 2, Local, in-memory Event Bus.

Considering this application is a monolith, and no modules have been deployed separately as microservices yet, and so there's no need for an external message bus. Also much simpler.

## Consequences
- Will have to switch to an external message broker if a module is converted into a microservice.
- System is simpler to implement.
- Less external dependencies.