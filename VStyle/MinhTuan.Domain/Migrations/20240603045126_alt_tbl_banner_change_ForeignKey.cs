using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MinhTuan.Domain.Migrations
{
    /// <inheritdoc />
    public partial class alt_tbl_banner_change_ForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
               name: "FK_Banners_DataCategories_CategoryId",
               table: "Banners");
            migrationBuilder.AddForeignKey(
        name: "FK_Banners_DataCategories_CategoryId",
        table: "Banners",
        column: "CategoryId",
        principalTable: "DataCategories",
        principalColumn: "Id",
        onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
               name: "FK_Banners_DataCategories_CategoryId",
               table: "Banners");
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