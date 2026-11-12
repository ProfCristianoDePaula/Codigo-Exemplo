# Clínica Veterinária SAEP

Aplicação ASP.NET Core MVC + API REST, EF Core, SQLite local (padrão) ou SQL Server. Não existe carga de dados automática: a base começa vazia. No primeiro acesso, crie a conta inicial; em seguida cadastre tutores, pets e agendamentos pela interface.

## Requisitos

.NET SDK 10.0. Rode na pasta deste projeto:

```powershell
dotnet restore
dotnet build
dotnet run
```

Acesse a URL HTTPS mostrada no terminal. Os arquivos SQLite `clinica-vet.db` e chaves de proteção são criados localmente. A aplicação cria o schema vazio ao iniciar; isso não insere registros.

## SQL Server

Defina `Database:Provider` como `SqlServer` e forneça a conexão via User Secrets (sem gravar senha nos arquivos):

```powershell
dotnet user-secrets init
dotnet user-secrets set "Database:Provider" "SqlServer"
dotnet user-secrets set "ConnectionStrings:ClinicaConnection" "Server=SERVIDOR;Database=ClinicaVet;User Id=USUARIO;Password=SENHA;TrustServerCertificate=True;Encrypt=True"
dotnet run
```

O usuário do banco precisa ter permissão para criar as tabelas na primeira execução. A aplicação usa `EnsureCreated` sem seed. Se optar por migrations em produção, remova a criação automática e gere migrations específicas para o provedor utilizado.

## Recursos

- Primeiro acesso protegido: só permite criar usuário quando ainda não existe conta.
- Cadastro público de novos usuários pela tela de login, após a criação da primeira conta. Todos os usuários têm acesso completo aos dados e recursos da aplicação.
- Login com hash seguro, cookie com validade de 30 minutos e logout por POST.
- CRUD de tutores com busca por nome/e-mail. CPF criptografado com ASP.NET Data Protection e nunca retornado pela API.
- CRUD de pets com tutor obrigatório; CRUD de agendamentos relacionados a pet e ordenados por data.
- API autenticada: `GET /api/tutores` e `GET /api/tutores/{id}`.

A API usa o mesmo cookie de autenticação da interface. Não há dados de demonstração nem usuário/senha pré-criados.
