namespace BankBills.Models;

public record BBCsvRecord
{
	public string Date { get; init; } = string.Empty;
	public string History { get; init; } = string.Empty;
	public string Amount { get; init; } = string.Empty; 
}