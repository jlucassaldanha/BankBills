namespace BankBills.Models;

public record TransactionAnalyticsSummaryRecord(
    DateOnly MaxDate,
	DateOnly MinDate,
	double TotalInFlow,
	double TotalOutFlow
);

