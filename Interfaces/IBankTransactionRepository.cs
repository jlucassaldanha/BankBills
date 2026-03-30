using BankBills.DTOs;
using BankBills.Entities;
using BankBills.Models;

namespace BankBills.Interfaces;

public interface IBankTransactionRepository
{
	Task AddTransactionAsync(BankTransaction bankTransaction);
	Task AddRangeAsync(IEnumerable<BankTransaction> bankTransactions);
	Task UpdateTransactionAsync(BankTransaction bankTransaction);
	Task DeleteTransactionAsync(Guid bankTransactionId);
	Task<List<BankTransactionRecord>> GetTransactionsAsync(
		int? month = null, 
		int? year = null, 
		Guid? titleId = null, 
		BankType? bank = null, 
		TransactionType? type = null
	);
	Task<List<BankTransactionRecord>> GetAllTransactionsAsync();
	Task<List<BankTransactionRecord>> GetTransactionsByDateRangeAsync(DateOnly startDate, DateOnly endDate);
}