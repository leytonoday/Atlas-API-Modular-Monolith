# 8. Rich Domain Models

## Status

Accepted

## Context

A domain model should represent the solution to a particular domain problem. Each domain model will probably be mapped 1:1 with a DB table. 

## Possible Solutions

1. We could have an Anemic Domain Model, where the business logic that operates upon the domain models exists within service classes
2. Create Rich Domain Models where the business logic that operates upon the domain models exists ON the model itself.

## Decision

Solution 2 - Rich Domain Model.

1. No, because the business logic of the project could be relatively complex, and we cannot afford corruption by duplication. That is to say, we cannot allow various arbitrary services to create and manipulate
the domain entity. Just because we have a specific serivce for doing that, it doesn't stop some other serivce from also doing it, thus creating the possibility for "corruption by duplication".

We want the take advantage of OOP principles such as encapsulation and abstraction, by only allowing the domain model to be manipulated via public methods that exist on the domain model class. Even the 
creation of a domain model should only be achieved via a public static "Create" method (i.e., they should have private constructors).

## Consequences

- All domain models (represented by classes) should have private members and constructors
- Implementation details should are abstracted 
- It's easier to protect business rules using a Rich Domain Model, and prevent corruption via duplication.