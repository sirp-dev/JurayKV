using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayKV.Persistence.RelationalDB.Migrations
{
    /// <inheritdoc />
    public partial class NoteToUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Note",
                table: "Transactions",
                newName: "UniqueReference");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UniqueReference",
                table: "Transactions",
                newName: "Note");
        }
    }
}
