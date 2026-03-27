using Microsoft.EntityFrameworkCore;
using BankBills.Entities;

namespace BankBills.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
	public DbSet<BankTransaction> BankTransaction { get; set; }
    public DbSet<Title> Title { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<BankTransaction>()
            .Property(t => t.Bank)
            .HasConversion<string>();
	}
}