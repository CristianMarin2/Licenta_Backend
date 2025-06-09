namespace SelfCheckoutSystem.Models.Dtos;

public class SaleSessionDto
{
    public Guid Id { get; set; }
    public string CashierCode { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public decimal InitialCashAmount { get; set; }
    public DateTime? EndTime { get; set; }

    public decimal? DeclaredFinalCashAmount { get; set; }
    public decimal ExpectedFinalCashAmount { get; set; }
}
