using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SelfCheckoutSystem.Data;
using SelfCheckoutSystem.Models;
using SelfCheckoutSystem.Models.Dtos;

namespace SelfCheckoutSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly SelfCheckoutDbContext _context;

    public UsersController(SelfCheckoutDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetAll()
    {
        return await _context.Users.ToListAsync();
    }

    // POST: /api/users
[HttpPost]
public async Task<ActionResult<User>> CreateUser([FromBody] CreateUserDto dto)
{
    var exists = await _context.Users.AnyAsync(u => u.EmployeeCode == dto.EmployeeCode);
    if (exists)
        return Conflict("Codul de angajat există deja.");

    var user = new User
    {
        EmployeeCode = dto.EmployeeCode,
        Username = dto.Username,
        PasswordHash = dto.PasswordHash,
        Role = dto.Role,

        SaleSessions = new()
    };

    _context.Users.Add(user);
    await _context.SaveChangesAsync();

    return CreatedAtAction(nameof(GetAll), new { code = user.EmployeeCode }, user);
}

    // POST: /api/users/login
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.EmployeeCode == request.EmployeeCode && u.PasswordHash == request.Password);

        if (user == null)
            return Unauthorized("Username sau parolă invalidă.");

        return Ok(new
        {
            employeeCode = user.EmployeeCode,
            username = user.Username,
            role = user.Role
        });
    }
}

public class LoginRequest
{
    public string EmployeeCode { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
