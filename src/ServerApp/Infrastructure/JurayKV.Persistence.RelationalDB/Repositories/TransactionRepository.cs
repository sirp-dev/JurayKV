using JurayKV.Domain.Aggregates.TransactionAggregate;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TanvirArjel.ArgumentChecker;
using JurayKV.Domain.Aggregates.NotificationAggregate;
using JurayKV.Persistence.RelationalDB.Repositories.GenericRepositories;
using JurayKV.Domain.Aggregates.IdentityAggregate;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;

namespace JurayKV.Persistence.RelationalDB.Repositories
{
    internal sealed class TransactionRepository : GenericRepository<Transaction>, ITransactionRepository
    {
        private readonly JurayDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public TransactionRepository(JurayDbContext dbContext, UserManager<ApplicationUser> userManager) : base(dbContext)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public Task<bool> ExistsAsync(Expression<Func<Transaction, bool>> condition)
        {
            IQueryable<Transaction> queryable = _dbContext.Set<Transaction>();

            if (condition != null)
            {
                queryable = queryable.Where(condition);
            }

            return queryable.AnyAsync();
        }

        public async Task<Transaction> GetByIdAsync(Guid transactionId)
        {
            transactionId.ThrowIfEmpty(nameof(transactionId));

            Transaction transaction = await _dbContext.Set<Transaction>().Include(x => x.User).FirstOrDefaultAsync(x => x.Id == transactionId);
            return transaction;
        }

