using BankBills.Entities;
using BankBills.Models;

namespace BankBills.Interfaces;

public interface IAnalyticsRepository
{
	Task<SummaryAnalyticsTransactionRecord> GetSummaryAsync(
		int? month = null, 
		int? year = null, 
		Guid? titleId = null, 
		BankType? bank = null
	);
	Task<List<CategoryAnalyticsTransactionRecord>> GetCategorySpentAsync(
		int? month = null, 
		int? year = null,  
		BankType? bank = null
	);
}