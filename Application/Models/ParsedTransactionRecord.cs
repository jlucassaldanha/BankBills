using BankBills.Domain.Enums;

namespace BankBills.Application.Models;
public record ParsedTransactionRecord(DateOnly Date, double Amount, TransactionType Type, string Title);