using System;
using System.Threading.Tasks;

namespace JurayKV.Domain.Aggregates.WalletAggregate
{
    public interface IWalletRepository
    {
        Task<Wallet> GetByIdAsync(Guid walletId);
        Task<Wallet> GetByUserIdAsync(Guid userId);

        Task InsertAsync(Wallet wallet);

        Task UpdateAsync(Wallet wallet);

        Task DeleteAsync(Wallet wallet);

        Task<bool> CheckPhoneUnique(string phone);
        Task<bool> CheckEmailUnique(string email);
        Task LogUserAsync(string log, string authorizerUserEmail, Guid userId);
    }
}
