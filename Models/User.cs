using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SelfCheckoutSystem.Models;

public class User
{
    [Key]
    [Required]
    public string EmployeeCode { get; set; } = string.Empty;

    [Required]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    public string Role { get; set; } = "Cashier";

    [JsonIgnore]
    public List<SaleSession> SaleSessions { get; set; } = new();
}
