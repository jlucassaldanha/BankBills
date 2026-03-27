namespace BankBills.Models;

public record NubankCsvRecord
{
	public string Date { get; init; } = string.Empty;
	public string Title { get; init; } = string.Empty;
	public string Amount { get; init; } = string.Empty; 
}