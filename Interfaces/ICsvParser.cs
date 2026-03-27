using BankBills.Models;

namespace BankBills.Interfaces;

public interface ICsvParser
{
	IEnumerable<NubankCsvRecord> Parse(Stream fileStream);
}