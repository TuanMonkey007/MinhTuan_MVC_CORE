using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MinhTuan.Domain.Migrations
{
    /// <inheritdoc />
    public partial class altTable_DataCategory_ParentID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
      name: "FK_DataCategories_Categories_ParentId",
      table: "DataCategories");
            migrationBuilder.DropColumn(
                name: "ParentCode",
                table: "DataCategories");
           
            migrationBuilder.AddColumn<Guid>(
                name: "ParentId",
                table: "DataCategories",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
            migrationBuilder.AddForeignKey(
     name: "FK_DataCategories_Categories_ParentIdv2",
     table: "DataCategories",
     column: "ParentId",
     principalTable: "Categories",
     principalColumn: "Id",
     onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "DataCategories");

            migrationBuilder.AddColumn<string>(
                name: "ParentCode",
                table: "DataCategories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
