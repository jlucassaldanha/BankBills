using BankBills.Entities;

namespace BankBills.Interfaces;

public interface ITitleRepository
{
	Task AddTitleAsync(Title title);
	Task UpdateTitleAsync(Title title);
	Task DeleteTitleAsync(Guid titleId);
	Task<List<Title>> GetAllTitlesAsync();
}