var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

var livros = new List<Livro> { new Livro { Id = 1, Titulo = "1984", Autor = "George Orwell", ISBN = "978-0451524935", AnoPublicacao = 1949, Genero = "Distopia", Disponivel = true, DataCadastro = DateTime.Now.AddMonths(-6) }, new Livro { Id = 2, Titulo = "Dom Casmurro", Autor = "Machado de Assis", ISBN = "978-8525406958", AnoPublicacao = 1899, Genero = "Romance", Disponivel = false, DataCadastro = DateTime.Now.AddMonths(-5) }, new Livro { Id = 3, Titulo = "O Senhor dos Anéis", Autor = "J.R.R. Tolkien", ISBN = "978-0544003415", AnoPublicacao = 1954, Genero = "Fantasia", Disponivel = true, DataCadastro = DateTime.Now.AddMonths(-4) }, new Livro { Id = 4, Titulo = "Cem Anos de Solidão", Autor = "Gabriel García Márquez", ISBN = "978-0060883287", AnoPublicacao = 1967, Genero = "Realismo Mágico", Disponivel = true, DataCadastro = DateTime.Now.AddMonths(-3) }, new Livro { Id = 5, Titulo = "O Código Limpo", Autor = "Robert C. Martin", ISBN = "978-8576082675", AnoPublicacao = 2008, Genero = "Tecnologia", Disponivel = false, DataCadastro = DateTime.Now.AddMonths(-2) } };

int proximoId = 1;

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// GET /livros- Lista todos os livros
app.MapGet("/livros", () =>
{
    return Results.Ok(livros);
});

// GET /livros/id- Retorna o livro pelo id
app.MapGet("/livros/{id:int}", (int id) =>
{
    var livro = livros.FirstOrDefault(f => f.Id == id);
    return livro != null ? Results.Ok(livro) : Results.NotFound($"Livro {id} não encontrado!");
});

// GET /livros/genero/{genero} - Buscar livro por genero
app.MapGet("/livros/genero/{genero}", (string genero) =>
{
    var livrosFiltrados = livros.Where(f => f.Genero.Equals(genero, StringComparison.OrdinalIgnoreCase)).ToList();
    return livrosFiltrados.Any() ? Results.Ok(livrosFiltrados) : Results.NotFound($"Não existe livros para o gênero {genero}")
});

// POST /livros - Adicionar um novo livro
app.MapPost("/livros", (Livro entrada) =>
{
    // Validações
    if (string.IsNullOrWhiteSpace(entrada.Titulo))
        return Results.BadRequest("Título é obrigatório.");
    if (string.IsNullOrWhiteSpace(entrada.Autor))
        return Results.BadRequest("Autor é obrigatório.");
    if (livros.Any(l => l.ISBN == entrada.ISBN))
        return Results.BadRequest("Já existe um livro com esse ISBN.");
    if (entrada.AnoPublicacao > DateTime.Now.Year || entrada.AnoPublicacao < 1000)
        return Results.BadRequest("Ano de publicação inválido.");

    // Configurar propriedades automáticas
        Livro novoLivro = new Livro();
    novoLivro.Id = proximoId++;
    novoLivro.DataCadastro = DateTime.Now;
    novoLivro.Disponivel = true;

    //Propriedades
    novoLivro.Titulo = entrada.Titulo;
    novoLivro.AnoPublicacao = entrada.AnoPublicacao;
    novoLivro.Genero = entrada.Genero;
    novoLivro.ISBN = entrada.ISBN;
    novoLivro.Autor = entrada.Autor;

    livros.Add(novoLivro);
    return Results.Created($"/livros/{novoLivro.Id}", novoLivro);

});

// PUT /livros/id = Atualizar o livro
app.MapPut("/livros/{id:int}", (int id, Livro livroAtualizado) =>
{
    var livro = livros.FirstOrDefault(f => f.Id == id);

    if (livro == null)
        return Results.NotFound($"Livro com o id {id} não foi encontrado");
    if (string.IsNullOrWhiteSpace(livroAtualizado.Titulo))
        return Results.BadRequest("Título é obrigatório");

    livro.Titulo = livroAtualizado.Titulo;
    livro.Autor = livroAtualizado.Autor;
    livro.ISBN = livroAtualizado.ISBN;
    livro.AnoPublicacao = livroAtualizado.AnoPublicacao;
    livro.Genero = livroAtualizado.Genero;
    livro.Disponivel = livroAtualizado.Disponivel;

    return Results.Ok(livroAtualizado);
});

//DELETE /livros/id    - Remover o livro
app.MapDelete("/livros/{id:int}", (int id) =>
{
    var livro = livros.FirstOrDefault(f => f.Id == id);

    if (livro == null)
        return Results.NotFound($"Livro com o id {id} não foi encontrado");

    livros.Remove(livro);
    return Results.NoContent();
});
// PATCH /livros/id/disponibilidade - Altera disponibilidade do livro
app.MapPatch("/livros/{id:int}/disponibilidade", (int id) =>
{
    var livro = livros.FirstOrDefault(f => f.Id == id);

    if (livro == null)
        return Results.NotFound($"Livro com o id {id} não foi encontrado");

    livro.Disponivel = !livro.Disponivel;
    return Results.Ok(livro);
});



app.Run();

