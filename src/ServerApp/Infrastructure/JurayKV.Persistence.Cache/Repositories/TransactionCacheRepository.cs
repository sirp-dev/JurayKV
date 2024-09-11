using JurayKV.Application.Caching.Repositories;
using JurayKV.Application.Queries.IdentityKvAdQueries;
using JurayKV.Application.Queries.KvPointQueries;
using JurayKV.Application.Queries.TransactionQueries;
using JurayKV.Domain.Aggregates.KvPointAggregate;
using JurayKV.Domain.Aggregates.TransactionAggregate;
using JurayKV.Persistence.Cache.Keys;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TanvirArjel.EFCore.GenericRepository;
using TanvirArjel.Extensions.Microsoft.Caching;

namespace JurayKV.Persistence.Cache.Repositories
{
    public sealed class TransactionCacheRepository : ITransactionCacheRepository
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IQueryRepository _repository;
        private readonly ITransactionRepository _transactionRepository;
        public TransactionCacheRepository(IDistributedCache distributedCache, IQueryRepository repository, ITransactionRepository transactionRepository)
        {
            _distributedCache = distributedCache;
            _repository = repository;
            _transactionRepository = transactionRepository;
        }

        public async Task<List<TransactionListDto>> GetListAsync()
        {
            //string cacheKey = TransactionCacheKeys.ListKey;
            //List<TransactionListDto> list = await _distributedCache.GetAsync<List<TransactionListDto>>(cacheKey);

            //if (list == null)
            //{
            Expression<Func<Transaction, TransactionListDto>> selectExp = d => new TransactionListDto
            {
                Id = d.Id,
                Amount = d.Amount,
                Description = d.Description,
                UniqueReference = d.UniqueReference,
                OptionalNote = d.OptionalNote,
                Status = d.Status,
                TrackCode = d.TrackCode,
                TransactionReference = d.TransactionReference,
                TransactionType = d.TransactionType,
                UserId = d.UserId,
                Fullname = d.User.SurName + " " + d.User.FirstName + " " + d.User.LastName,
                WalletId = d.WalletId,
                WalletBalance = d.Wallet.Amount,
                CreatedAtUtc = d.CreatedAtUtc
            };

            var list = await _repository.GetListAsync(selectExp);

            //    await _distributedCache.SetAsync(cacheKey, list);
            //}

            return list.OrderByDescending(x => x.CreatedAtUtc).ToList();
        }

        public async Task<TransactionDetailsDto> GetByIdAsync(Guid transactionId)
        {
            //string cacheKey = TransactionCacheKeys.GetKey(transactionId);
            //TransactionDetailsDto transaction = await _distributedCache.GetAsync<TransactionDetailsDto>(cacheKey);

            //if (transaction == null)
            //{
            Expression<Func<Transaction, TransactionDetailsDto>> selectExp = d => new TransactionDetailsDto
            {
                Id = d.Id,
                Amount = d.Amount,
                Description = d.Description,
                UniqueReference = d.UniqueReference,
                OptionalNote = d.OptionalNote,
                Status = d.Status,
                TrackCode = d.TrackCode,
                TransactionReference = d.TransactionReference,
                TransactionType = d.TransactionType,
                UserId = d.UserId,
                WalletId = d.WalletId,
                CreatedAtUtc = d.CreatedAtUtc,
                TransactionVerificationId = d.TransactionVerificationId,

            };

            var transaction = await _repository.GetByIdAsync(transactionId, selectExp);

            //    await _distributedCache.SetAsync(cacheKey, transaction);
            //}

            return transaction;
        }

        public async Task<TransactionDetailsDto> GetDetailsByIdAsync(Guid transactionId)
        {
            //string cacheKey = TransactionCacheKeys.GetDetailsKey(transactionId);
            //TransactionDetailsDto transaction = await _distributedCache.GetAsync<TransactionDetailsDto>(cacheKey);

            //if (transaction == null)
            //{
            Expression<Func<Transaction, TransactionDetailsDto>> selectExp = d => new TransactionDetailsDto
            {
                Id = d.Id,
                Amount = d.Amount,
                Description = d.Description,
                UniqueReference = d.UniqueReference,
                OptionalNote = d.OptionalNote,
                Status = d.Status,
                TrackCode = d.TrackCode,
                TransactionReference = d.TransactionReference,
                TransactionType = d.TransactionType,
                UserId = d.UserId,
                WalletId = d.WalletId,
                CreatedAtUtc = d.CreatedAtUtc
            };

            var transaction = await _repository.GetByIdAsync(transactionId, selectExp);

            //    await _distributedCache.SetAsync(cacheKey, transaction);
            //}

            return transaction;
        }


