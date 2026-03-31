using BankBills.Models;
using BankBills.Entities;

namespace BankBills.Interfaces;

public interface IAnalyticsService
{
	Task<SummaryAnalyticsRecord> SummaryAnalyticsAsync(
		int? month = null, 
		int? year = null, 
		Guid? titleId = null, 
		BankType? bank = null
	);

	Task<List<CategoryAnalyticsRecord>> CategoryAnalyticsAsync(
		int? month = null, 
		int? year = null,  
		BankType? bank = null
	);
}