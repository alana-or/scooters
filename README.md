> [!CAUTION]
> 2 projetos na mesma solução Scooters e Deliveries, só para não precisar separar agora, mas num cenário real devem estar em repositórios diferentes

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

### Deliveries.Application
Contém serviços, modelos, mapeamentos, dtos, etc
- Deliveries.Domain

### Deliveries.Domain
Contém classes de domínio, enums, etc

# Docker compose
Inicializa todos os projetos e suas dependencias, cada projeto tem sua network

# Testes

> Projetos da camada de infraestrutura e interface adapters contém projetos de testes de integração

> Projetos de applicação e domínio tem testes de unidade
