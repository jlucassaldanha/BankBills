using BankBills.Entities;
using BankBills.Models;

namespace BankBills.Interfaces;

public interface ITransactionsAnalyticsRepository
{
	Task<TransactionAnalyticsSummaryRecord> GetSummaryAsync(
		int? month = null, 
		int? year = null, 
		Guid? titleId = null, 
		BankType? bank = null
	);
	Task<List<TransactionCategoryAnalyticsRecord>> GetCategorySpentAsync(
		int? month = null, 
		int? year = null,  
		BankType? bank = null
	);
}