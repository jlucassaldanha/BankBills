namespace BankBills.Application.Models;

public record SummaryAnalyticsTransactionRecord(
    DateOnly MaxDate,
	DateOnly MinDate,
	double TotalInFlow,
	double TotalOutFlow
);

