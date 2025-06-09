using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SelfCheckoutSystem.Migrations
{
    /// <inheritdoc />
    public partial class RezolvareBuguri : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FinalCashAmount",
                table: "SaleSessions",
                newName: "ExpectedFinalCashAmount");

            migrationBuilder.AddColumn<decimal>(
                name: "DeclaredFinalCashAmount",
                table: "SaleSessions",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeclaredFinalCashAmount",
                table: "SaleSessions");

            migrationBuilder.RenameColumn(
                name: "ExpectedFinalCashAmount",
                table: "SaleSessions",
                newName: "FinalCashAmount");
        }
    }
}
