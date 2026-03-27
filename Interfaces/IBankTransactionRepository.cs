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
	Task<List<BankTransactionRecord>> GetAllTransactionsAsync();
	Task<List<BankTransactionRecord>> GetTransactionsByBankAsync(BankType bank);
	Task<List<BankTransactionRecord>> GetTransactionsByTitleAsync(Guid titleId);
	Task<List<BankTransactionRecord>> GetTransactionsByDateAsync(DateOnly date);
	Task<List<BankTransactionRecord>> GetTransactionsByTypeAsync(TransactionType type);
}