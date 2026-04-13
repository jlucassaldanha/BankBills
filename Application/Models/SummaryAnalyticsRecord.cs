namespace BankBills.Application.Models;

public record SummaryAnalyticsRecord(
    string TimeSpan,
	double TotalInFlow,
	double TotalOutFlow,
	double DayMeanInFlow,
	double DayMeanOutFlow,
	double FinalTotal,
	string FinalFlow
);

