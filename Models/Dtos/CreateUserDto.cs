namespace SelfCheckoutSystem.Models.Dtos
{
    public class CreateUserDto
    {
        public string EmployeeCode { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = "Cashier";
    }
}
