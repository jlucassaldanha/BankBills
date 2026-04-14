using System.Globalization;
using BankBills.Domain.Entities;
using BankBills.Application.Interfaces;
using BankBills.Domain.Interfaces;
using BankBills.Domain.Enums;
using BankBills.Application.Models;

namespace BankBills.Application.Services;

public class TransactionService(
	ICsvParser csvParser,
	ITransactionRepository transactionRepository,
	ITitleRepository titleRepository
) : ITransactionService
{
	public async Task ProcessTransactionFileAsync(Stream fileStream, BankType bank)
	{
		List<ParsedTransactionRecord> records = [];
		
		if (bank == BankType.Nubank)
			records = await ProcessNubankFileAsync(fileStream);
		if (bank == BankType.BB)
			records = await ProcessBBFileAsync(fileStream);

		if (records.Count == 0) return;

		var existingTitles = await titleRepository.GetAllTitlesAsync();
		var titleDictionary = existingTitles.ToDictionary(t => t.Name.ToUpper(), t => t.Id);
		
		var parsedDates = records
			.Select(r => r.Date)
			.Where(d => d != DateOnly.MinValue)
			.ToList();

		var minDate = parsedDates.Min();
		var maxDate = parsedDates.Max();

		var existingTransactions = await transactionRepository.GetTransactionsByDateRangeAsync(minDate, maxDate, bank);

		var transactionOccurrences = new Dictionary<string, int>();
		foreach (var tx in existingTransactions)
		{
			var key = $"{tx.Date:yyyy-MM-dd}_{tx.TitleId}_{tx.Amount}";
			
			transactionOccurrences.TryGetValue(key, out int count);
			transactionOccurrences[key] = count + 1;
		}

		var bankTransactions = new List<BankTransaction>();
		foreach (var record in records)
		{
			var titleKey = record.Title.ToUpper();
			if (!titleDictionary.TryGetValue(titleKey, out Guid currentTitleId))
			{
				var newTitle = new Title(record.Title);
				await titleRepository.AddTitleAsync(newTitle);

				currentTitleId = newTitle.Id;
				titleDictionary.Add(titleKey, currentTitleId);
			}

			var uniqueKey = $"{record.Date:yyyy-MM-dd}_{currentTitleId}_{record.Amount}";
			if (transactionOccurrences.TryGetValue(uniqueKey, out int currentCount) && currentCount > 0)
			{
				transactionOccurrences[uniqueKey] = currentCount - 1;
				continue;
			}

			var transaction = new BankTransaction(
				date: record.Date,
				amount: record.Amount,
				type: record.Type,
				bank: bank,
				titleId: currentTitleId
			);

			bankTransactions.Add(transaction);
		}

		if (bankTransactions.Count > 0)
		{
			await transactionRepository.AddRangeAsync(bankTransactions);
		}
	}

	private async Task<List<ParsedTransactionRecord>> ProcessNubankFileAsync(Stream fileStream)
	{
		var rawRecords = csvParser.ParseNubank(fileStream).ToList();
		var parsedRecords = new List<ParsedTransactionRecord>();

		foreach (var record in rawRecords)
		{
			if (!DateOnly.TryParseExact(record.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
				continue;

			if (!double.TryParse(record.Amount, NumberStyles.Any, CultureInfo.InvariantCulture, out var amount))
				continue;

			var transactionType = amount > 0 ? TransactionType.OutFlow : TransactionType.InFlow;
			var absoluteAmount = Math.Abs(amount);
			
			var transaction = new ParsedTransactionRecord(
				Date: date,
				Amount: absoluteAmount,
				Type: transactionType,
				Title: record.Title
			);

			parsedRecords.Add(transaction);
		}

		return parsedRecords;
	}

	private async Task<List<ParsedTransactionRecord>> ProcessBBFileAsync(Stream fileStream)
	{
		var rawRecords = csvParser.ParseBB(fileStream).ToList();
		var parsedRecords = new List<ParsedTransactionRecord>();

		foreach (var record in rawRecords)
		{
			if (!DateOnly.TryParseExact(record.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
				continue;

			if (!double.TryParse(record.Amount[..^2], NumberStyles.Any, new CultureInfo("pt-BR"), out var amount))
				continue;

			var transactionType = record.Amount[^2..].Trim() == "D" ? TransactionType.OutFlow : TransactionType.InFlow;

			var transaction = new ParsedTransactionRecord(
				Date: date,
				Amount: amount,
				Type: transactionType,
				Title: record.History
			);

			parsedRecords.Add(transaction);
		}

		return parsedRecords;
	}
}