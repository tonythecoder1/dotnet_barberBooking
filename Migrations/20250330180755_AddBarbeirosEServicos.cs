using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dotidentity.Migrations
{
    /// <inheritdoc />
    public partial class AddBarbeirosEServicos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BarbeiroId",
                table: "Reservas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ServicoId",
                table: "Reservas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Barbeiros",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Barbeiros", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Servicos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Servicos", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_BarbeiroId",
                table: "Reservas",
                column: "BarbeiroId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_ServicoId",
                table: "Reservas",
                column: "ServicoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservas_Barbeiros_BarbeiroId",
                table: "Reservas",
                column: "BarbeiroId",
                principalTable: "Barbeiros",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservas_Servicos_ServicoId",
                table: "Reservas",
                column: "ServicoId",
                principalTable: "Servicos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservas_Barbeiros_BarbeiroId",
                table: "Reservas");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservas_Servicos_ServicoId",
                table: "Reservas");

            migrationBuilder.DropTable(
                name: "Barbeiros");

            migrationBuilder.DropTable(
                name: "Servicos");

            migrationBuilder.DropIndex(
                name: "IX_Reservas_BarbeiroId",
                table: "Reservas");

            migrationBuilder.DropIndex(
                name: "IX_Reservas_ServicoId",
                table: "Reservas");

            migrationBuilder.DropColumn(
                name: "BarbeiroId",
                table: "Reservas");

            migrationBuilder.DropColumn(
                name: "ServicoId",
                table: "Reservas");
        }
    }
}
