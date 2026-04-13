using BankBills.Application.DTOs;
using BankBills.Application.Interfaces;
using BankBills.Application.Models;
using BankBills.Domain.Enums;

namespace BankBills.Endpoints;

public static class AnalyticsEndpoints
{
	public static void MapAnalyticsEndpoints(this WebApplication app)
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
			var response = await analyticsService.SummaryAnalyticsAsync(month, year, titleId, bank);
			return Results.Ok(new DataResponse<SummaryAnalyticsRecord>(response));
		})
		.WithName("Get Summary");

		group.MapGet("/top-categories", async (
			IAnalyticsService analyticsService,
			int? month = null, 
			int? year = null, 
			BankType? bank = null
		) =>
		{
			var response = await analyticsService.CategoryAnalyticsAsync(month, year, bank);
			return Results.Ok(new DataResponse<List<CategoryAnalyticsRecord>>(response));
		})
		.WithName("Get Top Categories");
	}
}