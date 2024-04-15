# 13. In-Memory Event Bus

## Status

Accepted.

## Context

Each module has seperate dependencies, so we need to ensure that modules can't access eachothers dependencies.

## Possible solutions

### 1. A single IoC (Inversion of Control) Container for all Modules

Each module will have access to the same IoC contianer. Much simpler to implement, and we can configure all dependencies in one location. However, modules are now inherently coupled. Also, if someone accidently adds a direct reference between two modules, module A could get access to module B's dependencies from the IoC container.

### 2. One IoC (Inversion of Control) Container per Module

Each module will have it's own IOC container. More complex to setup. But, less coupling. Even if two modules reference eachother, Module A could never access Module B's dependencies.

## Decision

Solution 2, One IoC (Inversion of Control) Container per Module.

Less coupling between modules, and increased module autonomy. 

## Consequences
- Implement some system to setup seperate IoC containers for each module.
- Modules are much more autonomous.
- Less coupling.