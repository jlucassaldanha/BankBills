using BankBills.Domain.Entities;

namespace BankBills.Domain.Interfaces;

public interface ITitleRepository
{
	Task AddTitleAsync(Title title);
	Task UpdateTitleAsync(Title title);
	Task DeleteTitleAsync(Guid titleId);
	Task<List<Title>> GetAllTitlesAsync();
}