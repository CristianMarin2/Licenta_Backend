namespace SelfCheckoutSystem.Models.Dtos
{
    public class SaleTransactionDto
    {
        public Guid SaleSessionId { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentMethod { get; set; } = "Cash";
        public decimal? CashAmountReceived { get; set; }
        public decimal? CardAmountReceived { get; set; }
        public decimal? ChangeGiven { get; set; }
        public string Status { get; set; } = "Succeeded";
        public List<SaleItemDto> Items { get; set; } = new();
    }
}
