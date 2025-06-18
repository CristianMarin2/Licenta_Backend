namespace SelfCheckoutSystem.Models.Dtos
{
    public class UpdateUserDto
    {
        public string Username { get; set; } = string.Empty;
        public string Role { get; set; } = "Cashier";
        public bool IsActive { get; set; } = true;
    }
}
