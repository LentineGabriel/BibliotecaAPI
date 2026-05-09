using BibliotecaAPI.Context;
using BibliotecaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaAPI.Data
{
    public static class DbSeeder
    {
        // essa classe foi criada p/ popular a base de dados com mais alguns registros (e feita 100% pelo Chat GPT 5.2, eu apenas revisei antes de implementar)

        public static async Task SeedAsync(AppDbContext db)
        {
            await using var tx = await db.Database.BeginTransactionAsync();

            // =========================
            // 1) AUTORES (NOVOS)
            // =========================
            var novosAutores = new List<Autor>
            {
                new Autor { PrimeiroNome = "George", Sobrenome = "Orwell", Nacionalidade = "Britânica", DataNascimento = new DateOnly(1903, 6, 25) },
                new Autor { PrimeiroNome = "J.R.R.", Sobrenome = "Tolkien", Nacionalidade = "Britânica", DataNascimento = new DateOnly(1892, 1, 3) },
                new Autor { PrimeiroNome = "Clarice", Sobrenome = "Lispector", Nacionalidade = "Brasileira", DataNascimento = new DateOnly(1920, 12, 10) },
                new Autor { PrimeiroNome = "Gabriel", Sobrenome = "García Márquez", Nacionalidade = "Colombiana", DataNascimento = new DateOnly(1927, 3, 6) },
                new Autor { PrimeiroNome = "Isaac", Sobrenome = "Asimov", Nacionalidade = "Americana", DataNascimento = new DateOnly(1920, 1, 2) }
            };

            foreach(var a in novosAutores)
            {
                bool existe = await db.Autores.AnyAsync(x =>
                    x.PrimeiroNome == a.PrimeiroNome &&
                    x.Sobrenome == a.Sobrenome &&
                    x.DataNascimento == a.DataNascimento);

                if(!existe)
                    db.Autores.Add(a);
            }
            await db.SaveChangesAsync();

            // =========================
            // 2) CATEGORIAS (APENAS AS DUAS DO DOMÍNIO)
            // =========================
            var categoriasBase = new List<Categorias>
            {
                new Categorias { NomeCategoria = "Ficção",     DescricaoCategoria = "Obras ficcionais." },
                new Categorias { NomeCategoria = "Não-Ficção", DescricaoCategoria = "Obras baseadas em fatos, estudos, relatos ou conteúdo técnico." }
            };

            foreach(var c in categoriasBase)
            {
                bool existe = await db.Categorias.AnyAsync(x => x.NomeCategoria == c.NomeCategoria);
                if(!existe)
                    db.Categorias.Add(c);
            }
            await db.SaveChangesAsync();

            // =========================
            // 3) EDITORAS (NOVAS)
            // =========================
            var novasEditoras = new List<Editoras>
            {
                new Editoras { NomeEditora = "Penguin Classics", PaisOrigem = "Reino Unido", AnoFundacao = 1946, SiteOficial = "https://www.penguin.co.uk" },
                new Editoras { NomeEditora = "DarkSide Books", PaisOrigem = "Brasil", AnoFundacao = 2012, SiteOficial = "https://darksidebooks.com.br" },
                new Editoras { NomeEditora = "Editora 34", PaisOrigem = "Brasil", AnoFundacao = 1992, SiteOficial = "https://www.editora34.com.br" }
            };

            foreach(var e in novasEditoras)
            {
                bool existe = await db.Editoras.AnyAsync(x => x.NomeEditora == e.NomeEditora);
                if(!existe)
                    db.Editoras.Add(e);
            }
            await db.SaveChangesAsync();

            // Recarrega para mapear IDs sem "chute"
            var autores = await db.Autores.AsNoTracking().ToListAsync();
            var categorias = await db.Categorias.AsNoTracking().ToListAsync();
            var editoras = await db.Editoras.AsNoTracking().ToListAsync();

            int AutorId(string primeiro , string sobrenome , DateOnly nascimento) =>
                autores.Single(x => x.PrimeiroNome == primeiro && x.Sobrenome == sobrenome && x.DataNascimento == nascimento).IdAutor;

            int CategoriaIdFixa(string nomeCategoria)
            {
                nomeCategoria = nomeCategoria.Trim();

                if(nomeCategoria != "Ficção" && nomeCategoria != "Não-Ficção")
                    throw new InvalidOperationException($"Categoria inválida: '{nomeCategoria}'. Permitidas: 'Ficção' e 'Não-Ficção'.");

                return categorias.Single(x => x.NomeCategoria == nomeCategoria).IdCategoria;
            }

            int EditoraId(string nomeEditora) =>
                editoras.Single(x => x.NomeEditora == nomeEditora).IdEditora;

            // =========================
            // 4) LIVROS (NOVOS)
            // =========================
            // Regra de dedupe: NomeLivro (pode ajustar se quiser considerar também autor/editora)
            var novosLivros = new List<Livros>
            {
                new Livros
                {
                    NomeLivro = "1984",
                    AnoPublicacao = 1949,
                    IdAutor = AutorId("George", "Orwell", new DateOnly(1903, 6, 25)),
                    IdEditora = EditoraId("Penguin Classics")
                },
                new Livros
                {
                    NomeLivro = "A Revolução dos Bichos",
                    AnoPublicacao = 1945,
                    IdAutor = AutorId("George", "Orwell", new DateOnly(1903, 6, 25)),
                    IdEditora = EditoraId("Penguin Classics")
                },
                new Livros
                {
                    NomeLivro = "O Hobbit",
                    AnoPublicacao = 1937,
                    IdAutor = AutorId("J.R.R.", "Tolkien", new DateOnly(1892, 1, 3)),
                    IdEditora = EditoraId("Penguin Classics")
                },
                new Livros
                {
                    NomeLivro = "O Senhor dos Anéis: A Sociedade do Anel",
                    AnoPublicacao = 1954,
                    IdAutor = AutorId("J.R.R.", "Tolkien", new DateOnly(1892, 1, 3)),
                    IdEditora = EditoraId("Penguin Classics")
                },
                new Livros
                {
                    NomeLivro = "A Hora da Estrela",
                    AnoPublicacao = 1977,
                    IdAutor = AutorId("Clarice", "Lispector", new DateOnly(1920, 12, 10)),
                    IdEditora = EditoraId("Editora 34")
                },
                new Livros
                {
                    NomeLivro = "Cem Anos de Solidão",
                    AnoPublicacao = 1967,
                    IdAutor = AutorId("Gabriel", "García Márquez", new DateOnly(1927, 3, 6)),
                    IdEditora = EditoraId("Penguin Classics")
                },
                new Livros
                {
                    NomeLivro = "Eu, Robô",
                    AnoPublicacao = 1950,
                    IdAutor = AutorId("Isaac", "Asimov", new DateOnly(1920, 1, 2)),
                    IdEditora = EditoraId("Penguin Classics")
                },
                new Livros
                {
                    NomeLivro = "Fundação",
                    AnoPublicacao = 1951,
                    IdAutor = AutorId("Isaac", "Asimov", new DateOnly(1920, 1, 2)),
                    IdEditora = EditoraId("Penguin Classics")
                }
            };

            foreach(var l in novosLivros)
            {
                bool existe = await db.Livros.AnyAsync(x => x.NomeLivro == l.NomeLivro);
                if(!existe)
                    db.Livros.Add(l);
            }
            await db.SaveChangesAsync();

            // Recarrega livros para pegar IDs
            var livros = await db.Livros.AsNoTracking().ToListAsync();

            int LivroId(string nomeLivro) =>
                livros.Single(x => x.NomeLivro == nomeLivro).IdLivro;

            // =========================
            // 5) LIVROS_CATEGORIAS (NOVOS VÍNCULOS)
            // =========================
            // Aqui eu também uso categorias que você já tem (Ficção, Ficção Científica) + novas
            var vinculos = new (string Livro , string Categoria)[]
            {
                ("1984", "Ficção"),
                ("A Revolução dos Bichos", "Ficção"),
                ("O Hobbit", "Ficção"),
                ("O Senhor dos Anéis: A Sociedade do Anel", "Ficção"),
                ("A Hora da Estrela", "Ficção"),
                ("Cem Anos de Solidão", "Ficção"),
                ("Eu, Robô", "Ficção"),
                ("Fundação", "Ficção"),
            };

            foreach(var (livroNome , categoriaNome) in vinculos)
            {
                // Se por algum motivo a categoria não existir, isso explode no Single.
                // Mantém a seed "barulhenta" (melhor do que falhar silenciosamente).
                var livroId = LivroId(livroNome);
                var categoriaId = CategoriaIdFixa(categoriaNome);

                bool existe = await db.LivrosCategorias.AnyAsync(x =>
                    x.LivroId == livroId && x.CategoriaId == categoriaId);

                if(!existe)
                {
                    db.LivrosCategorias.Add(new LivroCategoria
                    {
                        LivroId = livroId ,
                        CategoriaId = categoriaId
                    });
                }
            }

            await db.SaveChangesAsync();
            await tx.CommitAsync();
        }
    }
}
