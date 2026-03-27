using BankBills.Models;
using CsvHelper.Configuration;

namespace BankBills.Services.Parsers;

public class NubankCsvRecordMap : ClassMap<NubankCsvRecord>
{
	public NubankCsvRecordMap()
	{
		Map(m => m.Date).Name("date");
		Map(m => m.Title).Name("title");
		Map(m => m.Amount).Name("amount");
	}
}