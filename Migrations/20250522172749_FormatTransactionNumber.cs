using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SelfCheckoutSystem.Migrations
{
    /// <inheritdoc />
    public partial class FormatTransactionNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PosTerminalNumber",
                table: "SaleTransactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TransactionNumber",
                table: "SaleTransactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PosTerminalNumber",
                table: "SaleTransactions");

            migrationBuilder.DropColumn(
                name: "TransactionNumber",
                table: "SaleTransactions");
        }
    }
}
