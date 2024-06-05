## 2 projetos na mesma solu��o Scooters e Deliveries, s� para n�o precisar separar agora, mas num cen�rio real devem estar em reposit�rios diferentes

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
Projetos da camada de infraestrutura e interface adapters cont�m projetos de testes de integra��o
Projetos de applica��o e dom�nio tem testes de unidade
