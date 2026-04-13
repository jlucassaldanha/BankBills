using BankBills.Infrastructure.Data;
using BankBills.Infrastructure.Repositories;
using BankBills.Application.Interfaces;
using BankBills.Application.Services;
using BankBills.Application.Services.Parsers;
using BankBills.Endpoints;
using BankBills.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
	options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ITitleRepository, TitleRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IAnalyticsRepository, AnalyticsRepository>();

builder.Services.AddScoped<ICsvParser, NubankCsvParser>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
	var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
	dbContext.Database.Migrate();
}

app.UseSwagger();
app.UseSwaggerUI();

app.MapTransactionEndpoints();
app.MapTitlesEndpoints();
app.MapAnalyticsEndpoints();

app.Run();
