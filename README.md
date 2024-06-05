## 2 projetos na mesma solução Scooters e Deliveries, só para não precisar separar agora, mas num cenário real devem estar em repositórios diferentes

# Scooters
Projeto usado para adicionar mais scooters ao sistema

## Scooters.Api
AutoMapper
FluentValidation
Rabbitmq.Client

## Scooters.Consumer
Rabbitmq.Client

## Scooters.Data
AutoBogus
EntityFrameworkCore
Npgsql.EntityFrameworkCore.PostgreSQL

# Deliveries 
Projeto usado para adicionar Deliveries Person e Rentals

## Deliveries.Api
AutoMapper
FluentValidation

## Deliveries.Data
AutoBogus
EntityFrameworkCore
Npgsql.EntityFrameworkCore.PostgreSQL

# Docker compose
Inicializa todos os projetos e suas dependencias, cada projeto tem sua network

# Testes
Projetos da camada de infraestrutura e interface adapters contém projetos de testes de integração
Projetos de applicação e domínio tem testes de unidade
