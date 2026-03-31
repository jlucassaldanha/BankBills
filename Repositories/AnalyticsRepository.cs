using BankBills.Entities;
using BankBills.Interfaces;
using Microsoft.EntityFrameworkCore;
using BankBills.Data;
using BankBills.Models;

namespace BankBills.Repositories;

public class AnalyticsRepository(AppDbContext db) : IAnalyticsRepository
{
	private readonly AppDbContext _db = db;

	public async Task<SummaryAnalyticsTransactionRecord> GetSummaryAsync(
		int? month = null, 
		int? year = null, 
		Guid? titleId = null, 
		BankType? bank = null
	)
	{
		var query = _db.BankTransaction.AsQueryable();

		if (month.HasValue) query = query.Where(t => t.Date.Month == month);
		if (year.HasValue) query = query.Where(t => t.Date.Year == year);
		if (titleId.HasValue) query = query.Where(t => t.TitleId.Equals(titleId));
		if (bank.HasValue) query = query.Where(t => t.Bank == bank);

		var result = await query
			.AsNoTracking()
			.GroupBy(t => 1) 
			.Select(g => new SummaryAnalyticsTransactionRecord(
				g.Max(t => t.Date),
				g.Min(t => t.Date),
				g.Where(t => t.Type == TransactionType.InFlow).Sum(t => t.Amount),
				g.Where(t => t.Type == TransactionType.OutFlow).Sum(t => t.Amount)
			))
			.FirstOrDefaultAsync();

		return result ?? new SummaryAnalyticsTransactionRecord(new DateOnly(), new DateOnly(), 0, 0);
	}

	public async Task<List<CategoryAnalyticsTransactionRecord>> GetCategorySpentAsync(
		int? month = null, 
		int? year = null, 
		BankType? bank = null
	)
	{
		var query = _db.BankTransaction.AsQueryable();

		if (month.HasValue) query = query.Where(t => t.Date.Month == month);
		if (year.HasValue) query = query.Where(t => t.Date.Year == year);
		if (bank.HasValue) query = query.Where(t => t.Bank == bank);

		query = query.Where(t => t.Type == TransactionType.OutFlow);

		var rawData = await query
			.AsNoTracking()
			.GroupBy(t => new { t.TitleId, t.Title.Name }) 
			.Select(g => new {
				MaxDate = g.Max(t => t.Date),
				MinDate = g.Min(t => t.Date),
				TitleId = g.Key.TitleId,
				Name = g.Key.Name,
				TotalSpent = g.Sum(t => t.Amount)
			})
			.OrderByDescending(c => c.TotalSpent)
			.ToListAsync();

		return [.. rawData
			.Select(data => new CategoryAnalyticsTransactionRecord(
				data.MaxDate,
				data.MinDate,
				data.TitleId,
				data.Name,
				data.TotalSpent
			))];
	}
}