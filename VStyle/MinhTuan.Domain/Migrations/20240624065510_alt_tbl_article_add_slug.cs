using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MinhTuan.Domain.Migrations
{
    /// <inheritdoc />
    public partial class alt_tbl_article_add_slug : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "Articles");

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishDate",
                table: "Articles",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "Articles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublishDate",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "Articles");

            migrationBuilder.AddColumn<Guid>(
                name: "Category",
                table: "Articles",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
