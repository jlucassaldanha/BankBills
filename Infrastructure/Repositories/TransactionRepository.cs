using BankBills.Domain.Entities;
using BankBills.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using BankBills.Infrastructure.Data;
using BankBills.Application.Models;
using BankBills.Domain.Enums;

namespace BankBills.Infrastructure.Repositories;

public class TransactionRepository(AppDbContext db) : ITransactionRepository
{
	private readonly AppDbContext _db = db;

	public async Task AddTransactionAsync(BankTransaction bankTransaction)
	{
		await _db.BankTransaction.AddAsync(bankTransaction);
		await _db.SaveChangesAsync();
	}

	public async Task AddRangeAsync(IEnumerable<BankTransaction> bankTransactions)
	{
		await _db.BankTransaction.AddRangeAsync(bankTransactions);
    	await _db.SaveChangesAsync();
	}

	public async Task UpdateTransactionAsync(BankTransaction bankTransaction)
	{
		_db.BankTransaction.Update(bankTransaction);
		await _db.SaveChangesAsync();
	}
	
	public async Task DeleteTransactionAsync(Guid bankTransactionId)
	{
		await _db.BankTransaction.Where(bt => bt.Id == bankTransactionId).ExecuteDeleteAsync();
	}

	public async Task<List<TransactionRecord>>GetTransactionsAsync(
		int? month = null, 
		int? year = null, 
		Guid? titleId = null, 
		BankType? bank = null, 
		TransactionType? type = null
	)
	{
		var query = _db.BankTransaction.AsQueryable();

		if (month.HasValue)
		{
			query = query.Where(bt => bt.Date.Month == month);
		}

		if (year.HasValue)
		{
			query = query.Where(bt => bt.Date.Year == year);
		}

		if (titleId.HasValue)
		{
			query = query.Where(bt => bt.TitleId.Equals(titleId));
		}

		if (bank.HasValue)
		{
			query = query.Where(bt => bt.Bank == bank);
		}

		if (type.HasValue)
		{
			query = query.Where(bt => bt.Type == type);
		}

		return await query
			.Select(bt => new TransactionRecord(
				bt.Id,
				bt.Date,
				bt.Amount,
				bt.Type.ToString(),
				bt.Bank.ToString(),
				bt.TitleId,
				bt.Title.Name
			))
			.ToListAsync();
	}
	
	/*public async Task<List<BankTransactionRecord>> GetAllTransactionsAsync()
	{
		return await _db.BankTransaction
			.Select(bt => new BankTransactionRecord(
				bt.Id,
				bt.Date,
				bt.Amount,
				bt.Type.ToString(),
				bt.Bank.ToString(),
				bt.TitleId,
				bt.Title.Name
			))
			.ToListAsync();
	}*/

	public async Task<List<TransactionRecord>> GetTransactionsByDateRangeAsync(DateOnly startDate, DateOnly endDate, BankType bank)
	{
		return await _db.BankTransaction
			.AsNoTracking()
			.Where(t => t.Date >= startDate && t.Date <= endDate)
			.Where(bt => bt.Bank == bank)
			.Select(bt => new TransactionRecord(
				bt.Id,
				bt.Date,
				bt.Amount,
				bt.Type.ToString(),
				bt.Bank.ToString(),
				bt.TitleId,
				bt.Title.Name
			))
			.ToListAsync();
	}
}