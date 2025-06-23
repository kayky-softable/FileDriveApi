using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FileDriveAPi.Migrations
{
    /// <inheritdoc />
    public partial class FIleDriveApi_migration_v1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "MyFiles");

            migrationBuilder.DropColumn(
                name: "Length",
                table: "MyFiles");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "MyFiles");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "MyFiles",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "Length",
                table: "MyFiles",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "MyFiles",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
