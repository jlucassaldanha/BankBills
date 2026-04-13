namespace BankBills.Interfaces;

public interface ITransactionService
{
	Task ProcessNubankFileAsync(Stream fileStream);
	Task ProcessBBFileAsync(Stream fileStream);
}