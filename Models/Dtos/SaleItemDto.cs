namespace SelfCheckoutSystem.Models.Dtos
{
      public class SaleItemDto
    {
        public string ProductBarcode { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}