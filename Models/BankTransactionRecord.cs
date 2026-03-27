namespace BankBills.Models;

public record BankTransactionRecord(
    Guid Id,
    DateOnly Date,
    double Amount,
    string Type,
    string Bank,
    Guid TitleId,
    string TitleName
);