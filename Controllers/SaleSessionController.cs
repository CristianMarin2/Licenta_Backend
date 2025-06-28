using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SelfCheckoutSystem.Data;
using SelfCheckoutSystem.Models;
using SelfCheckoutSystem.Models.Dtos;

namespace SelfCheckoutSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SaleSessionController : ControllerBase
{
    private readonly SelfCheckoutDbContext _context;

    public SaleSessionController(SelfCheckoutDbContext context)
    {
        _context = context;
    }

    // GET: /api/salesession/active
    [HttpGet("active")]
    public async Task<IActionResult> GetActiveSessions()
    {
        var sessions = await _context.SaleSessions
            .Where(s => s.EndTime == null)
            .ToListAsync();

        return Ok(sessions);
    }

    // POST: /api/salesession/start
    [HttpPost("start")]
    public async Task<IActionResult> StartSession([FromBody] StartSessionDto dto)
    {
        var session = new SaleSession
        {
            Id = Guid.NewGuid(),
            CashierCode = dto.CashierCode,
            InitialCashAmount = dto.InitialCashAmount,
            StartTime = DateTime.UtcNow
        };

        _context.SaleSessions.Add(session);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetActiveSessions), new { id = session.Id }, session);
    }

// POST: /api/salesession/end/{id}
[HttpPost("end/{id}")]
public async Task<IActionResult> EndSession(Guid id, [FromBody] EndSessionDto dto)
{
    var session = await _context.SaleSessions
        .Include(s => s.Transactions)
        .FirstOrDefaultAsync(s => s.Id == id);

    if (session == null)
        return NotFound();

    session.DeclaredFinalCashAmount = dto.DeclaredFinalCashAmount;
    session.EndTime = DateTime.UtcNow;

    var totalCashIn = session.Transactions
        .Where(t => t.Status == TransactionStatus.Succeeded)
        .Sum(t => t.CashAmountReceived);

    var totalChangeOut = session.Transactions
        .Where(t => t.Status == TransactionStatus.Succeeded)
        .Sum(t => t.ChangeGiven.GetValueOrDefault());

    session.ExpectedFinalCashAmount = session.InitialCashAmount + totalCashIn - totalChangeOut;

    await _context.SaveChangesAsync();

    var responseDto = new SaleSessionDto
    {
        Id = session.Id,
        CashierCode = session.CashierCode,
        StartTime = session.StartTime,
        EndTime = session.EndTime,
        InitialCashAmount = session.InitialCashAmount,
        DeclaredFinalCashAmount = session.DeclaredFinalCashAmount,
        ExpectedFinalCashAmount = session.ExpectedFinalCashAmount ?? 0
    };

    return Ok(responseDto);
}
}