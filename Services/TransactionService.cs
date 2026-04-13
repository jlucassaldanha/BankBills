using System.Globalization;
using BankBills.Entities;
using BankBills.Interfaces;

namespace BankBills.Services;

public class TransactionService(
	ICsvParser csvParser,
	ITransactionRepository bankTransactionRepository,
	ITitleRepository titleRepository
) : ITransactionService
{
	public async Task ProcessNubankFileAsync(Stream fileStream)
	{
		var existingTitles = await titleRepository.GetAllTitlesAsync();
		var titleDictionary = existingTitles.ToDictionary(t => t.Name.ToUpper(), t => t.Id);

		var rawRecords = csvParser.ParseNubank(fileStream).ToList();
		if (rawRecords.Count == 0) return;

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
			
			transactionOccurrences.TryGetValue(key, out int count);
			transactionOccurrences[key] = count + 1;
		}

		var bankTransactions = new List<BankTransaction>();
		foreach (var record in rawRecords)
		{
			if (!DateOnly.TryParseExact(record.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
				continue;

			if (!double.TryParse(record.Amount, NumberStyles.Any, CultureInfo.InvariantCulture, out var amount))
				continue;

			var transactionType = amount > 0 ? TransactionType.OutFlow : TransactionType.InFlow;
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
			if (transactionOccurrences.TryGetValue(uniqueKey, out int currentCount) && currentCount > 0)
			{
				transactionOccurrences[uniqueKey] = currentCount - 1;
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

	public async Task ProcessBBFileAsync(Stream fileStream)
	{
		var existingTitles = await titleRepository.GetAllTitlesAsync();
		var titleDictionary = existingTitles.ToDictionary(t => t.Name.ToUpper(), t => t.Id);

		var rawRecords = csvParser.ParseBB(fileStream).ToList();
		if (rawRecords.Count == 0) return;

		var parsedDates = rawRecords
			.Select(r => DateOnly.TryParseExact(r.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var d) ? d : DateOnly.MinValue)
			.Where(d => d != DateOnly.MinValue)
			.ToList();

		var minDate = parsedDates.Min();
		var maxDate = parsedDates.Max();

		var existingTransactions = await bankTransactionRepository.GetTransactionsByDateRangeAsync(minDate, maxDate); // Colocar filtro de qual banco?
		
		var transactionOccurrences = new Dictionary<string, int>();
		foreach (var tx in existingTransactions)
		{
			var key = $"{tx.Date:dd/MM/yyyy}_{tx.TitleId}_{tx.Amount}";
			
			transactionOccurrences.TryGetValue(key, out int count);
			transactionOccurrences[key] = count + 1;
		}

		var bankTransactions = new List<BankTransaction>();
		foreach (var record in rawRecords)
		{
			if (!DateOnly.TryParseExact(record.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
				continue;

			if (!double.TryParse(record.Amount[..^2], NumberStyles.Any, CultureInfo.InvariantCulture, out var amount))
				continue;

			var transactionType = record.Amount[^2..].Trim() == "D" ? TransactionType.OutFlow : TransactionType.InFlow;

			var titleKey = record.History.ToUpper();
			if (!titleDictionary.TryGetValue(titleKey, out Guid currentTitleId))
			{
				var newTitle = new Title(record.History);
				await titleRepository.AddTitleAsync(newTitle);

				currentTitleId = newTitle.Id;
				titleDictionary.Add(titleKey, currentTitleId);
			}

			var uniqueKey = $"{date:dd/MM/yyyy}_{currentTitleId}_{amount}";
			if (transactionOccurrences.TryGetValue(uniqueKey, out int currentCount) && currentCount > 0)
			{
				transactionOccurrences[uniqueKey] = currentCount - 1;
				continue;
			}

			var transaction = new BankTransaction(
				date: date,
				amount: amount,
				type: transactionType,
				bank: BankType.BB,
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