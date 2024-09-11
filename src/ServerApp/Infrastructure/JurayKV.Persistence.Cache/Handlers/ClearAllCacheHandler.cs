using JurayKV.Application.Caching.Handlers;
using JurayKV.Domain.Aggregates.BucketAggregate;
using JurayKV.Domain.Aggregates.CompanyAggregate;
using JurayKV.Domain.Aggregates.ExchangeRateAggregate;
using JurayKV.Domain.Aggregates.IdentityActivityAggregate;
using JurayKV.Domain.Aggregates.IdentityAggregate;
using JurayKV.Domain.Aggregates.NotificationAggregate;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Persistence.Cache.Handlers
{
    public class ClearAllCacheHandler : IClearAllCacheHandler
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IBucketCacheHandler _bucketCacheHandler;
        private readonly ICompanyCacheHandler _companyCacheHandler;
        private readonly IExchangeRateCacheHandler _exchangeRateCacheHandler;
        private readonly IIdentityActivityCacheHandler _identityActivityCacheHandler;
        private readonly IIdentityKvAdCacheHandler _departmentCacheHandler;
        private readonly IKvAdCacheHandler _kvAdCacheHandler;
        private readonly INotificationCacheHandler _notificationCacheHandler;
        private readonly ISliderCacheHandler _sliderCacheHandler;
        private readonly ITransactionCacheHandler _transactionCacheHandler;
        private readonly IUserManagerCacheHandler _userManagerCacheHandler;
        private readonly IWalletCacheHandler _walletCacheHandler;

        public ClearAllCacheHandler(IDistributedCache distributedCache,
            IBucketCacheHandler bucketCacheHandler, ICompanyCacheHandler companyCacheHandler,
            IExchangeRateCacheHandler exchangeRateCacheHandler, IIdentityActivityCacheHandler identityActivityCacheHandler, IIdentityKvAdCacheHandler departmentCacheHandler, IKvAdCacheHandler kvAdCacheHandler, INotificationCacheHandler notificationCacheHandler, ISliderCacheHandler sliderCacheHandler, ITransactionCacheHandler transactionCacheHandler, IUserManagerCacheHandler userManagerCacheHandler, IWalletCacheHandler walletCacheHandler)
        {
            _distributedCache = distributedCache;
            _bucketCacheHandler = bucketCacheHandler;
            _companyCacheHandler = companyCacheHandler;
            _exchangeRateCacheHandler = exchangeRateCacheHandler;
            _identityActivityCacheHandler = identityActivityCacheHandler;
            _departmentCacheHandler = departmentCacheHandler;
            _kvAdCacheHandler = kvAdCacheHandler;
            _notificationCacheHandler = notificationCacheHandler;
            _sliderCacheHandler = sliderCacheHandler;
            _transactionCacheHandler = transactionCacheHandler;
            _userManagerCacheHandler = userManagerCacheHandler;
            _walletCacheHandler = walletCacheHandler;
        }

        public async Task ClearCacheAsync()
        {

            await _bucketCacheHandler.RemoveListAsync();
            await _bucketCacheHandler.RemoveDropdownListAsync();
            //await _bucketCacheHandler.RemoveDetailsByIdAsync(bucket.Id);
            //await _bucketCacheHandler.RemoveGetByIdAsync(bucket.Id);


            await _companyCacheHandler.RemoveListAsync();
            await _companyCacheHandler.RemoveDropdownListAsync();
            //await _companyCacheHandler.RemoveGetAsync(company.Id);
            //await _companyCacheHandler.RemoveDetailsByIdAsync(company.Id);

            await _exchangeRateCacheHandler.RemoveListAsync();
            //await _exchangeRateCacheHandler.RemoveGetAsync(exchangeRate.Id);
            await _exchangeRateCacheHandler.RemoveLastListAsync();
            //await _exchangeRateCacheHandler.RemoveGetAsync(exchangeRate.Id);

            await _identityActivityCacheHandler.RemoveListAsync();
            //await _identityActivityCacheHandler.RemoveGetAsync(identityActivity.Id);
            //await _identityActivityCacheHandler.RemoveGetAsync(identityActivity.Id);


            await _departmentCacheHandler.RemoveListAsync();
            await _departmentCacheHandler.RemoveListActiveTodayAsync();
            //await _departmentCacheHandler.RemoveGetByUserIdAsync(create.UserId);
            //await _departmentCacheHandler.RemoveGetActiveByUserIdAsync(create.UserId);
            //await _departmentCacheHandler.RemoveDetailsByIdAsync(create.Id);
            //await _departmentCacheHandler.RemoveGetAsync(create.Id);


            await _kvAdCacheHandler.RemoveListAsync();
            //await _kvAdCacheHandler.RemoveDetailsByIdAsync(create.Id);
            //await _kvAdCacheHandler.RemoveByBucketIdAsync(create.BucketId);
            //await _kvAdCacheHandler.RemoveGetAsync(create.Id);


            await _notificationCacheHandler.RemoveListAsync();
            //await _notificationCacheHandler.RemoveDetailsByIdAsync(notification.Id);
            //await _notificationCacheHandler.RemoveGetAsync(notification.Id);

            await _sliderCacheHandler.RemoveListAsync();


            await _transactionCacheHandler.RemoveListAsync();
            //await _transactionCacheHandler.RemoveGetAsync(transaction.Id);
            //await _transactionCacheHandler.RemoveDetailsByIdAsync(transaction.Id);
            //await _transactionCacheHandler.RemoveList10ByUserAsync(transaction.UserId);

             await _walletCacheHandler.RemoveListAsync();
            await _userManagerCacheHandler.RemoveListAsync();
            //await _userManagerCacheHandler.RemoveDetailsByIdAsync(applicationUser.Id);
        }

     }
}
