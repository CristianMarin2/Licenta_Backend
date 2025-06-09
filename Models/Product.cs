using System.ComponentModel.DataAnnotations;

namespace SelfCheckoutSystem.Models;

public class Product
{
    [Key]
    [Required]
    public string Barcode { get; set; } = string.Empty;

    [Required]
    public string Name { get; set; } = string.Empty;

    [Range(0, double.MaxValue)]
    public decimal Price { get; set; }

    [Range(0, 1)]
    public decimal VatRate { get; set; }

    public bool IsActive { get; set; } = true;

    public List<SaleItem> SaleItems { get; set; } = new();
}
