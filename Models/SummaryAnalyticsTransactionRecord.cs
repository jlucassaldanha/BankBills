namespace BankBills.Models;

public record SummaryAnalyticsTransactionRecord(
    DateOnly MaxDate,
	DateOnly MinDate,
	double TotalInFlow,
	double TotalOutFlow
);

