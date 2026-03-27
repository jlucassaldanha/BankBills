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

		app.MapGet("/api/transactions", async (IBankTransactionRepository repo) =>
		{
			var bankTransactions = await repo.GetAllTransactionsAsync();
			return Results.Ok(bankTransactions);
		});

		app.MapGet("/api/transactions/nubank", async (IBankTransactionRepository repo) =>
		{
			var bankTransactions = await repo.GetTransactionsByBankAsync(BankType.Nubank);
			return Results.Ok(bankTransactions);
		});

		// Colocar filtros opcionais como o tipo, o banco, a data, o titulo

		/*app.MapGet("/api/titles", async (ITitleRepository repo) =>
		{
			var titles = await repo.GetAllTitlesAsync();
			return Results.Ok(titles);
		});*/
	}
}