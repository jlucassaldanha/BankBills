using BankBills.Interfaces;

namespace BankBills.Endpoints;

public static class TitlesEndpoints
{
	public static void MapTitlesEndpoints(this WebApplication app)
	{
		var group = app.MapGroup("/api/titles");

		group.MapGet("/", async (ITitleRepository repo) =>
		{
			var titles = await repo.GetAllTitlesAsync();

            if (titles.Count <= 0)
				return Results.NotFound("Nenhum título encontrado.");

			return Results.Ok(titles);
		});
	}
}