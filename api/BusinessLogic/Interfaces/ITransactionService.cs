using BusinessLogic.Models;

namespace BusinessLogic.Interfaces
{
    public interface ITransactionService
    {
        Task CreateTransactionAsync(int userId, CreateTransactionRequest request);
        Task<List<TransactionResponseDto>> GetTransactionsAsync(int userId, GetTransactionsQuery query);
    }
}