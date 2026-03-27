using System.Globalization;
using BankBills.Interfaces;
using BankBills.Models;
using CsvHelper;
using CsvHelper.Configuration;

namespace BankBills.Services.Parsers;

public class NubankCsvParser : ICsvParser
{
	public IEnumerable<NubankCsvRecord> Parse(Stream fileStream)
	{
		var config = new CsvConfiguration(CultureInfo.InvariantCulture)
		{
			HasHeaderRecord = true,
			Delimiter = ",",
			MissingFieldFound = null
		};

		using var reader = new StreamReader(fileStream);
		using var csv = new CsvReader(reader, config);

		csv.Context.RegisterClassMap<NubankCsvRecordMap>();

		return [.. csv.GetRecords<NubankCsvRecord>()];
	}
}