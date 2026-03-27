using BankBills.Entities;
using BankBills.Interfaces;
using Microsoft.EntityFrameworkCore;
using BankBills.Data;
using BankBills.DTOs;
using BankBills.Models;

namespace BankBills.Repositories;

public class BankTransactionRepository(AppDbContext db) : IBankTransactionRepository
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
	
	public async Task<List<BankTransactionRecord>> GetAllTransactionsAsync()
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
	}

	public async Task<List<BankTransactionRecord>> GetTransactionsByBankAsync(BankType bank)
	{
		return await _db.BankTransaction
			.Where(bt => bt.Bank == bank)
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
	}
	
	public async Task<List<BankTransactionRecord>> GetTransactionsByTitleAsync(Guid titleId)
	{
		return await _db.BankTransaction
			.Where(bt => bt.TitleId == titleId)
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
	}
	
	public async Task<List<BankTransactionRecord>> GetTransactionsByDateAsync(DateOnly date)
	{
		return await _db.BankTransaction
			.Where(bt => bt.Date == date)
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
	}
	
	public async Task<List<BankTransactionRecord>> GetTransactionsByTypeAsync(TransactionType type)
	{
		return await _db.BankTransaction
			.Where(bt => bt.Type == type)
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
	}
}