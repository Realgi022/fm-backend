using Azure;
using BusinessLogic.Exceptions;
using BusinessLogic.Interfaces;
using BusinessLogic.Mappers;
using BusinessLogic.Models;
using DAL.Entities;
using DAL.Enum;
using DAL.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;

namespace BusinessLogic.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITransactionRepository _transactionRepository;

        public TransactionService(
            IUserRepository userRepository,
            ITransactionRepository transactionRepository)
        {
            _userRepository = userRepository;
            _transactionRepository = transactionRepository;
        }

        public async Task<TransactionResponse> CreateTransactionAsync(int userId, CreateTransactionRequest request)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new InvalidCredentialsException();
            }

            TransactionType transactionType = request.Type.Trim().ToLower() switch
            {
                "income" => TransactionType.Income,
                "expense" => TransactionType.Expense,
                _ => throw new Exception("Invalid transaction type.")
            };

            if (!DateTime.TryParseExact(
            request.Date,
            "yyyy-MM-dd",
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out DateTime parsedDate))
            {
                throw new Exception("Date must be a valid date in format yyyy-MM-dd.");
            }

            var transaction = TransactionMapper.ToModel(request);
            transaction.UserId = userId;
            transaction.CreatedAt = DateTime.UtcNow;
            transaction.UpdatedAt = DateTime.UtcNow;

            var newTransaction = await _transactionRepository.CreateAsync(transaction);

            return TransactionMapper.ToResponse(newTransaction);
        }

        public async Task<List<TransactionResponse>> GetTransactionsAsync(int userId, GetTransactionsQuery query)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new InvalidCredentialsException();
            }

            TransactionType? transactionType = query.Type?.Trim().ToLower() switch
            {
                null => null,
                "" => null,
                "income" => TransactionType.Income,
                "expense" => TransactionType.Expense,
                _ => throw new Exception("Invalid transaction type.")
            };

            DateTime? startDate = null;
            DateTime? endDate = null;

            if (!string.IsNullOrWhiteSpace(query.Period))
            {
                var parts = query.Period.Split('-');
                int year = int.Parse(parts[0]);
                int month = int.Parse(parts[1]);

                startDate = new DateTime(year, month, 1);
                endDate = startDate.Value.AddMonths(1);
            }

            var transactions = await _transactionRepository.GetFilteredAsync(
                userId,
                transactionType,
                query.Category,
                startDate,
                endDate);


            var mappedTransactions = new List<TransactionResponse>();
            foreach (var transaction in transactions) 
            {
            }

            return mappedTransactions;
        }
        public async Task<TransactionResponse> UpdateTransactionAsync(int transactionId, int userId, UpdateTransactionRequest request)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new InvalidCredentialsException();
            }

            var transaction = await _transactionRepository.GetByIdAsync(transactionId, userId);

            if (transaction == null || transaction.UserId != userId)
                throw new Exception("Transaction not found.");

            TransactionType type = request.Type.Trim().ToLower() switch
            {
                "income" => TransactionType.Income,
                "expense" => TransactionType.Expense,
                _ => throw new Exception("Invalid transaction type.")
            };

            transaction.Type = type;
            transaction.Amount = request.Amount;
            transaction.Currency = request.Currency ?? transaction.Currency;
            transaction.Category = request.Category;
            transaction.Description = request.Description;
            transaction.UpdatedAt = DateTime.UtcNow;

            var updatedTransaction = await _transactionRepository.UpdateAsync(transaction);

        }
        public async Task DeleteTransactionAsync(int transactionId, int userId)
        {
            _transactionRepository.DeleteAsync(transactionId, userId).Wait();
        }
    }
}