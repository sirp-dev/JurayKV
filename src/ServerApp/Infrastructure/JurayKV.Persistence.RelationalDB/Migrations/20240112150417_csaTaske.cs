using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayKV.Persistence.RelationalDB.Migrations
{
    /// <inheritdoc />
    public partial class csaTaske : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResponseOnTieRequest",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Tie2Request",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResponseOnTieRequest",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Tie2Request",
                table: "AspNetUsers");
        }
    }
}
