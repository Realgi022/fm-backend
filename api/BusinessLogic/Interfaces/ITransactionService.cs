using BusinessLogic.Models;

namespace BusinessLogic.Interfaces
{
    public interface ITransactionService
    {
        Task<TransactionResponse> CreateTransactionAsync(int userId, CreateTransactionRequest request);
        Task<List<TransactionResponse>> GetTransactionsAsync(int userId, GetTransactionsQuery query);
        Task<TransactionResponse> UpdateTransactionAsync(int transactionId, int userId, UpdateTransactionRequest request);
        Task DeleteTransactionAsync(int transactionId, int userId);
    }
}