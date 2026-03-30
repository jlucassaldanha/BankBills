using BankBills.Entities;
using BankBills.Interfaces;

namespace BankBills.Controllers;

public static class BankTransactionControllers
{
	public static void MapBankTransactionControllers(this WebApplication app)
	{
		var group = app.MapGroup("/api/transactions");

		group.MapGet("/", async (
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

		group.MapPost("/nubank/import", async (
			IFormFileCollection files,
			IBankTransactionService bankTransactionService
		) =>
		{
			if (files is null || files.Count == 0)
			{
				return Results.BadRequest("Nenhum arquivo foi enviado.");
			}

			int processedFiles = 0;

			foreach(var file in files)
			{
				var extension = Path.GetExtension(file.FileName);
				if (!extension.Equals(".csv", StringComparison.OrdinalIgnoreCase))
					return Results.BadRequest("Apenas arquivos .csv são aceitos.");

				using var stream = file.OpenReadStream();
				await bankTransactionService.ProcessNubankFileAsync(stream);
				processedFiles++;
			}

			return Results.Ok("Arquivo processado e transações importadas para o banco de dados");
		})
		.DisableAntiforgery();
	}
}