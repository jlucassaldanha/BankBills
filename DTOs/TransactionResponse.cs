namespace BankBills.DTOs;

public record TransactionResponse(
    Guid Id,
    DateOnly Date,
    double Amount,
    string Type,
    string Bank,
    string TitleName
);