using BankBills.Application.Models;

namespace BankBills.Application.Interfaces;

public interface ICsvParser
{
	IEnumerable<NubankCsvRecord> ParseNubank(Stream fileStream);
	IEnumerable<BBCsvRecord> ParseBB(Stream fileStream);
}