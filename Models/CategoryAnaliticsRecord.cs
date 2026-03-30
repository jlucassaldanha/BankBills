using BankBills.Entities;

namespace BankBills.Models;

public record CategoryAnalyticsRecord(
    string TimeSpan,
	Guid TitleId,
	string TitleName,
	double TotalSpent,
	int Days,
	double TotalMean
);

