using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace blog_include.Migrations
{
    /// <inheritdoc />
    public partial class imagemadd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Imagem",
                table: "Postagens",
                type: "longblob",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Imagem",
                table: "Postagens");
        }
    }
}
