using JurayKV.Application.Caching.Repositories;
using JurayKV.Application.Queries.CompanyQueries;
using JurayKV.Application.Queries.WalletQueries;
using JurayKV.Domain.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Application.Queries.UserAccountQueries.DashboardQueries
{
    public sealed class GetClientDashboardQuery : IRequest<UserDashboardDto>
    {
        public GetClientDashboardQuery(Guid userId)
        {
            UserId = userId.ThrowIfEmpty(nameof(userId));
        }

        public Guid UserId { get; }

        // Handler
        private class GetClientDashboardQueryHandler : IRequestHandler<GetClientDashboardQuery, UserDashboardDto>
        {
            private readonly ICompanyCacheRepository _companyCacheRepository;
            private readonly IMediator _mediator;

            private readonly IUserManagerCacheRepository _userManagerCacheRepository;
            private readonly IKvPointCacheRepository _kvPointCacheRepository;
            private readonly IKvAdCacheRepository _kvAdCacheRepository;
            private readonly ITransactionCacheRepository _transactionCacheRepository;
            private readonly IWalletCacheRepository _walletCacheRepository;
            private readonly IExchangeRateCacheRepository _exchangeRateCacheRepository;
            private readonly IIdentityKvAdCacheRepository _identityKvAdCacheRepository;
            public GetClientDashboardQueryHandler(IUserManagerCacheRepository userManagerCacheRepository,
                IKvPointCacheRepository kvPointCacheRepository, IKvAdCacheRepository kvAdCacheRepository,
                ITransactionCacheRepository transactionCacheRepository, IWalletCacheRepository walletCacheRepository,
                IExchangeRateCacheRepository exchangeRateCacheRepository, IIdentityKvAdCacheRepository identityKvAdCacheRepository, ICompanyCacheRepository companyCacheRepository, IMediator mediator)
            {
                _userManagerCacheRepository = userManagerCacheRepository;
                _kvPointCacheRepository = kvPointCacheRepository;
                _kvAdCacheRepository = kvAdCacheRepository;
                _transactionCacheRepository = transactionCacheRepository;
                _walletCacheRepository = walletCacheRepository;
                _exchangeRateCacheRepository = exchangeRateCacheRepository;
                _identityKvAdCacheRepository = identityKvAdCacheRepository;
                _companyCacheRepository = companyCacheRepository;
                _mediator = mediator;
            }

            public async Task<UserDashboardDto> Handle(GetClientDashboardQuery request, CancellationToken cancellationToken)
            {
                request.ThrowIfNull(nameof(request));
                DateTime currentDate = DateForSix.GetTheDateBySix(DateTime.UtcNow.AddHours(1));
                UserDashboardDto userdto = new UserDashboardDto();
                var user = await _userManagerCacheRepository.GetByIdAsync(request.UserId);
                if (user == null)
                {
                    return null;
                }
                userdto.Fullname = user.Fullname;
                userdto.Passport = user.PassportUrl;
                //get company by userid
                var company = await _companyCacheRepository.GetByUserIdAsync(request.UserId);
                userdto.Company = company.Name;

                var wallet = await _walletCacheRepository.GetByUserIdAsync(request.UserId);
                userdto.Balance = wallet.Amount;
                var companyadsactive = await _kvAdCacheRepository.GetActiveListAllBucketByCompanyAsync(currentDate, company.Id);
                userdto.AdsRunning = companyadsactive.Count();

                var companyads = await _kvAdCacheRepository.GetListAllBucketByCompanyAsync(company.Id);
                userdto.LastTenAds = companyads.Select(d => new LastTenAds
                {
                    Image = d.ImageUrl,
                    ImageKey = d.ImageKey,
                    Id = d.Id,
                    Views = d.IdentityKvAdListDtos.Sum(x => x.Points),
                    CreatedAtUtc = d.CreatedAtUtc
                }).OrderByDescending(x => x.CreatedAtUtc).Take(10).ToList();
                userdto.TotalViews = companyads
    .SelectMany(x => x.IdentityKvAdListDtos) // Flatten the nested lists
    .Sum(x => x.Points);
                var lastTenPoints = await _kvPointCacheRepository.GetListByCountAsync(10, request.UserId);

                userdto.LastTenPoints = lastTenPoints.Select(x => new LastTenPoints
                {
                    Id = x.Id,
                    Status = x.Status,
                    Point = x.Point,
                    CreatedAtUtc = x.CreatedAtUtc
                }).ToList();

                var lastTenTransactions = await _transactionCacheRepository.GetListByCountAsync(10, request.UserId);

                userdto.LastTenTransactions = lastTenTransactions.Select(x => new LastTenTransactions
                {
                    Id = x.Id,
                    Status = x.Status,
                    Amount = x.Amount,
                    CreatedAtUtc = x.CreatedAtUtc,
                    TransactionType = x.TransactionType
                }).ToList();

                userdto.TransactionsCount = await _transactionCacheRepository.TransactionCount(request.UserId);
                userdto.AdsCount = await _identityKvAdCacheRepository.AdsCount(request.UserId);
                return userdto;
            }
        }
    }

}
