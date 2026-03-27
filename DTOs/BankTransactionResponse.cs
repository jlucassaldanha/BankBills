namespace BankBills.DTOs;

public record BankTransactionResponse(
    Guid Id,
    DateOnly Date,
    double Amount,
    string Type,
    string Bank,
    string TitleName
);