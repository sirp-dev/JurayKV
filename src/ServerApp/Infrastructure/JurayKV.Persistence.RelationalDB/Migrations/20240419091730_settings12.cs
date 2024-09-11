using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayKV.Persistence.RelationalDB.Migrations
{
    /// <inheritdoc />
    public partial class settings12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BankAccount",
                table: "Settings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankAccountNumber",
                table: "Settings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankName",
                table: "Settings",
                type: "nvarchar(max)",
                nullable: true);

            //migrationBuilder.AddColumn<int>(
            //    name: "PaymentGateWay",
            //    table: "Settings",
            //    type: "int",
            //    nullable: false,
            //    defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BankAccount",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "BankAccountNumber",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "BankName",
                table: "Settings");

            //migrationBuilder.DropColumn(
            //    name: "PaymentGateWay",
            //    table: "Settings");
        }
    }
}
