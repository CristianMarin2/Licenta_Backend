using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using SelfCheckoutSystem.Models;
using System.Globalization;

namespace SelfCheckoutSystem.Services;

public class ReceiptGenerator
{
    private readonly IWebHostEnvironment _env;

    public ReceiptGenerator(IWebHostEnvironment env)
    {
        _env = env;
    }

    public string GeneratePdf(SaleTransaction transaction)
    {
        var fileName = $"receipt_{transaction.Id}.pdf";
        var outputDir = Path.Combine(_env.WebRootPath ?? "wwwroot", "receipts");
        var outputPath = Path.Combine(outputDir, fileName);

        Directory.CreateDirectory(outputDir);

        Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A6);
                page.Margin(10);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(10));

                page.Header().Column(col =>
                {
                    col.Item().Text("Lucrare Licenta").Bold().FontSize(16).AlignCenter();
                    col.Item().Text("Bon fiscal").Bold().FontSize(12).AlignCenter();
                    col.Item().Text($"Bon nr: {transaction.TransactionNumber}").FontSize(10).AlignCenter();
                    col.Item().Text($"Data: {transaction.DateTime:dd.MM.yyyy HH:mm:ss}").FontSize(10).AlignCenter();
                });

                page.Content().Column(col =>
                {
                    col.Spacing(5);

                    foreach (var item in transaction.Items)
                    {
                        col.Item().Row(row =>
                        {
                            row.RelativeItem(3).Text($"{item.Product?.Name ?? item.ProductBarcode} x{item.Quantity}");
                            row.RelativeItem().AlignRight().Text($"{item.UnitPrice * item.Quantity:0.00} lei");
                        });
                    }

                    col.Item().LineHorizontal(0.5f);
                    col.Item().Row(row =>
                    {
                        row.RelativeItem(3).Text("Total").Bold();
                        row.RelativeItem().AlignRight().Text($"{transaction.TotalAmount:0.00} lei").Bold();
                    });

                    if (transaction.CashAmountReceived > 0)
                    {
                        col.Item().Row(row =>
                        {
                            row.RelativeItem(3).Text("Plătit cash:");
                            row.RelativeItem().AlignRight().Text($"{transaction.CashAmountReceived:0.00} lei");
                        });
                    }

                    if (transaction.CardAmountReceived > 0)
                    {
                        col.Item().Row(row =>
                        {
                            row.RelativeItem(3).Text("Plătit card:");
                            row.RelativeItem().AlignRight().Text($"{transaction.CardAmountReceived:0.00} lei");
                        });
                    }

                    if (transaction.ChangeGiven is > 0)
                    {
                        col.Item().Row(row =>
                        {
                            row.RelativeItem(3).Text("Rest:");
                            row.RelativeItem().AlignRight().Text($"{transaction.ChangeGiven:0.00} lei");
                        });
                    }

                    col.Item().LineHorizontal(0.5f);
                    col.Item().AlignCenter().Text("Vă mulțumim!").FontSize(10).Italic();
                });
            });
        }).GeneratePdf(outputPath);

        var relativePath = $"/receipts/{fileName}";
        return relativePath;
    }
}
