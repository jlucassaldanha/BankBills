namespace BankBills.Entities;

public enum TransactionType
{
	InFlow = 1,
	OutFlow = 2
}

public enum BankType
{
	Nubank = 1
}

public class BankTransaction(DateOnly date, double amount, TransactionType type, BankType bank, Guid titleId)
{
	public Guid Id { get; private set; } = Guid.NewGuid();
	public DateOnly Date { get; set; } = date;
	public double Amount { get; set; } = amount;
	public TransactionType Type { get; set; } = type;
	public virtual BankType Bank { get; set; } = bank;

	public Guid TitleId { get; set; } = titleId;
	public Title Title { get; set; } = null!;
}