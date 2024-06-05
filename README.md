> [!CAUTION]
> 2 projetos na mesma solução Scooters e Deliveries, só para não precisar separar agora, mas num cenário real devem estar em repositórios diferentes

[<img src="https://github.com/alana-or/scooters/assets/19328250/e5edfb46-a7f6-4524-9b8e-6a5deb1a0634" width="500"/>](https://github.com/alana-or/scooters/assets/19328250/e5edfb46-a7f6-4524-9b8e-6a5deb1a0634)

# Scooters
Projeto usado para adicionar mais scooters ao sistema

### Scooters.Api
Contém implementação de chamadas HTTP/S
- Scooters.Data
- Scooters.Application
- AutoMapper
- FluentValidation
- Rabbitmq.Client

### Scooters.Consumer
Comtém implementação do Worker que ouve tópicos do Rabbitmq
- Scooters.Application
- Scooters.Data
- Rabbitmq.Client

### Scooters.Data
Contém implementação de repositórios, migrações e builders
- Scooters.Domain
- AutoBogus
- EntityFrameworkCore
- Npgsql.EntityFrameworkCore.PostgreSQL

```dotnet ef migrations add InitialCreate -p Scooters.Data -c ScootersContext```

# Deliveries 
Projeto usado para adicionar Deliveries Person e Rentals

### Deliveries.Api
Contém implementação de chamadas HTTP/S
- Deliveries.Application
- Deliveries.Data
- AutoMapper
- FluentValidation

### Deliveries.Data
Contém implementação de repositórios, migrações e builders
- Deliveries.Application
- AutoBogus
- EntityFrameworkCore
- Npgsql.EntityFrameworkCore.PostgreSQL

```dotnet ef migrations add InitialCreate -p Deliveries.Data -c DeliveriesContext```

### Deliveries.Application
Contém serviços, modelos, mapeamentos, dtos, etc
- Deliveries.Domain

### Deliveries.Domain
Contém classes de domínio, enums, etc

# Docker compose
Inicializa todos os projetos e suas dependencias, cada projeto tem sua network

# Testes
Cada projeto de testes de integração sobe seu próprio container Postgres para testes e roda as migrações antes de executar os testes
- FluentAssertions
- Moq
- NUnit
- Testcontainers
- Testcontainers.PostgreSql
  
> Projetos da camada de infraestrutura e interface adapters contém projetos de testes de integração

> Projetos de applicação e domínio tem testes de unidade
