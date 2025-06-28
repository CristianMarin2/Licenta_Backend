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

    // GET: /api/users
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetAll([FromQuery] bool? onlyActive = null)
    {
        var query = _context.Users.AsQueryable();

        if (onlyActive == true)
            query = query.Where(u => u.IsActive);

        return await query.ToListAsync();
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
            PasswordHash = "1234",
            Role = dto.Role,
            IsActive = true, 
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

        if (user == null || !user.IsActive)
            return Unauthorized("Cont inactiv sau date greșite.");

        return Ok(new
        {
            employeeCode = user.EmployeeCode,
            username = user.Username,
            role = user.Role
        });
    }

    // PUT: /api/users/{employeeCode}/active
    [HttpPut("{employeeCode}/active")]
    public async Task<IActionResult> SetActiveStatus(string employeeCode, [FromBody] bool isActive)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.EmployeeCode == employeeCode);
        if (user == null)
            return NotFound();

        user.IsActive = isActive;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPut("{employeeCode}")]
public async Task<IActionResult> UpdateUser(string employeeCode, [FromBody] UpdateUserDto dto)
{
    var user = await _context.Users.FirstOrDefaultAsync(u => u.EmployeeCode == employeeCode);
    if (user == null)
        return NotFound();

    user.Username = dto.Username;
    user.Role = dto.Role;
    user.IsActive = dto.IsActive;

    await _context.SaveChangesAsync();
    return NoContent();
}

}



public class LoginRequest
{
    public string EmployeeCode { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
