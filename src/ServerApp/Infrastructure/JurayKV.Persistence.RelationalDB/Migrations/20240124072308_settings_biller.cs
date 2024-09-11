using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayKV.Persistence.RelationalDB.Migrations
{
    /// <inheritdoc />
    public partial class settings_biller : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Tier",
                table: "Variations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "DisableAirtime",
                table: "Settings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "DisableBetting",
                table: "Settings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "DisableData",
                table: "Settings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "DisableElectricity",
                table: "Settings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "DisableReferralBonus",
                table: "Settings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "DisableTV",
                table: "Settings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Tier",
                table: "CategoryVariations",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tier",
                table: "Variations");

            migrationBuilder.DropColumn(
                name: "DisableAirtime",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "DisableBetting",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "DisableData",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "DisableElectricity",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "DisableReferralBonus",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "DisableTV",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "Tier",
                table: "CategoryVariations");
        }
    }
}
