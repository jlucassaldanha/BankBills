using BankBills.Application.Models;
using BankBills.Domain.Enums;

namespace BankBills.Application.Interfaces;

public interface ITransactionService
{
	Task ProcessTransactionFileAsync(Stream fileStream, BankType bank);
}