# 📚 Biblioteca API & Dashboard Admin

<p align="center">
  <img src="https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=.net&logoColor=white" alt=".NET 8" />
  <img src="https://img.shields.io/badge/MySQL-4479A1?style=for-the-badge&logo=mysql&logoColor=white" alt="MySQL" />
  <img src="https://img.shields.io/badge/JWT-black?style=for-the-badge&logo=JSON%20web%20tokens" alt="JWT" />
  <img src="https://img.shields.io/badge/JavaScript-F7DF1E?style=for-the-badge&logo=javascript&logoColor=black" alt="JavaScript" />
  <img src="https://img.shields.io/badge/CSS3-1572B6?style=for-the-badge&logo=css3&logoColor=white" alt="CSS3" />
</p>

## 📖 Sobre o Projeto

A **Biblioteca API** é um ecossistema completo para o gerenciamento de sistemas bibliotecários e controle de leitura pessoal. Desenvolvida com as tecnologias mais modernas do ecossistema .NET, a solução conta com uma API robusta e um **Dashboard Administrativo SPA** (Single Page Application) elegante e performático.

Este projeto foi construído como um laboratório avançado de engenharia de software, priorizando a separação de responsabilidades, segurança via JWT e uma experiência de usuário (UX) fluida.

---

## 🗺️ Sumário

- [🚀 Visão Geral](#-visão-geral)
- [🧠 Funcionalidades](#-funcionalidades)
- [🎨 Dashboard Admin (Front-end)](#-dashboard-admin-front-end)
- [🛠️ Tecnologias](#️-tecnologias)
- [📦 Arquitetura do Sistema](#-arquitetura-do-sistema)
- [🔐 Segurança](#-segurança)
- [⚙️ Como Executar](#️-como-executar)
- [📑 Documentação](#-documentação)
- [👨‍💻 Autor](#-autor)

---

## 🚀 Visão Geral

O sistema gerencia cinco pilares principais de uma biblioteca:

- **Livros**: Cadastro completo com vínculo a autores, editoras e categorias.
- **Autores**: Gestão biográfica e nacionalidade.
- **Editoras**: Controle de fundação e links oficiais.
- **Categorias**: Organização taxonômica do acervo.
- **Usuários & Perfis**: Sistema de autenticação com controle de acesso baseado em Roles (RBAC).

---

## 🧠 Funcionalidades

### ✅ V1 — Gestão Administrativa

- **CRUD Completo**: Todas as entidades (Livros, Autores, etc) possuem operações de criação, leitura, atualização e exclusão.
- **Paginação Avançada**: Listagens otimizadas com `X.PagedList`.
- **Versionamento**: Suporte a múltiplas versões da API simultaneamente (V1 e V2).
- **Filtros Dinâmicos**: Pesquisa de livros por nome, autor, editora, ano ou categoria.

### 🚧 V2 — Experiência do Usuário (Em progresso)

- **Status de Leitura**: Controle pessoal (Lido, Lendo, A Ler).
- **Estante Virtual**: Associação direta de livros ao perfil do usuário logado.
- **Refresh Token**: Renovação segura de sessões.

---

## 🎨 Dashboard Admin (Front-end)

O sistema acompanha um **Dashboard SPA** moderno, desenvolvido em **Vanilla JS**, focado em performance máxima e design minimalista (Dark Mode com tons verdes pastéis).

**Destaques do Front-end:**

- **Autenticação Segura**: Fluxo de login integrado com armazenamento de JWT no `localStorage`.
- **Interceptor de Requisições**: Injeção automática de tokens em todas as chamadas à API.
- **CRUD Via Modals**: Operações de edição e criação realizadas em janelas modais, sem recarregar a página.
- **Dashboard Dinâmico**: Cards informativos com contagem em tempo real de todo o acervo.
- **UX Reativa**: Navegação instantânea entre seções e feedbacks visuais de sucesso/erro.

---

## 🛠️ Tecnologias

### Back-end

- **Linguagem**: C# (.NET 8)
- **Framework API**: ASP.NET Core Web API
- **ORM**: Entity Framework Core
- **Banco de Dados**: MySQL (via Pomelo)
- **Autenticação**: Identity Framework + JWT (JSON Web Token)
- **Mapeamento**: AutoMapper
- **Documentação**: Swagger / OpenAPI
- **Versionamento**: API Versioning

### Front-end

- **Core**: HTML5 Semantic Elements
- **Logic**: Vanilla JavaScript (ES6+)
- **Styling**: CSS3 Modern (Variáveis, Flexbox, Grid, Glassmorphism)
- **Icons**: Unicode/Emoji UI

---

## 📦 Arquitetura do Sistema

O projeto utiliza uma arquitetura organizada em camadas, inspirada em princípios de **Clean Architecture**, garantindo testabilidade e manutenção simplificada:

```text
BibliotecaAPI/
├── back-end/
│   ├── Context/       # Contexto do Entity Framework
│   ├── Controllers/   # Endpoints da API (V1 e V2)
│   ├── Services/      # Use Cases e Lógica de Negócio
│   ├── Repositories/  # Camada de Acesso a Dados
│   ├── Models/        # Entidades do Domínio
│   ├── DTOs/          # Objetos de Transferência de Dados
│   └── Pagination/    # Lógica de filtros e páginas
├── front-end/
│   ├── css/           # Estilos modernos e Dark Mode
│   ├── js/            # Core logic, API client e App logic
│   └── index.html     # SPA Entry point
```

---

## 🔐 Segurança

- **JWT Authentication**: As rotas são protegidas e exigem um token válido emitido no login.
- **RBAC (Role Based Access Control)**:
  - `AdminsOnly`: Acesso total (CRUD, Gestão de Usuários).
  - `AdminsAndUsers`: Acesso de leitura e funcionalidades de estante virtual.
- **CORS Configurado**: A API está preparada para servir o front-end de forma segura em diferentes origens.

---

## ⚙️ Como Executar

### Pré-requisitos

- .NET 8 SDK
- MySQL Server

### Passo a Passo

1. **Clone o repositório:**

   ```bash
   git clone https://github.com/seu-usuario/BibliotecaAPI.git
   cd BibliotecaAPI
   ```

2. **Configure o Banco de Dados:**
   Ajuste a `ConnectionString` no arquivo `appsettings.json` dentro da pasta `back-end`.

3. **Inicie o Back-end:**

   ```bash
   cd back-end
   dotnet ef database update
   dotnet run
   ```

4. **Acesse o Front-end:**
   Abra o arquivo `front-end/index.html` em qualquer navegador moderno. (Recomendado usar a extensão _Live Server_ no VS Code).

---

## 📑 Documentação

A documentação interativa está disponível via Swagger. Com a API rodando, acesse:
`https://localhost:{porta}/swagger`

---

## 👨‍💻 Autor

Desenvolvido por **Gabriel Lentine**.  
Projeto focado em demonstrar competência técnica em desenvolvimento Full-Stack com o ecossistema Microsoft.

📅 **Período**: Nov/2025 ~ Mai/2026