        public async Task InsertAsync(Transaction transaction)
        {
            transaction.ThrowIfNull(nameof(transaction));

            await _dbContext.AddAsync(transaction);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Guid> InsertReturnIdAsync(Transaction transaction)
        {
            transaction.ThrowIfNull(nameof(transaction));

            await _dbContext.AddAsync(transaction);
            await _dbContext.SaveChangesAsync();
            return transaction.Id;
        }

        public async Task UpdateAsync(Transaction transaction)
        {
            transaction.ThrowIfNull(nameof(transaction));

            EntityEntry<Transaction> trackedEntity = _dbContext.ChangeTracker.Entries<Transaction>()
                .FirstOrDefault(x => x.Entity == transaction);

            if (trackedEntity == null)
            {
                _dbContext.Update(transaction);
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Transaction transaction)
        {
            transaction.ThrowIfNull(nameof(transaction));

            _dbContext.Remove(transaction);
            await _dbContext.SaveChangesAsync();
        }
        public async Task<bool> CheckTransactionAboveTieOne(string uniqueId, Guid userId)
        {
            var setting = await _dbContext.Settings.FirstOrDefaultAsync();
            var checktransaction = await _dbContext.Transactions.Where(x => x.UniqueReference.Contains(uniqueId) && x.UserId == userId).SumAsync(x => x.Amount);
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user.Tier == Domain.Primitives.Enum.Tier.Tier2)
            {
                return false;
            }
            if (checktransaction > setting.AirtimeMaxRechargeTieOne)
            {
                return true;
            }
            return false;
        }
        public async Task<List<Transaction>> LastListByCountByUserId(int toplistcount, Guid userId)
        {
            var list = await _dbContext.Transactions
                .Where(x => x.UserId == userId).OrderByDescending(x => x.CreatedAtUtc)
                .Take(toplistcount).ToListAsync();
            return list;
        }

        public async Task<int> TransactionCount(Guid userId)
        {
            return await _dbContext.Transactions.Where(x => x.UserId == userId).CountAsync();
        }
        public async Task<List<Transaction>> GetListByUserId(Guid userId, int count)
        {
            var list = await _dbContext.Transactions
                .Include(x => x.User)
                .Include(x => x.Wallet)
                 .Where(x => x.UserId == userId).OrderByDescending(x => x.CreatedAtUtc).Take(count)
                  .ToListAsync();
            return list;
        }
        public async Task<List<Transaction>> GetListByUserId(Guid userId)
        {
            var list = await _dbContext.Transactions
                .Include(x => x.User)
                .Include(x => x.Wallet)
                 .Where(x => x.UserId == userId).OrderByDescending(x => x.CreatedAtUtc)
                  .ToListAsync();
            return list;
        }

        public async Task<List<Transaction>> GetReferralListByUserId(Guid userId)
        {
            var list = await _dbContext.Transactions
                .Include(x => x.User)
                .Include(x => x.Wallet)
                 .Where(x => x.UserId == userId && x.Description.ToLower().Contains("Referral Bonus")).OrderByDescending(x => x.CreatedAtUtc)
                  .ToListAsync();
            return list;
        }



        public async Task<List<ListTransactionUserDto>> GetListUserTransactions(int pageSize, int pageNumber, string searchString, int sortOrder)
        {
            var query = _userManager.Users.AsQueryable();

            // Apply search filter if searchString is provided
            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(u =>
                    u.Email.Contains(searchString) ||
                    u.FirstName.Contains(searchString) ||
                    u.LastName.Contains(searchString) ||
                    u.SurName.Contains(searchString) ||
                    u.PhoneNumber.Contains(searchString));
            }

            // Apply sorting

            //
            var userTransactions = _dbContext.Transactions
                                 .AsQueryable();
            var userPoints = _dbContext.kvPoints
                            .AsQueryable();
            var walletBalance = await _dbContext.Wallets
                                .SumAsync(t => t.Amount);
            int queryCount = query.Count();
            // Apply pagination
            query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            var result = await query.Select(u => new ListTransactionUserDto
            {
                UserId = u.Id.ToString(),
                Email = u.Email,
                Name = u.SurName + " " + u.FirstName + " " + u.LastName + " ",
                Phone = u.PhoneNumber,
                TotalPoints = userPoints.Where(p => p.UserId == u.Id).Sum(p => p.Point),
                TotalReferrals = userTransactions.Where(p => p.UserId == u.Id && p.TransactionType == Domain.Primitives.Enum.TransactionTypeEnum.Credit).Sum(p => p.Amount),
                TotalDebit = userTransactions.Where(p => p.UserId == u.Id && p.TransactionType == Domain.Primitives.Enum.TransactionTypeEnum.Debit).Sum(p => p.Amount),
                WalletBalance = _dbContext.Wallets.FirstOrDefault(t => t.UserId == u.Id).Amount,
                TotalInList = queryCount
            }).ToListAsync();


            switch (sortOrder)
            {
                case 1:
                    result = result.OrderBy(u => u.Email).ToList();
                    break;
                case 2:
                    result = result.OrderBy(u => u.Phone).ToList();
                    break;
                case 3:
                    result = result.OrderBy(u => u.TotalPoints).ToList();
                    break;
                case 4:
                    result = result.OrderBy(u => u.TotalReferrals).ToList();
                    break;
                case 5:
                    result = result.OrderBy(u => u.TotalDebit).ToList();
                    break;
                case 6:
                    result = result.OrderBy(u => u.WalletBalance).ToList();
                    break;
                // Implement sorting logic based on your requirements
                default:
                    result = result.OrderBy(u => u.Email).ToList();
                    break;
            }


            return result;
        }

        public async Task<ListTransactionUserDto> GetUserTransactionsSummary()
        {

            var result = new ListTransactionUserDto
            {
                TotalPoints = _dbContext.kvPoints.Sum(p => p.Point),
                TotalReferrals = _dbContext.Transactions.Where(p => p.TransactionType == Domain.Primitives.Enum.TransactionTypeEnum.Credit).Sum(p => p.Amount),
                TotalDebit = _dbContext.Transactions.Where(p => p.TransactionType == Domain.Primitives.Enum.TransactionTypeEnum.Debit).Sum(p => p.Amount),
                WalletBalance = _dbContext.Wallets.Sum(x => x.Amount)
            };

            return result;
        }
    }

}
