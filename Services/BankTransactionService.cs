using System.Globalization;
using BankBills.Entities;
using BankBills.Interfaces;
using BankBills.Services.Parsers;
using Npgsql.EntityFrameworkCore.PostgreSQL.Storage.Internal.Mapping;

namespace BankBills.Services;

public class BankTransactionService(
	ICsvParser csvParser,
	IBankTransactionRepository bankTransactionRepository,
	ITitleRepository titleRepository
) : IBankTransactionService
{
	public async Task ProcessNubankFileAsync(Stream fileStream)
	{
		var existingTitles = await titleRepository.GetAllTitlesAsync();
		var titleDictionary = existingTitles.ToDictionary(t => t.Name.ToUpper(), t => t.Id);

		var rawRecords = csvParser.Parse(fileStream).ToList();
		if (!rawRecords.Any()) return;

		var parsedDates = rawRecords
			.Select(r => DateOnly.TryParseExact(r.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var d) ? d : DateOnly.MinValue)
			.Where(d => d != DateOnly.MinValue)
			.ToList();

		var minDate = parsedDates.Min();
		var maxDate = parsedDates.Max();

		var existingTransactions = await bankTransactionRepository.GetTransactionsByDateRangeAsync(minDate, maxDate);
		
		var transactionOccurrences = new Dictionary<string, int>();
		foreach (var tx in existingTransactions)
		{
			var key = $"{tx.Date:yyyy-MM-dd}_{tx.TitleId}_{tx.Amount}";
			if (!transactionOccurrences.ContainsKey(key))
			{
				transactionOccurrences[key] = 0;
			}
			transactionOccurrences[key]++;
		}

		var bankTransactions = new List<BankTransaction>();

		foreach (var record in rawRecords)
		{
			if (!DateOnly.TryParseExact(record.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
				continue;

			if (!Double.TryParse(record.Amount, NumberStyles.Any, CultureInfo.InvariantCulture, out var amount))
				continue;

			var transactionType = amount < 0 ? TransactionType.OutFlow : TransactionType.InFlow;
			var absoluteAmount = Math.Abs(amount);

			var titleKey = record.Title.ToUpper();
			if (!titleDictionary.TryGetValue(titleKey, out Guid currentTitleId))
			{
				var newTitle = new Title(record.Title);
				await titleRepository.AddTitleAsync(newTitle);

				currentTitleId = newTitle.Id;
				titleDictionary.Add(titleKey, currentTitleId);
			}

			var uniqueKey = $"{date:yyyy-MM-dd}_{currentTitleId}_{absoluteAmount}";

			if (transactionOccurrences.ContainsKey(uniqueKey) && transactionOccurrences[uniqueKey] > 0)
			{
				transactionOccurrences[uniqueKey]--;
				continue;
			}

			var transaction = new BankTransaction(
				date: date,
				amount: absoluteAmount,
				type: transactionType,
				bank: BankType.Nubank,
				titleId: currentTitleId
			);

			bankTransactions.Add(transaction);
		}

		if (bankTransactions.Count > 0)
		{
			await bankTransactionRepository.AddRangeAsync(bankTransactions);
		}
	}
}