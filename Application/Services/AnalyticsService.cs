using BankBills.Application.Models;
using BankBills.Application.Interfaces;
using BankBills.Domain.Interfaces;
using BankBills.Domain.Enums;

namespace BankBills.Application.Services;

public class AnalyticsService(
	IAnalyticsRepository analyticsRepository
) : IAnalyticsService
{
	public async Task<SummaryAnalyticsRecord> SummaryAnalyticsAsync(
		int? month = null, 
		int? year = null, 
		Guid? titleId = null, 
		BankType? bank = null
	)
	{
		var result = await analyticsRepository.GetSummaryAsync(month, year, titleId, bank);

		if (result.TotalInFlow == 0 && result.TotalOutFlow == 0)
			return new SummaryAnalyticsRecord("None", 0, 0, 0, 0, 0, "InFlow");
		
		var timeSpan = $"{result.MinDate:dd/MM/yyyy} até {result.MaxDate:dd/MM/yyyy}";

		int totalDays = result.MaxDate.DayNumber - result.MinDate.DayNumber + 1;

		double finalTotal = result.TotalInFlow - result.TotalOutFlow;

		var finalFlow = finalTotal >= 0 ? TransactionType.InFlow : TransactionType.OutFlow;

		return new SummaryAnalyticsRecord(
			timeSpan,
			Math.Round(result.TotalInFlow, 2),
			Math.Round(result.TotalOutFlow, 2),
			Math.Round(result.TotalInFlow / totalDays, 2),
			Math.Round(result.TotalOutFlow / totalDays, 2),
			Math.Round(Math.Abs(finalTotal), 2),
			finalFlow.ToString()
		);
	}

	public async Task<List<CategoryAnalyticsRecord>> CategoryAnalyticsAsync(
		int? month = null, 
		int? year = null,  
		BankType? bank = null
	)
	{
		var result = await analyticsRepository.GetCategorySpentAsync(month, year, bank);

		var categoriesAnalytics = new List<CategoryAnalyticsRecord>();
		foreach(var category in result)
		{
			var timeSpan = $"{category.MinDate:dd/MM/yyyy} até {category.MaxDate:dd/MM/yyyy}";
			int totalDays = category.MaxDate.DayNumber - category.MinDate.DayNumber + 1;

			//var banks = string.Join(", ", category.Bank.Distinct());

			categoriesAnalytics.Add(new CategoryAnalyticsRecord(
				timeSpan,
				category.TitleId,
				category.TitleName,
				category.Bank,
				Math.Round(category.TotalSpent, 2),
				totalDays,
				Math.Round(category.TotalSpent / totalDays, 2)
			));
		}

		return categoriesAnalytics;
	}
}