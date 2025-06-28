namespace SelfCheckoutSystem.Models.Dtos
{
    public class SaleTransactionResponse
    {
        public Guid Id { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public DateTime DateTime { get; set; }
        public string? ReceiptUrl { get; set; }

    }
}