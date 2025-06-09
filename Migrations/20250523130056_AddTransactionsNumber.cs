using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SelfCheckoutSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddTransactionsNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TransactionNumber",
                table: "SaleItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransactionNumber",
                table: "SaleItems");
        }
    }
}
