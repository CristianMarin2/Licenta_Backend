using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SelfCheckoutSystem.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Barcode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    VatRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Barcode);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    EmployeeCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.EmployeeCode);
                });

            migrationBuilder.CreateTable(
                name: "SaleSessions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CashierCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InitialCashAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DeclaredFinalCashAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ExpectedFinalCashAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaleSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SaleSessions_Users_CashierCode",
                        column: x => x.CashierCode,
                        principalTable: "Users",
                        principalColumn: "EmployeeCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SaleTransactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TransactionNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PosTerminalNumber = table.Column<int>(type: "int", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentMethod = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CashAmountReceived = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CardAmountReceived = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ChangeGiven = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SaleSessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaleTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SaleTransactions_SaleSessions_SaleSessionId",
                        column: x => x.SaleSessionId,
                        principalTable: "SaleSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SaleItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TransactionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TransactionNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductBarcode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaleItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SaleItems_Products_ProductBarcode",
                        column: x => x.ProductBarcode,
                        principalTable: "Products",
                        principalColumn: "Barcode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SaleItems_SaleTransactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "SaleTransactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SaleItems_ProductBarcode",
                table: "SaleItems",
                column: "ProductBarcode");

            migrationBuilder.CreateIndex(
                name: "IX_SaleItems_TransactionId",
                table: "SaleItems",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleSessions_CashierCode",
                table: "SaleSessions",
                column: "CashierCode");

            migrationBuilder.CreateIndex(
                name: "IX_SaleTransactions_SaleSessionId",
                table: "SaleTransactions",
                column: "SaleSessionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SaleItems");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "SaleTransactions");

            migrationBuilder.DropTable(
                name: "SaleSessions");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
