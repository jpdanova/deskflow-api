# 🎧 DeskFlow API — Gestão de Chamados e Helpdesk de TI

## 🎯 Sobre o Projeto
A **DeskFlow API** é uma Web API RESTful construída em .NET 10 com Entity Framework Core e SQL Server. Ela centraliza o fluxo de suporte técnico de TI: cadastro de categorias de incidentes, abertura de chamados, ciclo de vida do atendimento (Aberto → EmAndamento → Fechado), histórico de interações e consultas com filtros dinâmicos.

Projeto final do Módulo 01 do curso Desenvolvedor Back End .NET.
**Autor:** João Pedro da Nova

## 🛠️ Tecnologias Utilizadas
- .NET 10 / ASP.NET Core Web API (Controllers)
- Entity Framework Core 10 com Migrations
- SQL Server (Express, LocalDB ou Docker)
- OpenAPI + Swagger UI (interface em `http://localhost:5173/swagger` no ambiente de desenvolvimento)
- Git e GitHub (commits semânticos e branches por funcionalidade)

## 🚀 Como Executar a Aplicação

### Pré-requisitos
- .NET SDK 10 (ou superior)
- SQL Server em execução (Express, LocalDB ou Docker)
- Ferramenta do EF Core:
```
  dotnet tool install --global dotnet-ef
```

### Passo a Passo
1. Clone o repositório e entre na pasta da API:
```
   git clone https://github.com/jpdanova/deskflow-api.git
   cd deskflow-api/src/DeskFlow.API
```

2. Configure a connection string em `src/DeskFlow.API/appsettings.json` com o nome da sua instância do SQL Server. Exemplo usado no desenvolvimento (SQL Server Express, instância `SQLEXPRESS01`, autenticação do Windows):
```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=localhost\\SQLEXPRESS01;Database=DeskFlowDb;Trusted_Connection=True;TrustServerCertificate=True;"
   }
```
   Outras opções de servidor:
   - SQL Server Express padrão: `Server=localhost\\SQLEXPRESS;`
   - LocalDB: `Server=(localdb)\\MSSQLLocalDB;`
   - Docker ou login com usuário e senha: `Server=localhost,1433;Database=DeskFlowDb;User Id=sa;Password=SUA_SENHA;TrustServerCertificate=True;`

3. Crie o banco de dados (escolha uma opção):
   - **Opção A, via migrations (recomendada):**
```
     dotnet ef database update
```
     Cria o banco `DeskFlowDb` e as tabelas `Categorias`, `Chamados` e `Interacoes`.
   - **Opção B, via script SQL:** crie um banco vazio chamado `DeskFlowDb` no SQL Server e execute nele o arquivo `database/script.sql`.

4. Execute a API:
```
   dotnet run
```
   A API sobe em `http://localhost:5173`.
   Abra `http://localhost:5173/swagger` no navegador para testar os endpoints pela interface do Swagger.

> Para recompilar durante o desenvolvimento, pare a API (`Ctrl+C`) antes de rodar `dotnet build`, pois o Windows bloqueia o executável em uso.

## 🧱 Arquitetura em Camadas
Fluxo: **Controller → Service → Repository → AppDbContext**

```
deskflow-api/
├── database/
│   └── script.sql              → Script SQL gerado pelas migrations
└── src/DeskFlow.API/
    ├── Controllers/            → Rotas HTTP e status codes
    ├── Services/               → Regras de negócio e validações
    ├── Repositories/           → Acesso a dados (EF Core)
    ├── Models/
    │   ├── Entities/           → Categoria, Chamado, Interacao
    │   └── DTOs/               → Objetos de entrada e saída
    ├── Middlewares/            → ExceptionHandlingMiddleware
    ├── Exceptions/             → NotFoundException, BusinessException
    ├── Data/                   → AppDbContext
    ├── Migrations/             → Histórico do banco (EF Core)
    └── Program.cs              → Pipeline e injeção de dependência
```

- **Controllers:** recebem a requisição, delegam ao Service e devolvem o status code.
- **Services:** concentram as regras de negócio (transições de status, validações, regras de exclusão).
- **Repositories:** únicos a falar com o banco, via `AppDbContext`.
- **Injeção de dependência:** repositórios e services registrados com `AddScoped` no `Program.cs`.

## 🧠 Ciclo de Vida do Chamado
- **Aberto:** chamado registrado pelo solicitante. Status `Aberto` e `DataAbertura` são atribuídos automaticamente.
- **EmAndamento:** suporte em atendimento. Só é possível iniciar chamados `Aberto`.
- **Fechado:** encerrado com o texto de solução e a data de fechamento. Só é possível encerrar chamados `EmAndamento`.

Chamados `Fechado` não aceitam novas interações. Todas as transições são validadas na camada de Service.

