using BankBills.Entities;
using BankBills.Interfaces;

namespace BankBills.Controllers;

public static class AnalyticsControllers
{
	public static void MapAnalyticsControllers(this WebApplication app)
	{
		var group = app.MapGroup("/api/analytics");

		group.MapGet("/summary", async (
			IAnalyticsService analyticsService,
			int? month = null, 
			int? year = null, 
			Guid? titleId = null, 
			BankType? bank = null
		) =>
		{
			return await analyticsService.SummaryAnalyticsAsync(month, year, titleId, bank);
		});

		group.MapGet("/top-categories", async (IAnalyticsService analyticsService,
			int? month = null, 
			int? year = null, 
			BankType? bank = null
		) =>
		{
			return await analyticsService.CategoryAnalyticsAsync(month, year, bank);
		});

		group.MapGet("/prediction", async () => {});
	}
}