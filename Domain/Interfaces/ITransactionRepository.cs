using BankBills.Domain.Entities;
using BankBills.Application.Models;
using BankBills.Domain.Enums;

namespace BankBills.Domain.Interfaces;

public interface ITransactionRepository
{
	Task AddTransactionAsync(BankTransaction bankTransaction);
	Task AddRangeAsync(IEnumerable<BankTransaction> bankTransactions);
	Task UpdateTransactionAsync(BankTransaction bankTransaction);
	Task DeleteTransactionAsync(Guid bankTransactionId);
	Task<List<TransactionRecord>> GetTransactionsAsync(
		int? month = null, 
		int? year = null, 
		Guid? titleId = null, 
		BankType? bank = null, 
		TransactionType? type = null
	);
	//Task<List<BankTransactionRecord>> GetAllTransactionsAsync();
	Task<List<TransactionRecord>> GetTransactionsByDateRangeAsync(DateOnly startDate, DateOnly endDate);
}