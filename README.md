# 🎬 Movie Booking System

![.NET](https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![NuGet](https://img.shields.io/badge/NuGet-004880?style=for-the-badge&logo=nuget&logoColor=white)
![Clean Architecture](https://img.shields.io/badge/Clean%20Architecture-00599C?style=for-the-badge&logo=architecture&logoColor=white)

Um sistema robusto de reserva de filmes desenvolvido em C# seguindo os princípios de Clean Architecture, com foco em escalabilidade, testabilidade e manutenibilidade.

## 📋 Índice

- [Características](#-características)
- [Arquitetura](#-arquitetura)
- [Estrutura do Projeto](#-estrutura-do-projeto)
- [Tecnologias Utilizadas](#-tecnologias-utilizadas)
- [Pré-requisitos](#-pré-requisitos)
- [Instalação](#-instalação)
- [Execução](#-execução)
- [API Endpoints](#-api-endpoints)
- [Testes](#-testes)
- [Contribuição](#-contribuição)
- [Licença](#-licença)

## ✨ Características

### 🏗️ Layered Architecture
- **Clean Architecture**: Separação clara de responsabilidades em camadas
- **Domain-Driven Design**: Foco nas regras de negócio do domínio
- **SOLID Principles**: Código limpo e manutenível
- **Separation of Concerns**: Cada camada tem sua responsabilidade específica

### 🚀 Robust API
- **RESTful API**: Endpoints bem estruturados seguindo padrões REST
- **Swagger/OpenAPI**: Documentação automática da API
- **Validation**: Validação robusta de entrada de dados
- **Error Handling**: Tratamento consistente de erros
- **CORS Support**: Suporte para requisições cross-origin

### 🧪 Comprehensive Testing
- **Unit Tests**: Testes unitários para lógica de negócio
- **Integration Tests**: Testes de integração para APIs
- **Test Coverage**: Cobertura de código para garantir qualidade
- **Mocking**: Isolamento de dependências nos testes

### 👥 User Management
- **Authentication**: Sistema de autenticação seguro
- **Authorization**: Controle de acesso baseado em roles
- **JWT Tokens**: Autenticação stateless
- **User Profiles**: Gerenciamento de perfis de usuário

### 💉 Dependency Injection
- **IoC Container**: Inversão de controle nativa do .NET
- **Service Registration**: Registro automático de serviços
- **Lifetime Management**: Gerenciamento do ciclo de vida dos objetos
- **Testability**: Facilita a criação de testes unitários

### 📦 Data Transfer Objects (DTOs)
- **API Contracts**: Contratos bem definidos para APIs
- **Data Mapping**: Mapeamento automático entre entidades e DTOs
- **Validation Attributes**: Validação declarativa nos DTOs
- **Serialization**: Serialização otimizada para JSON

## 🏛️ Arquitetura

O sistema segue os princípios da **Clean Architecture**, organizando o código em camadas concêntricas:

```
┌─────────────────────────────────────┐
│             API Layer               │ ← Controllers, Middleware
├─────────────────────────────────────┤
│         Application Layer           │ ← Use Cases, Services, DTOs
├─────────────────────────────────────┤
│          Domain Layer               │ ← Entities, Value Objects, Rules
├─────────────────────────────────────┤
│       Infrastructure Layer          │ ← Data Access, External Services
└─────────────────────────────────────┘
```

### Princípios Arquiteturais

- **Dependency Rule**: Dependências apontam sempre para dentro
- **Independence**: Cada camada é independente das outras
- **Testability**: Arquitetura facilita testes automatizados
- **Flexibility**: Fácil para mudanças e extensões

## 📁 Estrutura do Projeto

```
MovieBookingSystem/
├── 🎯 MovieBookingSystem.API/              # API REST Layer
│   ├── Controllers/                        # Controllers REST
│   ├── Middleware/                         # Custom Middleware
│   ├── Extensions/                         # Service Extensions
│   └── Program.cs                          # Entry Point
│
├── 💼 MovieBookingSystem.Application/      # Business Logic Layer
│   ├── Services/                           # Application Services
│   ├── DTOs/                              # Data Transfer Objects
│   ├── Interfaces/                         # Application Interfaces
│   ├── Mappings/                          # AutoMapper Profiles
│   └── Validators/                        # FluentValidation Rules
│
├── 🏛️ MovieBookingSystem.Domain/           # Domain Layer
│   ├── Entities/                          # Domain Entities
│   ├── ValueObjects/                      # Value Objects
│   ├── Enums/                            # Domain Enumerations
│   ├── Interfaces/                        # Domain Interfaces
│   └── Specifications/                    # Business Rules
│
├── 🔧 MovieBookingSystem.Infrastructure/   # Infrastructure Layer
│   ├── Data/                             # Data Access Layer
│   │   ├── Context/                      # DbContext
│   │   ├── Repositories/                 # Repository Pattern
│   │   └── Configurations/               # Entity Configurations
│   ├── Services/                         # External Services
│   └── Extensions/                       # Infrastructure Extensions
│
└── 🧪 MovieBookingSystem.Tests/            # Testing Layer
    ├── Unit/                             # Unit Tests
    ├── Integration/                      # Integration Tests
    └── Helpers/                          # Test Helpers
```

## 🛠️ Tecnologias Utilizadas

### Core Technologies
- ![.NET 8](https://img.shields.io/badge/.NET%208-512BD4?style=flat-square&logo=dotnet&logoColor=white) **.NET 8** - Framework principal
- ![C#](https://img.shields.io/badge/C%23%2012-239120?style=flat-square&logo=c-sharp&logoColor=white) **C# 12** - Linguagem de programação
- ![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-512BD4?style=flat-square&logo=dotnet&logoColor=white) **ASP.NET Core** - Web API framework

### Database & ORM
- ![Entity Framework](https://img.shields.io/badge/Entity%20Framework%20Core-512BD4?style=flat-square&logo=microsoft&logoColor=white) **Entity Framework Core** - ORM
- ![SQL Server](https://img.shields.io/badge/SQL%20Server-CC2927?style=flat-square&logo=microsoft-sql-server&logoColor=white) **SQL Server** - Banco de dados

### Authentication & Security
- ![JWT](https://img.shields.io/badge/JWT-000000?style=flat-square&logo=json-web-tokens&logoColor=white) **JWT Bearer Tokens** - Autenticação
- ![Identity](https://img.shields.io/badge/ASP.NET%20Identity-512BD4?style=flat-square&logo=microsoft&logoColor=white) **ASP.NET Core Identity** - Gerenciamento de usuários

### Documentation & Validation
- ![Swagger](https://img.shields.io/badge/Swagger-85EA2D?style=flat-square&logo=swagger&logoColor=black) **Swagger/OpenAPI** - Documentação da API
- ![FluentValidation](https://img.shields.io/badge/FluentValidation-FF6B6B?style=flat-square) **FluentValidation** - Validação de dados

### Testing & Quality
- ![xUnit](https://img.shields.io/badge/xUnit-512BD4?style=flat-square) **xUnit** - Framework de testes
- ![Moq](https://img.shields.io/badge/Moq-FF6B6B?style=flat-square) **Moq** - Mocking framework
- ![AutoMapper](https://img.shields.io/badge/AutoMapper-BE9A2F?style=flat-square) **AutoMapper** - Object mapping

### Package Management
- ![NuGet](https://img.shields.io/badge/NuGet-004880?style=flat-square&logo=nuget&logoColor=white) **NuGet** - Gerenciador de pacotes

## 📋 Pré-requisitos

Antes de executar o projeto, certifique-se de ter instalado:

- ![.NET](https://img.shields.io/badge/.NET%208%20SDK-512BD4?style=flat-square&logo=dotnet&logoColor=white) [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- ![SQL Server](https://img.shields.io/badge/SQL%20Server-CC2927?style=flat-square&logo=microsoft-sql-server&logoColor=white) [SQL Server](https://www.microsoft.com/sql-server/sql-server-downloads) ou SQL Server LocalDB
- ![Visual Studio](https://img.shields.io/badge/Visual%20Studio%202022-5C2D91?style=flat-square&logo=visual-studio&logoColor=white) [Visual Studio 2022](https://visualstudio.microsoft.com/) ou [VS Code](https://code.visualstudio.com/)

## 🚀 Instalação

### 1. Clone o repositório
```bash
git clone https://github.com/seu-usuario/movie-booking-system.git
cd movie-booking-system
```

### 2. Restaure os pacotes NuGet
```bash
dotnet restore
```

### 3. Configure a string de conexão
Edite o arquivo `appsettings.json` na camada API:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqlLocaldb;Database=MovieBookingDb;Trusted_Connection=true;"
  }
}
```

### 4. Execute as migrations
```bash
dotnet ef database update --project MovieBookingSystem.Infrastructure --startup-project MovieBookingSystem.API
```

### 5. Compile o projeto
```bash
dotnet build
```

## ▶️ Execução

### Executar a API
```bash
cd MovieBookingSystem.API
dotnet run
```

A API estará disponível em:
- **HTTP**: `http://localhost:5000`
- **HTTPS**: `https://localhost:5001`
- **Swagger UI**: `https://localhost:5001/swagger`

### Executar com Hot Reload (Desenvolvimento)
```bash
dotnet watch run --project MovieBookingSystem.API
```

## 🔌 API Endpoints

### 🎬 Movies
```http
GET    /api/movies              # Lista todos os filmes
GET    /api/movies/{id}         # Busca filme por ID
POST   /api/movies              # Cria novo filme
PUT    /api/movies/{id}         # Atualiza filme
DELETE /api/movies/{id}         # Remove filme
```

### 🎭 Theaters
```http
GET    /api/theaters            # Lista todos os cinemas
GET    /api/theaters/{id}       # Busca cinema por ID
POST   /api/theaters            # Cria novo cinema
PUT    /api/theaters/{id}       # Atualiza cinema
DELETE /api/theaters/{id}       # Remove cinema
```

### 🎫 Bookings
```http
GET    /api/bookings            # Lista reservas do usuário
GET    /api/bookings/{id}       # Busca reserva por ID
POST   /api/bookings            # Cria nova reserva
PUT    /api/bookings/{id}       # Atualiza reserva
DELETE /api/bookings/{id}       # Cancela reserva
```

### 👤 Authentication
```http
POST   /api/auth/register       # Registra novo usuário
POST   /api/auth/login          # Autentica usuário
POST   /api/auth/refresh        # Renova token
POST   /api/auth/logout         # Logout
```

### 📊 Example Request/Response

**POST** `/api/bookings`
```json
{
  "movieId": 1,
  "theaterId": 1,
  "showTime": "2024-06-10T19:30:00",
  "seats": ["A1", "A2"],
  "totalPrice": 25.00
}
```

**Response** `201 Created`
```json
{
  "id": 123,
  "movieTitle": "Exemplo de Filme",
  "theaterName": "Cinema Central",
  "showTime": "2024-06-10T19:30:00",
  "seats": ["A1", "A2"],
  "totalPrice": 25.00,
  "bookingDate": "2024-06-05T14:30:00",
  "status": "Confirmed"
}
```

## 🧪 Testes

### Executar todos os testes
```bash
dotnet test
```

### Executar testes com cobertura
```bash
dotnet test --collect:"XPlat Code Coverage"
```

### Executar testes específicos
```bash
# Testes unitários
dotnet test MovieBookingSystem.Tests/Unit/

# Testes de integração
dotnet test MovieBookingSystem.Tests/Integration/
```

### Gerar relatório de cobertura
```bash
dotnet tool install -g dotnet-reportgenerator-globaltool
reportgenerator -reports:"**/coverage.cobertura.xml" -targetdir:"coveragereport" -reporttypes:Html
```

## 📈 Estrutura de Testes

```
MovieBookingSystem.Tests/
├── Unit/
│   ├── Domain/                 # Testes das entidades de domínio
│   ├── Application/            # Testes dos serviços de aplicação
│   └── API/                    # Testes dos controllers
├── Integration/
│   ├── API/                    # Testes de integração da API
│   └── Infrastructure/         # Testes de acesso a dados
└── Helpers/
    ├── TestFixtures/           # Fixtures para testes
    └── MockData/               # Dados de teste
```

## 🤝 Contribuição

Contribuições são sempre bem-vindas! Para contribuir:

1. **Fork** o projeto
2. Crie uma **branch** para sua feature (`git checkout -b feature/MinhaFeature`)
3. **Commit** suas mudanças (`git commit -m 'Add: Nova funcionalidade'`)
4. **Push** para a branch (`git push origin feature/MinhaFeature`)
5. Abra um **Pull Request**

### Padrões de Commit
```
feat: nova funcionalidade
fix: correção de bug
docs: documentação
style: formatação
refactor: refatoração
test: testes
chore: tarefas de manutenção
```

## 📄 Licença

Este projeto está licenciado sob a **MIT License** - veja o arquivo [LICENSE](LICENSE) para detalhes.

---

## 📞 Contato

**Desenvolvedor**: Romeu Garcia  
**Email**: romeuJgarcia@hotmail.com  
**LinkedIn**: [Seu LinkedIn](https://www.linkedin.com/in/romeugarcia/)  
**GitHub**: [Seu GitHub](https://github.com/Romeujgarcia)

---

<div align="center">

**⭐ Se este projeto foi útil para você, considere dar uma estrela!**

Made with ❤️ and C#

</div>


https://roadmap.sh/projects/movie-reservation-system
