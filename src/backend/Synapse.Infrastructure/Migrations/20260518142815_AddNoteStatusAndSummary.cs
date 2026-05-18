using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Synapse.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNoteStatusAndSummary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Notes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Summary",
                table: "Notes",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "Summary",
                table: "Notes");
        }
    }
}
