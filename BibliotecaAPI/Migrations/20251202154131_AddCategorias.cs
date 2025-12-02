using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BibliotecaAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddCategorias : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdCategoria",
                table: "Livros",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    IdCategoria = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NomeCategoria = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DescricaoCategoria = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.IdCategoria);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Livros_Categorias_IdCategoria",
                table: "Livros");

            migrationBuilder.DropTable(
                name: "Categorias");

            migrationBuilder.DropIndex(
                name: "IX_Livros_IdCategoria",
                table: "Livros");

            migrationBuilder.DropColumn(
                name: "IdCategoria",
                table: "Livros");
        }
    }
}
