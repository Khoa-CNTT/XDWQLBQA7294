using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eshopp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "tb_Product",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "tb_Posts",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "tb_News",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "tb_Category",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "tb_Product");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "tb_Posts");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "tb_News");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "tb_Category");
        }
    }
}
