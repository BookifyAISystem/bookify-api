using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bookify_data.Migrations
{
    /// <inheritdoc />
    public partial class AddBookContentVersionSummaries : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AiSummary",
                table: "BookContentVersion",
                newName: "Summary5");

            migrationBuilder.AddColumn<string>(
                name: "Summary1",
                table: "BookContentVersion",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Summary2",
                table: "BookContentVersion",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Summary3",
                table: "BookContentVersion",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Summary4",
                table: "BookContentVersion",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Summary1",
                table: "BookContentVersion");

            migrationBuilder.DropColumn(
                name: "Summary2",
                table: "BookContentVersion");

            migrationBuilder.DropColumn(
                name: "Summary3",
                table: "BookContentVersion");

            migrationBuilder.DropColumn(
                name: "Summary4",
                table: "BookContentVersion");

            migrationBuilder.RenameColumn(
                name: "Summary5",
                table: "BookContentVersion",
                newName: "AiSummary");
        }
    }
}
