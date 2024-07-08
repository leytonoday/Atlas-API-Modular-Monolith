# Legal Lighthouse | Atlas 

A SaaS product for legal document services using AI. The codename for the project is Atlas.

## Note
The following repositories were used as to learn many of the design patterns implemented in this codebase:
- https://github.com/PeterKneale/modular_monolith_saas
- https://github.com/kgrzybek/modular-monolith-with-ddd

## Architecture 
### Modular Monolith
- 3 modules (sub-domains) that are isolated and act as DDD bounded contexts:
    - Users (generic sub-domain): contains the authentication, authorization, and user management features of Atlas
    - Plans (supporting sub-domain): contains the SaaS functionality of Atlas with Stripe integration.
    - Law (core sub-domain): contains the core busines logic for the legal document services with AI.

<div align="center">
  <img src="https://github.com/leytonoday/Atlas-API-Modular-Monolith/assets/36010516/0b2b1bd8-f0e8-401c-ba4a-89a89d6a8b46" />
</div>

- Modules communicate using EDA (Event Driven Architecture). The inbox and outbox patterns are used to increase the reliability of messaging between the modules, with considerations for event idempotency. This, in theory, allows modules in Atlas to communicate with an exactly-once-guarantee. Each module exposes a public IntegrationEvents assembly that stores events that other modules can subscribe to, allowing for loosely coupled asynchronous communication between modules.

<div align="center">
  <img src="https://github.com/leytonoday/Atlas-API-Modular-Monolith/assets/36010516/214be6fb-9206-4a5d-9bfd-04ba1bc3f6a8" />
</div>

### Clean Architecture
- Each module within Atlas follows Clean Architecture and is comprised of 3 assemblies: Application, Domain, and Infrastructure.
- The Presentation layer of the Clean Architecture doesn’t have its own assembly, because for the sake of simplicity, these have been consolidated into the Atlas.Web assembly for all modules.
- The domain assembly hosts the business entities and their respective repositories, as well as core business logic that is common across all applications within an organisation for this business entity. The application assembly hosts the CQRS code, where all application specific business logic lies, as well as where orchestration between business entities and external systems takes place. The infrastructure assembly provides concrete implementations for services that interact with external services (where the interfaces are defined in the application layer). This might include services for the database (concretions of repository interfaces defined in the domain layer), or services for AI, such as OpenAI’s GPT-4.

## API Design Patterns and Techniques
- Façade Pattern
- Mediator Pattern
- CQRS
- Composition Root Pattern
- Inbox and Outbox Patterns
- Repository Pattern
- Unit of Work Pattern
- DDD (Domain Driven Design)
- Decorator Pattern
