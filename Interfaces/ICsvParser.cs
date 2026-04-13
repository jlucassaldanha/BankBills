using BankBills.Models;

namespace BankBills.Interfaces;

public interface ICsvParser
{
	IEnumerable<NubankCsvRecord> ParseNubank(Stream fileStream);
	IEnumerable<BBCsvRecord> ParseBB(Stream fileStream);
}