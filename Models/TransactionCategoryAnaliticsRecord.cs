using BankBills.Entities;

namespace BankBills.Models;

public record TransactionCategoryAnalyticsRecord(
    DateOnly MaxDate,
	DateOnly MinDate,
	Guid TitleId,
	string TitleName,
	double TotalSpent
);

