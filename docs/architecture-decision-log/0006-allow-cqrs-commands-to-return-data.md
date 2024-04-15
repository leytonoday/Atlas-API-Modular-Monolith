# 6. Allow CQRS Commands To Return Data

## Status

Accepted

## Context

In theory, a Command in CQRS should not return any data, that is the job of a Query. However, sometimes it's reasonable to want to return some data after a Command. For example, after the creation of some resource, you might
return the Id of the newly created resource.

## Decision

Commands are allowed to return data after processing. 

## Consequences

- There's two different types of commands and command handlers: ones that return void, and ones that return some data. 
- Added complexity as a result of the extra command types.
- We can return the ID of a newly created resource without having to make a second API call to retrieve the ID.