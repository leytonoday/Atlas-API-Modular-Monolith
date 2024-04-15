# 2. Single REST API Entry Point

## Status

Accepted

## Context

The modules of the system need to be exposed to the outside world in some way. 

## Possible Solutions

1. Create a single ASP.NET Core Web application that hosts all endpoints (the presentation layers). This single project will reference all modules and communicate with them directly.

2. Create an ASP.NET Core Web application for each module. 

## Decision

Solution 1.

There's not much added value in having separate API projects on a per-module basis. From a client persoective, solution 1 and 2 are the exact same (it makes no difference from the front-end). For the sake of simplicity, solution 1 has been selected. 

## Consequences

- A single API project to expose the modules to the outside world.
- API configuration and deployment is significantly easier.
- Decreased build time.
- Each controller must communicate with the associated module.