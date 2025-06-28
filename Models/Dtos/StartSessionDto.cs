namespace SelfCheckoutSystem.Models.Dtos
{
    public class StartSessionDto
    {
        public string CashierCode { get; set; } = string.Empty;
        public decimal InitialCashAmount { get; set; }
    }
}
