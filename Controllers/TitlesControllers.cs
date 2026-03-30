using BankBills.Interfaces;

namespace BankBills.Controllers;

public static class TitlesControllers
{
	public static void MapTitlesControllers(this WebApplication app)
	{
		app.MapGet("/api/titles", async (ITitleRepository repo) =>
		{
			var titles = await repo.GetAllTitlesAsync();

            if (titles.Count <= 0)
				return Results.NotFound("Nenhum título encontrado.");

			return Results.Ok(titles);
		});
	}
}