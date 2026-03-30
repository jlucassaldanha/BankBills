using BankBills.Entities;
using BankBills.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BankBills.Controllers;

public static class BankTransactionControllers
{
	public static void MapBankTransactionControllers(this WebApplication app)
	{
		app.MapPost("/api/transactions/nubank/import", async (
			IFormFile file,
			IBankTransactionService bankTransactionService
		) =>
		{
			if (file is null || file.Length == 0)
			{
				return Results.BadRequest("Nenhum arquivo enviado.");
			}

			var extension = Path.GetExtension(file.FileName);
			if (!extension.Equals(".csv", StringComparison.OrdinalIgnoreCase))
			{
				return Results.BadRequest("Apenas arquivos .csv são aceitos.");
			}

			using var stream = file.OpenReadStream();
			await bankTransactionService.ProcessNubankFileAsync(stream);

			return Results.Ok("Arquivo processado e transações importadas para o banco de dados");
		})
		.DisableAntiforgery();

		app.MapGet("/api/transactions", async (
			IBankTransactionRepository repo,
			int? month = null, 
			int? year = null, 
			Guid? titleId = null, 
			BankType? bank = null, 
			TransactionType? type = null 
		) =>
		{
			var bankTransactions = await repo.GetTransactionsAsync(month, year, titleId, bank, type);

			if (bankTransactions.Count <= 0)
				return Results.NotFound("Transações não encontradas.");

			return Results.Ok(bankTransactions);
		});
	}
}