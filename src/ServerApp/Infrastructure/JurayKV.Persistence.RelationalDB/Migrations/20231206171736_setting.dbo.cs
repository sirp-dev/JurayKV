using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayKV.Persistence.RelationalDB.Migrations
{
    /// <inheritdoc />
    public partial class settingdbo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DefaultAmountPerView = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MinimumAmountBudget = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Settings");
        }
    }
}
