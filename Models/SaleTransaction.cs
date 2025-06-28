using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SelfCheckoutSystem.Models;

public enum PaymentMethod
{
    Cash,
    Card,
    Mixed
}

public enum TransactionStatus
{
    Succeeded,
    Canceled
}

public class SaleTransaction
{
    [Key]
    public Guid Id { get; set; }

    public DateTime DateTime { get; set; } = DateTime.UtcNow
        .AddTicks(-(DateTime.UtcNow.Ticks % TimeSpan.TicksPerSecond));

    [Required]
    public string TransactionNumber { get; set; } = string.Empty; 

    public int PosTerminalNumber { get; set; }  

    [Range(0, double.MaxValue)]
    public decimal TotalAmount { get; set; }

    public PaymentMethod PaymentMethod { get; set; }

    public TransactionStatus Status { get; set; } = TransactionStatus.Succeeded;

    public decimal? CashAmountReceived { get; set; }

    public decimal? CardAmountReceived { get; set; }

    public decimal? ChangeGiven { get; set; }

    public Guid SaleSessionId { get; set; }

    [ForeignKey(nameof(SaleSessionId))]
    public SaleSession SaleSession { get; set; } = null!;

    public List<SaleItem> Items { get; set; } = new();
}

