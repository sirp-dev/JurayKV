using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayKV.Persistence.RelationalDB.Migrations
{
    /// <inheritdoc />
    public partial class updteundilute8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TransactionVerificationId",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransactionVerificationId",
                table: "Transactions");
        }
    }
}
