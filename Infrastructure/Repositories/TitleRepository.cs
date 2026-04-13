using BankBills.Domain.Entities;
using BankBills.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using BankBills.Infrastructure.Data;

namespace BankBills.Infrastructure.Repositories;

public class TitleRepository(AppDbContext db) : ITitleRepository
{
	private readonly AppDbContext _db = db;

	public async Task AddTitleAsync(Title title)
	{
		await _db.Title.AddAsync(title);
		await _db.SaveChangesAsync();
	}

	public async Task UpdateTitleAsync(Title title) 
	{
		_db.Title.Update(title);
		await _db.SaveChangesAsync();
	}

	public async Task DeleteTitleAsync(Guid titleId) 
	{
		await _db.Title.Where(t => t.Id == titleId).ExecuteDeleteAsync();
		await _db.SaveChangesAsync();
	}

	public async Task<List<Title>> GetAllTitlesAsync()
	{
		return await _db.Title.ToListAsync();
	}
}