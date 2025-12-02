using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BibliotecaAPI.Migrations
{
    /// <inheritdoc />
    public partial class AjusteForeignKeyCategoria : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Livros_Categorias_IdCategoria",
                table: "Livros");

            migrationBuilder.DropIndex(
                name: "IX_Livros_IdCategoria",
                table: "Livros");

            migrationBuilder.AddColumn<int>(
                name: "CategoriaLivroIdCategoria",
                table: "Livros",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Livros_CategoriaLivroIdCategoria",
                table: "Livros",
                column: "CategoriaLivroIdCategoria");

            migrationBuilder.AddForeignKey(
                name: "FK_Livros_Categorias_CategoriaLivroIdCategoria",
                table: "Livros",
                column: "CategoriaLivroIdCategoria",
                principalTable: "Categorias",
                principalColumn: "IdCategoria");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Livros_Categorias_CategoriaLivroIdCategoria",
                table: "Livros");

            migrationBuilder.DropIndex(
                name: "IX_Livros_CategoriaLivroIdCategoria",
                table: "Livros");

            migrationBuilder.DropColumn(
                name: "CategoriaLivroIdCategoria",
                table: "Livros");

            migrationBuilder.CreateIndex(
                name: "IX_Livros_IdCategoria",
                table: "Livros",
                column: "IdCategoria");

            migrationBuilder.AddForeignKey(
                name: "FK_Livros_Categorias_IdCategoria",
                table: "Livros",
                column: "IdCategoria",
                principalTable: "Categorias",
                principalColumn: "IdCategoria",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
