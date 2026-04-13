namespace BankBills.Models;

public record CategoryAnalyticsTransactionRecord(
    DateOnly MaxDate,
	DateOnly MinDate,
	Guid TitleId,
	string TitleName,
	string? Bank,
	double TotalSpent
);

