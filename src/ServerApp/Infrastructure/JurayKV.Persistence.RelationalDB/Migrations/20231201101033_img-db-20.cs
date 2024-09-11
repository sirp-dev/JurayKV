using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayKV.Persistence.RelationalDB.Migrations
{
    /// <inheritdoc />
    public partial class imgdb20 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_kvAds_ImageFileId",
                table: "kvAds");

            migrationBuilder.CreateIndex(
                name: "IX_kvAds_ImageFileId",
                table: "kvAds",
                column: "ImageFileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_kvAds_ImageFileId",
                table: "kvAds");

            migrationBuilder.CreateIndex(
                name: "IX_kvAds_ImageFileId",
                table: "kvAds",
                column: "ImageFileId",
                unique: true,
                filter: "[ImageFileId] IS NOT NULL");
        }
    }
}
