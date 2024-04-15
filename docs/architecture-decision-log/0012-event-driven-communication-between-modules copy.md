# 12. Event Driven Communication Between Modules

## Status

Accepted.

## Context

Despite the fact that modules should be autonomous, they still need to communicate in *some* way.

## Possible solutions

### 1. Synchronously, via direct method calling

Each moodule exposes a either it's own API or an interface which can be directly accessed by other modules.

This is easier to implement, with less cognitive load for engineers. However, the this makes scaling out into microservices more difficult, as the tight-coupling will result in a distrubuted monolith rather than a microservice based architecture.

### 2. Asynchronous, via events

Each module has a set of integration events that can be accessed by other modules. They can "subscribe" to these integration events, and when the module that owns those 
integration events fires them, the subscribing module can react accordingly. Modules will employ a "fire and forget" approach to emitting integration events.

This way, our modules are:
- not directly coupled to eachother
- more autonomous 

## Decision

Solution 2. Asynchronous, via events

We want autonomy and losse coupling between our modules. Otherwise, we might as well use a classic monolithic structure without modules. Synchronous communication should be a rare occurance.

## Consequences
- Modules are more autonomous
- Scaling into a microservice architecture is easier
- Modules will need to be able to publish and subscribe to events from other modules.
