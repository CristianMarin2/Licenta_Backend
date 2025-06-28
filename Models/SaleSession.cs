using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SelfCheckoutSystem.Models;

public class SaleSession
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public string CashierCode { get; set; } = string.Empty;

    [ForeignKey(nameof(CashierCode))]
    public User Cashier { get; set; } = null!;

    public DateTime StartTime { get; set; } = DateTime.UtcNow;
    public DateTime? EndTime { get; set; }

    [Range(0, double.MaxValue)]
    public decimal InitialCashAmount { get; set; }

    [Range(0, double.MaxValue)]
    public decimal? DeclaredFinalCashAmount { get; set; }

    [Range(0, double.MaxValue)]
    public decimal? ExpectedFinalCashAmount { get; set; }

    public List<SaleTransaction> Transactions { get; set; } = new();
}
