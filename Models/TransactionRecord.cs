namespace BankBills.Models;

public record TransactionRecord(
    Guid Id,
    DateOnly Date,
    double Amount,
    string Type,
    string Bank,
    Guid TitleId,
    string TitleName
);