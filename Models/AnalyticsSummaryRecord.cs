using BankBills.Entities;

namespace BankBills.Models;

public record AnalyticsSummaryRecord(
    string TimeSpan,
	double TotalInFlow,
	double TotalOutFlow,
	double DayMeanInFlow,
	double DayMeanOutFlow,
	double FinalTotal,
	string FinalFlow
);

