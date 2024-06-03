using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MinhTuan.Domain.Migrations
{
    /// <inheritdoc />
    public partial class alt_tbl_banner_delete_ForeignKey : Migration
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

            migrationBuilder.AlterColumn<string>(
                name: "Path",
                table: "Banners",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Path",
                table: "Banners",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

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
