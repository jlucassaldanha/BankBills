namespace BankBills.Interfaces;

public interface IBankTransactionService
{
	Task ProcessNubankFileAsync(Stream fileStream);
}