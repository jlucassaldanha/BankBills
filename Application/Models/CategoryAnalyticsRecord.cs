namespace BankBills.Application.Models;

public record CategoryAnalyticsRecord(
    string TimeSpan,
	Guid TitleId,
	string TitleName,
	string Bank,
	double TotalSpent,
	int Days,
	double TotalMean
);

