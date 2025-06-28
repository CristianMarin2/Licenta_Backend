using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SelfCheckoutSystem.Data;
using SelfCheckoutSystem.Models;
using SelfCheckoutSystem.Models.Dtos;
using SelfCheckoutSystem.Services;

namespace SelfCheckoutSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SaleTransactionController : ControllerBase
{
    private readonly SelfCheckoutDbContext _context;
    private readonly ReceiptGenerator _receiptGenerator;

    public SaleTransactionController(SelfCheckoutDbContext context, ReceiptGenerator receiptGenerator)
    {
        _context = context;
        _receiptGenerator = receiptGenerator;
    }

    [HttpPost]
    public async Task<ActionResult<SaleTransactionResponse>> CreateTransaction([FromBody] SaleTransactionDto dto)
    {
        var validationResult = await ValidateInputAsync(dto);
        if (validationResult is not null)
            return validationResult;

        var session = await GetSessionAsync(dto.SaleSessionId);
        if (session == null)
            return NotFound("Sesiunea nu a fost găsită.");

        var user = await _context.Users.FirstOrDefaultAsync(u => u.EmployeeCode == session.CashierCode);
        var isSelfCheckout = user?.EmployeeCode == "9999";

        var paymentValidation = ValidatePayment(dto, isSelfCheckout);
        if (paymentValidation is not null)
            return paymentValidation;

        var transaction = await BuildTransactionAsync(dto, session);
        _context.SaleTransactions.Add(transaction);
        await _context.SaveChangesAsync();

        await LoadProductReferencesAsync(transaction);

        transaction.SaleSession = session;

        var receiptUrl = _receiptGenerator.GeneratePdf(transaction);

        return CreatedAtAction(nameof(CreateTransaction), new { id = transaction.Id }, new SaleTransactionResponse
        {
            Id = transaction.Id,
            TotalAmount = transaction.TotalAmount,
            PaymentMethod = transaction.PaymentMethod.ToString(),
            DateTime = transaction.DateTime,
            ReceiptUrl = receiptUrl
        });
    }

    private async Task<ActionResult?> ValidateInputAsync(SaleTransactionDto dto)
    {
        if (dto.Items == null || dto.Items.Count == 0)
            return BadRequest("Tranzacția trebuie să conțină produse.");
        return null;
    }

    private async Task<SaleSession?> GetSessionAsync(Guid sessionId)
    {
        return await _context.SaleSessions
            .Include(s => s.Transactions)
            .ThenInclude(t => t.Items)
            .FirstOrDefaultAsync(s => s.Id == sessionId);
    }

    private ActionResult? ValidatePayment(SaleTransactionDto dto, bool isSelfCheckout)
    {
        var totalPaid = (dto.CashAmountReceived ?? 0) + (dto.CardAmountReceived ?? 0);

        if (!isSelfCheckout && dto.Status != "Canceled")
        {
            if (totalPaid < dto.TotalAmount)
                return BadRequest("Suma introdusă este insuficientă.");

            if (totalPaid > dto.TotalAmount && (dto.CashAmountReceived ?? 0) == 0)
                return BadRequest("Restul poate fi returnat doar dacă plata a fost parțial sau complet în numerar.");
        }

        return null;
    }

    private async Task<SaleTransaction> BuildTransactionAsync(SaleTransactionDto dto, SaleSession session)
    {
        var now = DateTime.UtcNow.AddTicks(-(DateTime.UtcNow.Ticks % TimeSpan.TicksPerSecond));
        var today = now.Date;
        var posTerminalNumber = 1;

        var countToday = await _context.SaleTransactions
            .Where(t => t.DateTime.Date == today && t.PosTerminalNumber == posTerminalNumber)
            .CountAsync();

        var formattedTransactionNumber = $"{today:ddMMyyyy}-{(countToday + 1):D3}-{posTerminalNumber}";
        var transactionId = Guid.NewGuid();
        var totalPaid = (dto.CashAmountReceived ?? 0) + (dto.CardAmountReceived ?? 0);
        var change = totalPaid > dto.TotalAmount ? totalPaid - dto.TotalAmount : 0;

        return new SaleTransaction
        {
            Id = transactionId,
            SaleSessionId = dto.SaleSessionId,
            DateTime = now,
            TransactionNumber = formattedTransactionNumber,
            PosTerminalNumber = posTerminalNumber,
            TotalAmount = dto.TotalAmount,
            PaymentMethod = (dto.CashAmountReceived > 0 && dto.CardAmountReceived > 0)
                ? PaymentMethod.Mixed
                : (dto.CardAmountReceived > 0 ? PaymentMethod.Card : PaymentMethod.Cash),
            CashAmountReceived = dto.CashAmountReceived,
            CardAmountReceived = dto.CardAmountReceived,
            ChangeGiven = change > 0 ? change : null,
            Status = dto.Status == "Canceled"
                ? TransactionStatus.Canceled
                : TransactionStatus.Succeeded,
            Items = dto.Items.Select(i => new SaleItem
            {
                Id = Guid.NewGuid(),
                TransactionId = transactionId,
                ProductBarcode = i.ProductBarcode,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                TransactionNumber = formattedTransactionNumber
            }).ToList()
        };
    }

    private async Task LoadProductReferencesAsync(SaleTransaction transaction)
    {
        foreach (var item in transaction.Items)
        {
            item.Product = await _context.Products.FindAsync(item.ProductBarcode);
        }
    }
}
