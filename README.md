# Reseller Solution

## Vis�o Geral

A solu��o Reseller � composta por m�ltiplos servi�os voltados para a gest�o de revendas e pedidos, utilizando arquitetura moderna baseada em microservi�os, mensageria e boas pr�ticas de desenvolvimento .NET.

## Arquitetura

- **Microservi�os**: Separa��o clara entre contexto de revendas (Reseller.API) e pedidos (OrderService).
- **Mensageria**: Integra��o via RabbitMQ para comunica��o ass�ncrona entre servi�os. (Em andamento)
- **Banco de Dados**: Utiliza��o de Entity Framework Core com SQLite para persist�ncia.
- **Padr�o Clean Architecture**: Separa��o de responsabilidades em camadas (Domain, Application, Infrastructure, API).
- **Valida��es**: Uso de FluentValidation para regras de neg�cio e valida��o de dados.
- **Documenta��o**: OpenAPI/Swagger dispon�vel para os servi�os HTTP.

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
- Cadastro de revendas com m�ltiplos contatos, endere�os e telefones.
- Consulta de revenda por CNPJ.
- Valida��o de dados de entrada.
- Documenta��o autom�tica via Swagger.

### OrderService
- Cria��o de pedidos vinculados a uma revenda.
- Envio de pedidos para processamento externo (m�nimo de 1000 itens).
- Valida��o de cliente e itens do pedido.
- Integra��o com servi�o de revenda para valida��o de revenda.
- Comunica��o ass�ncrona via RabbitMQ.

## Como Executar

### Pr�-requisitos
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
- Acesse o RabbitMQ Management em: http://localhost:15672 (usu�rio/senha: guest/guest)

### Testes

Para rodar os testes automatizados:

```sh
dotnet test OrderService.Tests/OrderService.Tests.csproj
```

## Estrutura dos Projetos

- **Reseller.API**: API de revendas
- **Reseller.Domain**: Entidades e regras de dom�nio
- **Reseller.Infrastructure**: Persist�ncia e inje��o de depend�ncias
- **Reseller.Service**: L�gica e regras de neg�cio
- **Reseller.Test**: Testes automatizados do Reseller
- 
- **OrderService**: API de pedidos
- **OrderService.Tests**: Testes automatizados do OrderService
  **Shared**: Utilit�rios e contratos compartilhados
- 
## Padr�es e Boas Pr�ticas

- Inje��o de depend�ncias em todos os servi�os
- Valida��o de dados robusta
- Testes unit�rios cobrindo cen�rios de sucesso e falha
- Uso de DTOs para entrada e sa�da de dados
- Separa��o de responsabilidades e baixo acoplamento

## Observa��es

- Para integra��o real entre servi�os, configure as URLs e conex�es conforme necess�rio nos arquivos de configura��o.
- O projeto est� pronto para expans�o, podendo receber novos servi�os e integra��es facilmente.
