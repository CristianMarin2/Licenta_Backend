using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SelfCheckoutSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddCardAmountReceived : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "CardAmountReceived",
                table: "SaleTransactions",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CardAmountReceived",
                table: "SaleTransactions");
        }
    }
}
