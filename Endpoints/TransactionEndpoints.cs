using BankBills.Application.DTOs;
using BankBills.Domain.Entities;
using BankBills.Application.Interfaces;
using BankBills.Domain.Interfaces;
using BankBills.Application.Models;
using BankBills.Domain.Enums;

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

		group.MapPost("/import/{bank}", async (
			string bank,
			IFormFileCollection files,
			ITransactionService bankTransactionService
		) =>
		{
			if (!Enum.TryParse<BankType>(bank, true, out var bankType))
			{
				var validBanks = string.Join(", ", Enum.GetNames<BankType>());
				return Results.BadRequest($"O Banco '{bank}' não é suportado. Tente um destes: {validBanks}");
			}

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
				await bankTransactionService.ProcessTransactionFileAsync(stream, bankType);
				processedFiles++;
			}

			return Results.Ok($"Transações importadas para com sucesso! {processedFiles} arquivos de {files.Count} processados.");
		})
		.DisableAntiforgery()
		.WithName("Post Bank Transactions");
	}
}