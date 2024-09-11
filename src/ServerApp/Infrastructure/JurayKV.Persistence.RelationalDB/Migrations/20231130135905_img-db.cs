using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayKV.Persistence.RelationalDB.Migrations
{
    /// <inheritdoc />
    public partial class imgdb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageKey",
                table: "kvAds");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "kvAds");

            migrationBuilder.AddColumn<Guid>(
                name: "ImageFileId",
                table: "kvAds",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ImageFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShowInDropdown = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageFiles", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_kvAds_ImageFileId",
                table: "kvAds",
                column: "ImageFileId",
                unique: true,
                filter: "[ImageFileId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_kvAds_ImageFiles_ImageFileId",
                table: "kvAds",
                column: "ImageFileId",
                principalTable: "ImageFiles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_kvAds_ImageFiles_ImageFileId",
                table: "kvAds");

            migrationBuilder.DropTable(
                name: "ImageFiles");

            migrationBuilder.DropIndex(
                name: "IX_kvAds_ImageFileId",
                table: "kvAds");

            migrationBuilder.DropColumn(
                name: "ImageFileId",
                table: "kvAds");

            migrationBuilder.AddColumn<string>(
                name: "ImageKey",
                table: "kvAds",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "kvAds",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
