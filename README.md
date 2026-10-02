# Banking

Projeto de estudo desenvolvido em .NET com o objetivo de explorar, na prática, conceitos de arquitetura de software, modelagem de domínio e padrões aplicados a sistemas back-end.

A aplicação começou como uma API CRUD simples para operações bancárias e foi evoluindo progressivamente para uma arquitetura com CQRS, MediatR, Domain Events, Event Sourcing, Projections e Snapshots.

## Objetivo

O principal objetivo do projeto é entender não apenas como implementar padrões arquiteturais, mas também quais problemas eles resolvem, seus trade-offs e em quais cenários sua utilização pode ou não fazer sentido.

## Tecnologias e conceitos utilizados

- .NET
- ASP.NET Core
- Entity Framework Core
- SQLite
- MediatR
- FluentValidation
- CQRS
- Clean Architecture
- Domain-Driven Design
- SOLID
- Repository Pattern
- Domain Events
- Event Sourcing
- Optimistic Concurrency
- Projections
- Read Models
- Materialized Views
- Event Replay
- Snapshots
- Global Exception Handling
- Pipeline Behaviors

## Evolução da arquitetura

O projeto foi construído de forma incremental.

A primeira versão seguia um fluxo tradicional:

```text
Controller
    ↓
Entity Framework Core
    ↓
SQLite
