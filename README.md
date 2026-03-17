# 📚 Biblioteca API
API RESTful para gerenciamento de um sistema bibliotecário, desenvolvida com foco em boas práticas de arquitetura, versionamento e segurança.
Projeto voltado para consolidar conhecimentos em desenvolvimento backend com .NET, incluindo autenticação, persistência de dados e organização de domínio e priorizando a construção de uma boa API.

---

## 🚀 Visão Geral
A Biblioteca API permite o gerenciamento completo de um ecossistema de livros, incluindo:
- 📖 Livros  
- ✍️ Autores  
- 🏢 Editoras  
- 🏷️ Categorias  
- 👤 Usuários  
- 🔐 Perfis e permissões  

Além disso, na versão mais recente, o sistema evolui para um controle de leitura pessoal, permitindo que usuários acompanhem seus livros.

---

## 🧠 Funcionalidades
### ✅ V1 — Base do Sistema

- CRUD completo de:
  - Autores
  - Editoras
  - Categorias
  - Livros
- Gerenciamento de usuários
- Perfis de usuário (roles)
- Relacionamentos entre entidades
- Documentação automática com Swagger

---

### 🚧 V2 — Evolução do Sistema
- 📚 Marcação de status de leitura:
  - Lido
  - Lendo
  - A Ler (padrão)
- Associação de livros a usuários
- Expansão do domínio para experiência personalizada
- Novas funcionalidades em desenvolvimento

---

## 🛠️ Tecnologias Utilizadas
- .NET 8  
- ASP.NET Core Web API  
- Entity Framework Core  
- MySQL (via Pomelo)  
- ASP.NET Identity  
- JWT (Json Web Token) para autenticação  
- AutoMapper  
- Swagger / OpenAPI  
- API Versioning  
- X.PagedList (paginação)  

---

## 🔐 Autenticação e Segurança
- Autenticação baseada em JWT  
- Controle de acesso com roles  
- Integração com ASP.NET Identity  

---

## 📦 Arquitetura e Padrões
- Separação de responsabilidades (Controllers, Services, Data)  
- Uso de DTOs para transporte de dados  
- Mapeamento com AutoMapper  
- Versionamento de API  
- Paginação de resultados  

---

## 📑 Documentação da API
Após rodar o projeto, acesse:

```
https://localhost:{porta}/swagger
```

Interface interativa para testar todos os endpoints.
(Também compatível com o Postman, se assim for necessário)

---

## ⚙️ Como Executar o Projeto
### Pré-requisitos
- .NET 8 SDK  
- MySQL  

---

### 🔧 Passos
```bash
# Clone o repositório
git clone https://github.com/seu-usuario/BibliotecaAPI.git

# Acesse a pasta
cd BibliotecaAPI

# Restaure os pacotes
dotnet restore

# Execute as migrations
dotnet ef database update

# Rode o projeto
dotnet run
```

---

## 📌 Estrutura do Projeto
```bash
BibliotecaAPI/
 ├── 📂 Context/
 ├── 📂 Controllers/
 ├── 📂 Data/
 ├── 📂 DTOs/
 ├── 📂 Enums/
 ├── 📂 Extensions/
 ├── 📂 Filters/
 ├── 📂 Migrations/
 ├── 📂 Models/
 ├── 📂 Pagination/
 ├── 📂 Repositories/
 ├── 📂 Services/
 ├── 📂 Settings/
 └── 📂 Swagger/
```

---

## 🎯 Objetivo do Projeto
Este projeto não é apenas um CRUD — ele foi pensado como um laboratório prático de backend, abordando:
- Arquitetura de APIs reais  
- Boas práticas com .NET  
- Autenticação moderna  
- Evolução incremental (V1 → V2)  

---

## 👨‍💻 Autor
Desenvolvido unicamente por mim, Gabriel Lentine, como parte da evolução prática em backend e APIs utilizando .NET e C#.
<br>Nov/2025 ~ Mar/2026
