using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MinhTuan.Domain.Migrations
{
    /// <inheritdoc />
    public partial class alt_tbl_banner_delete_FKv2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Banners_DataCategories_CategoryId",
                table: "Banners");

            migrationBuilder.DropIndex(
                name: "IX_Banners_CategoryId",
                table: "Banners");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Banners_CategoryId",
                table: "Banners",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Banners_DataCategories_CategoryId",
                table: "Banners",
                column: "CategoryId",
                principalTable: "DataCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
