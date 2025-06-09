using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SelfCheckoutSystem.Data;
using System.Text;
using SelfCheckoutSystem.Services;
using QuestPDF.Infrastructure;
QuestPDF.Settings.License = LicenseType.Community;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<SelfCheckoutDbContext>(options =>
    options.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=SelfCheckoutDB;Trusted_Connection=True;TrustServerCertificate=True"));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "http://localhost:3001")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// 5. Adaugă serviciul de generare PDF
builder.Services.AddSingleton<ReceiptGenerator>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseCors("AllowFrontend");
app.UseAuthorization();

app.UseStaticFiles();
app.MapControllers();

app.Run();
