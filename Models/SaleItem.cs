using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SelfCheckoutSystem.Models;

public class SaleItem
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid TransactionId { get; set; }

    [ForeignKey(nameof(TransactionId))]
    public SaleTransaction Transaction { get; set; } = null!;

    [Required]
    public string TransactionNumber { get; set; } = string.Empty;

    [Required]
    public string ProductBarcode { get; set; } = string.Empty;

    [ForeignKey(nameof(ProductBarcode))]
    public Product Product { get; set; } = null!;

    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }

    [Range(0, double.MaxValue)]
    public decimal UnitPrice { get; set; }
}
