using JurayKV.Application.Caching.Repositories;
using JurayKV.Application.Queries.WalletQueries;
using JurayKV.Domain.Aggregates.IdentityAggregate;
using JurayKV.Domain.Aggregates.WalletAggregate;
using JurayKV.Persistence.Cache.Keys;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TanvirArjel.EFCore.GenericRepository;
using TanvirArjel.Extensions.Microsoft.Caching;

namespace JurayKV.Persistence.Cache.Repositories
{
    public sealed class WalletCacheRepository : IWalletCacheRepository
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IQueryRepository _repository;
        private readonly IWalletRepository _wallet;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWalletRepository _walletRepository;

        public WalletCacheRepository(IDistributedCache distributedCache, IQueryRepository repository, IWalletRepository wallet, UserManager<ApplicationUser> userManager, IWalletRepository walletRepository)
        {
            _distributedCache = distributedCache;
            _repository = repository;
            _wallet = wallet;
            _userManager = userManager;
            _walletRepository = walletRepository;
        }

        public async Task<List<WalletDetailsDto>> GetListAsync()
        {
            //string cacheKey = WalletCacheKeys.ListKey;
            //List<WalletDetailsDto> list = await _distributedCache.GetAsync<List<WalletDetailsDto>>(cacheKey);

            //if (list == null)
            //{
            Expression<Func<Wallet, WalletDetailsDto>> selectExp = d => new WalletDetailsDto
            {
                Id = d.Id,
                Amount = d.Amount,
                CreatedAtUtc = d.CreatedAtUtc,
                LastUpdateAtUtc = d.LastUpdateAtUtc,
                Log = d.Log,
                Note = d.Note,
                UserId = d.UserId,
                Fullname = d.User.SurName + " " + d.User.FirstName + " " + d.User.LastName,
            };

            var list = await _repository.GetListAsync(selectExp);

            //    await _distributedCache.SetAsync(cacheKey, list);
            //}

            return list;
        }

        public async Task<WalletDetailsDto> GetByIdAsync(Guid walletId)
        {
            //string cacheKey = WalletCacheKeys.GetKey(walletId);
            //WalletDetailsDto wallet = await _distributedCache.GetAsync<WalletDetailsDto>(cacheKey);

            //if (wallet == null)
            //{
            Expression<Func<Wallet, WalletDetailsDto>> selectExp = d => new WalletDetailsDto
            {
                Id = d.Id,
                Amount = d.Amount,
                CreatedAtUtc = d.CreatedAtUtc,
                LastUpdateAtUtc = d.LastUpdateAtUtc,
                Log = d.Log,
                Note = d.Note,
                UserId = d.UserId,
                Fullname = d.User.SurName + " " + d.User.FirstName + " " + d.User.LastName,
            };

            var wallet = await _repository.GetByIdAsync(walletId, selectExp);

            //    await _distributedCache.SetAsync(cacheKey, wallet);
            //}


            return wallet;
        }

        public async Task<WalletDetailsDto> GetDetailsByIdAsync(Guid walletId)
        {
            //string cacheKey = WalletCacheKeys.GetDetailsKey(walletId);
            //WalletDetailsDto wallet = await _distributedCache.GetAsync<WalletDetailsDto>(cacheKey);

            //if (wallet == null)
            //{
            Expression<Func<Wallet, WalletDetailsDto>> selectExp = d => new WalletDetailsDto
            {
                Id = d.Id,
                Amount = d.Amount,
                CreatedAtUtc = d.CreatedAtUtc,
                LastUpdateAtUtc = d.LastUpdateAtUtc,
                Log = d.Log,
                Note = d.Note,
                UserId = d.UserId,
                Fullname = d.User.SurName + " " + d.User.FirstName + " " + d.User.LastName,
            };

            var wallet = await _repository.GetByIdAsync(walletId, selectExp);

            //    await _distributedCache.SetAsync(cacheKey, wallet);
            //}

            return wallet;
        }

        public async Task<WalletDetailsDto> GetByUserIdAsync(Guid userId)
        {
            //string cacheKey = WalletCacheKeys.GetUserKey(userId);
            //WalletDetailsDto wallet = await _distributedCache.GetAsync<WalletDetailsDto>(cacheKey);

            //if (wallet == null)
            //{ 
            var userwallet = await _wallet.GetByUserIdAsync(userId);
            if (userwallet == null)
            {
                var user = await _userManager.FindByIdAsync(userId.ToString());
                Wallet wallet = new Wallet(Guid.NewGuid());
                wallet.UserId = user.Id;
                wallet.Note = "NEW WALLET";
                wallet.Log = "LOG";
                wallet.Amount = 0;
                // Persist to the database
                 await _walletRepository.InsertAsync(wallet);
                var getwallet = await _walletRepository.GetByUserIdAsync(user.Id);
                var Xwallet = new WalletDetailsDto
                {
                    Amount = getwallet.Amount,
                    UserId = getwallet.UserId,
                    Id = getwallet.Id,
                    Fullname = getwallet.User.FirstName,

                };
                return Xwallet;
            }
            else
            {
                var wallet = new WalletDetailsDto
                {
                    Amount = userwallet.Amount,
                    UserId = userId,
                    Id = userwallet.Id,
                    Fullname = userwallet.User.FirstName,

                };
                return wallet;
            }
            //    await _distributedCache.SetAsync(cacheKey, wallet);
            //}
             
        }
    }

}
