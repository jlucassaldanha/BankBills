using BankBills.Models;
using CsvHelper.Configuration;

namespace BankBills.Services.Parsers;

public class BBCsvRecordMap : ClassMap<BBCsvRecord>
{
	public BBCsvRecordMap()
	{
		Map(m => m.Date).Name("Data");
		Map(m => m.History).Name("Hist�rico");
		Map(m => m.Amount).Name("Valor");
	}
}