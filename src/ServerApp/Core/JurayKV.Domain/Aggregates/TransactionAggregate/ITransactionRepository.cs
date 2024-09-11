using JurayKV.Domain.Aggregates.TransactionAggregate;
using JurayKV.Domain.Aggregates.NotificationAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JurayKV.Domain.Aggregates.KvPointAggregate;

namespace JurayKV.Domain.Aggregates.TransactionAggregate
{
    public interface ITransactionRepository
    {
        Task<Transaction> GetByIdAsync(Guid transactionId);

        Task InsertAsync(Transaction transaction);
        Task<Guid> InsertReturnIdAsync(Transaction transaction);
        Task UpdateAsync(Transaction transaction);

        Task DeleteAsync(Transaction transaction);
        Task<bool> CheckTransactionAboveTieOne(string uniqueId, Guid userId);
        Task<List<Transaction>> LastListByCountByUserId(int toplistcount, Guid userId);
        Task<List<Transaction>> GetListByUserId(Guid userId);
        Task<List<Transaction>> GetListByUserId(Guid userId, int count);
        Task<List<Transaction>> GetReferralListByUserId(Guid userId);
        Task<int> TransactionCount(Guid userId);

        Task<List<ListTransactionUserDto>> GetListUserTransactions(int pageSize, int pageNumber, string? searchString, int sortOrder);
        Task<ListTransactionUserDto> GetUserTransactionsSummary();


    }
}
