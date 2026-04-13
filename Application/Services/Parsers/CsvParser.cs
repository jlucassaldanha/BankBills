using System.Globalization;
using BankBills.Application.Interfaces;
using BankBills.Application.Models;
using CsvHelper;
using CsvHelper.Configuration;

namespace BankBills.Application.Services.Parsers;

public class NubankCsvParser : ICsvParser
{
	public IEnumerable<NubankCsvRecord> ParseNubank(Stream fileStream)
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

	public IEnumerable<BBCsvRecord> ParseBB(Stream fileStream)
	{
		var config = new CsvConfiguration(CultureInfo.InvariantCulture)
		{
			HasHeaderRecord = true,
			Delimiter = ",",
			MissingFieldFound = null
		};

		using var reader = new StreamReader(fileStream);
		using var csv = new CsvReader(reader, config);

		csv.Context.RegisterClassMap<BBCsvRecordMap>();

		return [.. csv.GetRecords<BBCsvRecord>()];
	}
}