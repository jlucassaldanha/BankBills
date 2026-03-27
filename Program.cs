using BankBills.Controllers;
using BankBills.Data;
using BankBills.Interfaces;
using BankBills.Repositories;
using BankBills.Services;
using BankBills.Services.Parsers;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
	options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ITitleRepository, TitleRepository>();
builder.Services.AddScoped<IBankTransactionRepository, BankTransactionRepository>();

builder.Services.AddScoped<ICsvParser, NubankCsvParser>();
builder.Services.AddScoped<IBankTransactionService, BankTransactionService>();

var app = builder.Build();

app.MapBankTransactionControllers();

app.Run();
