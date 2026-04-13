using BankBills.DTOs;
using BankBills.Entities;
using BankBills.Interfaces;
using BankBills.Models;

namespace BankBills.Endpoints;

public static class TransactionEndpoints
{
	public static void MapTransactionEndpoints(this WebApplication app)
	{
		var group = app.MapGroup("/api/transactions");

		group.MapGet("/", async (
			ITransactionRepository repo,
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

			return Results.Ok(new DataResponse<List<TransactionRecord>>(bankTransactions));
		})
		.WithName("Get Transactions");

		group.MapPost("/nubank/import", async (
			IFormFileCollection files,
			ITransactionService bankTransactionService
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

			return Results.Ok($"Transações importadas para com sucesso! {processedFiles} arquivos de {files.Count} processados.");
		})
		.DisableAntiforgery()
		.WithName("Post Nubank Transactions");

		group.MapPost("/bb/import", async (
			IFormFileCollection files,
			ITransactionService bankTransactionService
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
				await bankTransactionService.ProcessBBFileAsync(stream);
				processedFiles++;
			}

			return Results.Ok($"Transações importadas para com sucesso! {processedFiles} arquivos de {files.Count} processados.");
		})
		.DisableAntiforgery()
		.WithName("Post BB Transactions");
	}
}