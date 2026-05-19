# BankBills API

## Descrição

A BankBills API é uma aplicação desenvolvida em C# .NET focada no processamento de extratos e analise de gastos. O projeto foi estruturado utilizando conceitos de Clean Architecture.

## Tecnologias e Ferramentas
*   **C# / .NET:** Framework principal da aplicação (versão mais recente LTS).
*   **Entity Framework Core:** ORM utilizado para mapeamento objeto-relacional.
*   **PostgreSQL:** Sistema de gerenciamento de banco de dados relacional.
* **Containerização:** Docker

## Como Executar

### Pré-requisitos

* [.NET SDK](https://dotnet.microsoft.com/download) instalado.
* PostgreSQL rodando localmente ou via Docker.

### Configuração do Banco de Dados

1. Clone o repositório.
2. Navegue até o diretório da API.
3. Configure a string de conexão do PostgreSQL no arquivo `appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Database=BankBills;Username=seu_usuario;Password=sua_senha"
}


```



```
4. Aplique as migrations do Entity Framework Core para gerar o esquema no PostgreSQL:
   ```bash
   dotnet ef database update
   

```

### Executando a API

Para rodar o projeto localmente, utilize o comando:

```bash
dotnet run

```

A documentação interativa (Swagger/OpenAPI) estará disponível na rota `/swagger`.


