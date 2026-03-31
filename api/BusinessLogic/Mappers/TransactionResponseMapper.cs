using DAL.Entities;
using DAL.Enum;
using BusinessLogic.Models;

namespace BusinessLogic.Mappers
{
    public class TransactionResponseMapper
    {
        public static Transaction ToTransaction(TransactionResponse response)
        {
            return new Transaction
            {
                Id = response.Id,
                Amount = response.Amount,
                Currency = response.Currency,
                Category = response.Category,
                Description = response.Description,
                Date = response.Date,
                CreatedAt = response.CreatedAt,
                UpdatedAt = response.UpdatedAt,

                // Convert string → enum
                Type = Enum.Parse<TransactionType>(response.Type)
            };
        }
        public static TransactionResponse ToResponse(Transaction transaction) 
        {
            return new TransactionResponse
            {
                Id = transaction.Id,
                Amount = transaction.Amount,
                Type = transaction.Type.ToString(),
                Currency = transaction.Currency,
                Category = transaction.Category,
                Description = transaction.Description,
                Date = transaction.Date,
                CreatedAt = transaction.CreatedAt,
                UpdatedAt = transaction.UpdatedAt
            };
        }
    }
}