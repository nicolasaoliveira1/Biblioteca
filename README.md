📚 Biblioteca Digital API

Este projeto é uma Minimal API em C# para gerenciar o acervo de uma biblioteca digital.
A API permite realizar operações de consulta, cadastro, atualização e remoção de livros, seguindo os princípios REST.

🚀 Tecnologias Utilizadas

C#
.NET (Minimal API)
ASP.NET Core

⚙️ Como Executar o Projeto
1. Clonar o repositório
git clone https://github.com/seuusuario/BibliotecaApi.git
cd BibliotecaApi

2. Restaurar dependências
dotnet restore

3. Executar a aplicação
dotnet run

4. Acessar a API

Por padrão, a API estará disponível em:

http://localhost:5000


ou (HTTPS)

https://localhost:7000

📌 Endpoints Disponíveis
1. Listar todos os livros

GET /livros

Retorna todos os livros cadastrados.

2. Buscar livro por ID

GET /livros/{id}

Retorna um livro específico.

404 Not Found se não existir.

3. Buscar livros por gênero

GET /livros/genero/{genero}

Retorna lista filtrada por gênero (case insensitive).

Exemplo: /livros/genero/Fantasia

4. Adicionar novo livro

POST /livros

Cria um novo livro.

Validações:

Título e autor obrigatórios

ISBN deve ser único

Ano de publicação entre 1000 e ano atual

Retorno: 201 Created

5. Atualizar livro existente

PUT /livros/{id}

Atualiza as informações de um livro (exceto ID e DataCadastro).

404 Not Found se não existir.

6. Remover livro

DELETE /livros/{id}

Remove o livro informado.

Retorno: 204 No Content ou 404 Not Found.

7. Alterar disponibilidade

PATCH /livros/{id}/disponibilidade

Alterna o status de disponibilidade do livro (disponível / indisponível).

🧪 Testes Sugeridos

GET /livros → Deve retornar todos os livros

GET /livros/1 → Deve retornar o livro com ID 1

GET /livros/999 → Deve retornar 404

POST /livros → Criar livro válido

POST /livros → Criar livro com ISBN duplicado (deve falhar)

PUT /livros/1 → Atualizar livro existente

DELETE /livros/1 → Remover livro

GET /livros/genero/ficcao → Filtrar por gênero
