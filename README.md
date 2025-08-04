# Reseller Solution

## Visão Geral

A solução Reseller é composta por múltiplos serviços voltados para a gestão de revendas e pedidos, utilizando arquitetura moderna baseada em microserviços, mensageria e boas práticas de desenvolvimento .NET.

## Arquitetura

- **Microserviços**: Separação clara entre contexto de revendas (Reseller.API) e pedidos (OrderService).
- **Mensageria**: Integração via RabbitMQ para comunicação assíncrona entre serviços. (Em andamento)
- **Banco de Dados**: Utilização de Entity Framework Core com SQLite para persistência.
- **Padrão Clean Architecture**: Separação de responsabilidades em camadas (Domain, Application, Infrastructure, API).
- **Validações**: Uso de FluentValidation para regras de negócio e validação de dados.
- **Documentação**: OpenAPI/Swagger disponível para os serviços HTTP.

## Tecnologias Utilizadas

- .NET 8/9
- ASP.NET Core Web API
- Entity Framework Core
- MassTransit (RabbitMQ)
- Carter (endpoints minimalistas)
- FluentValidation
- Moq, xUnit, FluentAssertions (testes)
- Docker e Docker Compose

## Funcionalidades

### Reseller.API
- Cadastro de revendas com múltiplos contatos, endereços e telefones.
- Consulta de revenda por CNPJ.
- Validação de dados de entrada.
- Documentação automática via Swagger.

### OrderService
- Criação de pedidos vinculados a uma revenda.
- Envio de pedidos para processamento externo (mínimo de 1000 itens).
- Validação de cliente e itens do pedido.
- Integração com serviço de revenda para validação de revenda.
- Comunicação assíncrona via RabbitMQ.

## Como Executar

### Pré-requisitos
- Docker e Docker Compose instalados
- .NET 8/9 SDK para desenvolvimento local

### Subindo com Docker Compose

1. Certifique-se de que os arquivos `Dockerfile` existem para Reseller.API e OrderService.
2. Execute:

```sh
docker compose up --build
```

- Acesse a API de revendas em: http://localhost:5000/swagger
- Acesse a API de pedidos em: http://localhost:5001/swagger
- Acesse o RabbitMQ Management em: http://localhost:15672 (usuário/senha: guest/guest)

### Testes

Para rodar os testes automatizados:

```sh
dotnet test OrderService.Tests/OrderService.Tests.csproj
```

## Estrutura dos Projetos

- **Reseller.API**: API de revendas
- **Reseller.Domain**: Entidades e regras de domínio
- **Reseller.Infrastructure**: Persistência e injeção de dependências
- **Reseller.Service**: Lógica e regras de negócio
- **Reseller.Test**: Testes automatizados do Reseller
- 
- **OrderService**: API de pedidos
- **OrderService.Tests**: Testes automatizados do OrderService
  **Shared**: Utilitários e contratos compartilhados
- 
## Padrões e Boas Práticas

- Injeção de dependências em todos os serviços
- Validação de dados robusta
- Testes unitários cobrindo cenários de sucesso e falha
- Uso de DTOs para entrada e saída de dados
- Separação de responsabilidades e baixo acoplamento

## Observações

- Para integração real entre serviços, configure as URLs e conexões conforme necessário nos arquivos de configuração.
- O projeto está pronto para expansão, podendo receber novos serviços e integrações facilmente.