Valores aceitos:
- `prioridade`: `Baixa`, `Media`, `Alta`
- `status`: `Aberto`, `EmAndamento`, `Fechado`

## 📡 Endpoints

| Método | Rota | Descrição | Retornos |
|---|---|---|---|
| POST | `/api/categorias` | Cadastra categoria | 201, 400 |
| GET | `/api/categorias` | Lista categorias | 200 |
| GET | `/api/categorias/{id}` | Busca categoria por id | 200, 404 |
| PUT | `/api/categorias/{id}` | Atualiza o nome | 200, 400, 404 |
| DELETE | `/api/categorias/{id}` | Remove a categoria (bloqueia se houver chamados vinculados) | 204, 400, 404 |
| POST | `/api/chamados` | Abre chamado | 201, 400 |
| GET | `/api/chamados` | Lista com filtros `status`, `prioridade` e `categoriaId` | 200 |
| GET | `/api/chamados/{id}` | Detalhes com categoria e lista de interações | 200, 404 |
| PATCH | `/api/chamados/{id}/iniciar` | Aberto → EmAndamento | 200, 400, 404 |
| PATCH | `/api/chamados/{id}/encerrar` | EmAndamento → Fechado (exige `solucao`) | 200, 400, 404 |
| POST | `/api/chamados/{id}/interacoes` | Adiciona comentário (se o chamado não estiver Fechado) | 201, 400, 404 |

As rotas de mudança de status usam **PATCH**, por alterarem apenas parte do recurso (uso semântico dos verbos HTTP, conforme o RNF02).

Os filtros podem ser combinados, por exemplo:
```
GET /api/chamados?status=Aberto&prioridade=Alta&categoriaId=1
```

### Exemplos de uso (PowerShell)
```powershell
$api = "http://localhost:5173/api"

# Categoria
Invoke-RestMethod -Method Post -Uri "$api/categorias" -ContentType "application/json" -Body '{"nome":"Hardware"}'

# Abrir chamado
$body = @{ titulo="Computador nao liga"; descricao="Tela preta ao ligar"; prioridade="Alta"; solicitanteNome="Maria"; categoriaId=1 } | ConvertTo-Json
Invoke-RestMethod -Method Post -Uri "$api/chamados" -ContentType "application/json" -Body $body

# Ciclo de vida
Invoke-RestMethod -Method Patch -Uri "$api/chamados/1/iniciar"
Invoke-RestMethod -Method Post  -Uri "$api/chamados/1/interacoes" -ContentType "application/json" -Body '{"autor":"Suporte","mensagem":"Analisando o equipamento"}'
Invoke-RestMethod -Method Patch -Uri "$api/chamados/1/encerrar" -ContentType "application/json" -Body '{"solucao":"Fonte de alimentacao trocada"}'

# Detalhes e filtros
Invoke-RestMethod -Uri "$api/chamados/1" | ConvertTo-Json -Depth 5
Invoke-RestMethod -Uri "${api}/chamados?status=Fechado&prioridade=Alta"
```

## ⚠️ Tratamento de Erros
O `ExceptionHandlingMiddleware` captura as exceções não tratadas e devolve JSON padronizado, sem expor o stack trace:

```json
{ "status": 404, "mensagem": "Chamado 999 não encontrado." }
```

- `400 Bad Request`: validação de entrada ou regra de negócio violada (ex.: iniciar chamado já Fechado, comentar em chamado Fechado, excluir categoria com chamados)
- `404 Not Found`: categoria ou chamado inexistente
- `500 Internal Server Error`: erro interno, com mensagem genérica (o detalhe fica apenas no log do servidor)

## 🌿 Versionamento
O desenvolvimento seguiu o fluxo com branches por funcionalidade, mescladas na `develop` e, ao final, na `main`:

- `feature/entidades-dbcontext`: entidades, `AppDbContext`, migration inicial e script SQL
- `feature/middleware-erros`: exceções de negócio e `ExceptionHandlingMiddleware`
- `feature/categorias`: CRUD de categorias
- `feature/chamados`: abertura e detalhes de chamados
- `feature/ciclo-de-vida`: iniciar e encerrar chamado
- `feature/interacoes-filtros`: interações e listagem com filtros
- `feature/swagger`: documentação interativa com Swagger UI
- `docs/readme`: documentação

Os commits seguem o padrão semântico (`feat:`, `fix:`, `docs:`, `chore:`, `merge:`).

## 🔮 Melhorias Futuras
- Autenticação e autorização com ASP.NET Core Identity e tokens JWT
- Paginação e ordenação na listagem de chamados
- Testes automatizados (unitários e de integração)
- Atribuição de chamados a técnicos responsáveis

## 🎥 Vídeo de Apresentação
[Clique aqui para assistir ao vídeo de demonstração do projeto](https://www.youtube.com/watch?v=_pQAiE97Jy8)