        public async Task<List<TransactionListDto>> GetListByCountAsync(int toplistcount, Guid userId)
        {
            //string cacheKey = TransactionCacheKeys.ListByCountUserIdKey(userId);
            //List<TransactionListDto> list = await _distributedCache.GetAsync<List<TransactionListDto>>(cacheKey);

            //if (list == null)
            //{

            var xlist = await _transactionRepository.LastListByCountByUserId(toplistcount, userId);
            var list = xlist.Select(d => new TransactionListDto
            {
                Id = d.Id,
                Amount = d.Amount,
                Description = d.Description,
                UniqueReference = d.UniqueReference,
                OptionalNote = d.OptionalNote,
                Status = d.Status,
                TrackCode = d.TrackCode,
                TransactionReference = d.TransactionReference,
                TransactionType = d.TransactionType,
                UserId = d.UserId,
                Fullname = d.User.SurName + " " + d.User.SurName,
                WalletId = d.WalletId,
                WalletBalance = d.Wallet.Amount,
                CreatedAtUtc = d.CreatedAtUtc
            }).ToList();

            //    await _distributedCache.SetAsync(cacheKey, list);
            //}

            return list;
        }

        public Task<List<TransactionListDto>> GetListByCountAsync(int latestcount)
        {
            throw new NotImplementedException();
        }

        public async Task<int> TransactionCount(Guid userId)
        {
            return await _transactionRepository.TransactionCount(userId);
        }
        public async Task<List<TransactionListDto>> GetListByUserIdAsync(Guid userId, int count)
        {
            var mainlist = await _transactionRepository.GetListByUserId(userId, count);
            var list = mainlist.Select(d => new TransactionListDto
            {
                Id = d.Id,
                Amount = d.Amount,
                Description = d.Description,
                UniqueReference = d.UniqueReference,
                OptionalNote = d.OptionalNote,
                Status = d.Status,
                TrackCode = d.TrackCode,
                TransactionReference = d.TransactionReference,
                TransactionType = d.TransactionType,
                UserId = d.UserId,
                Fullname = d.User.SurName + " " + d.User.SurName,
                WalletId = d.WalletId,
                WalletBalance = d.Wallet.Amount,
                CreatedAtUtc = d.CreatedAtUtc
            }).ToList();

            return list;
        }
        public async Task<List<TransactionListDto>> GetListByUserIdAsync(Guid userId)
        {
            var mainlist = await _transactionRepository.GetListByUserId(userId);
            var list = mainlist.Select(d => new TransactionListDto
            {
                Id = d.Id,
                Amount = d.Amount,
                Description = d.Description,
                UniqueReference = d.UniqueReference,
                OptionalNote = d.OptionalNote,
                Status = d.Status,
                TrackCode = d.TrackCode,
                TransactionReference = d.TransactionReference,
                TransactionType = d.TransactionType,
                UserId = d.UserId,
                Fullname = d.User.SurName + " " + d.User.SurName,
                WalletId = d.WalletId,
                WalletBalance = d.Wallet.Amount,
                CreatedAtUtc = d.CreatedAtUtc
            }).ToList();

            return list;
        }

        public async Task<List<TransactionListDto>> GetReferralListByUserIdAsync(Guid userId)
        {
            var mainlist = await _transactionRepository.GetReferralListByUserId(userId);
            var list = mainlist.Select(d => new TransactionListDto
            {
                Id = d.Id,
                Amount = d.Amount,
                Description = d.Description,
                UniqueReference = d.UniqueReference,
                OptionalNote = d.OptionalNote,
                Status = d.Status,
                TrackCode = d.TrackCode,
                TransactionReference = d.TransactionReference,
                TransactionType = d.TransactionType,
                UserId = d.UserId,
                Fullname = d.User.SurName + " " + d.User.SurName,
                WalletId = d.WalletId,
                WalletBalance = d.Wallet.Amount,
                CreatedAtUtc = d.CreatedAtUtc
            }).ToList();

            return list;
        }

        public async Task<bool> CheckTransactionAboveTieOne(string uniqueId, Guid userId)
        {
            return await _transactionRepository.CheckTransactionAboveTieOne(uniqueId, userId);
        }

        public async Task<List<ListTransactionDto>> ListUserTransactions(int pageSize, int pageNumber, string searchString, int sortOrder)
        {

            var xlist = await _transactionRepository.GetListUserTransactions(pageSize, pageNumber, searchString, sortOrder);
            var list = xlist.Select(d => new ListTransactionDto
            {
                UserId = d.UserId,
                Email = d.Email,
                Name = d.Name,
                Phone = d.Phone,
                TotalPoints = d.TotalPoints,
                TotalReferrals = d.TotalReferrals,
                TotalDebit = d.TotalDebit,
                WalletBalance = d.WalletBalance,
                TotalInList = d.TotalInList,

            }).ToList();


            return list;
        }

        public async Task<ListTransactionDto> GetUserTransactionsSummary()
        {
            var d = await _transactionRepository.GetUserTransactionsSummary();
            var list = new ListTransactionDto
            {

                TotalPoints = d.TotalPoints,
                TotalReferrals = d.TotalReferrals,
                TotalDebit = d.TotalDebit,
                WalletBalance = d.WalletBalance,
                TotalInList = d.TotalInList,

            };

            return list;
        }
    }

}
