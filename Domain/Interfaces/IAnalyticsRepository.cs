using BankBills.Domain.Entities;
using BankBills.Application.Models;
using BankBills.Domain.Enums;

namespace BankBills.Domain.Interfaces;

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