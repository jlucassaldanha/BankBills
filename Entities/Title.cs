namespace BankBills.Entities;

public class Title(string name)
{
	public Guid Id { get; private set; } = Guid.NewGuid();
	public string Name { get; set; } = name;
}