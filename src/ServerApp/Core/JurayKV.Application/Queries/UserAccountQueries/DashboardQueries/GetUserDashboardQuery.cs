using JurayKV.Application.Caching.Repositories;
using JurayKV.Application.Queries.IdentityKvAdQueries;
using JurayKV.Application.Queries.KvPointQueries;
using JurayKV.Application.Queries.TransactionQueries;
using JurayKV.Domain.Aggregates.EmployeeAggregate;
using JurayKV.Domain.Aggregates.TransactionAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TanvirArjel.ArgumentChecker;
using TanvirArjel.EFCore.GenericRepository;

namespace JurayKV.Application.Queries.UserAccountQueries.DashboardQueries
{

    public sealed class GetUserDashboardQuery : IRequest<UserDashboardDto>
    {
        public GetUserDashboardQuery(Guid userId)
        {
            UserId = userId.ThrowIfEmpty(nameof(userId));
        }

        public Guid UserId { get; }

        // Handler
        public class GetUserDashboardQueryHandler : IRequestHandler<GetUserDashboardQuery, UserDashboardDto>
        {
            private readonly IMediator _mediator;
            private readonly IUserManagerCacheRepository _userManagerCacheRepository;
            private readonly IKvPointCacheRepository _kvPointCacheRepository;
            private readonly IKvAdCacheRepository _kvAdCacheRepository;
            private readonly ITransactionCacheRepository _transactionCacheRepository;
            private readonly IWalletCacheRepository _walletCacheRepository;
            private readonly IExchangeRateCacheRepository _exchangeRateCacheRepository;
            private readonly IIdentityKvAdCacheRepository _identityKvAdCacheRepository;
            public GetUserDashboardQueryHandler(IUserManagerCacheRepository userManagerCacheRepository,
                IKvPointCacheRepository kvPointCacheRepository, IKvAdCacheRepository kvAdCacheRepository,
                ITransactionCacheRepository transactionCacheRepository, IWalletCacheRepository walletCacheRepository,
                IExchangeRateCacheRepository exchangeRateCacheRepository, IIdentityKvAdCacheRepository identityKvAdCacheRepository, IMediator mediator)
            {
                _userManagerCacheRepository = userManagerCacheRepository;
                _kvPointCacheRepository = kvPointCacheRepository;
                _kvAdCacheRepository = kvAdCacheRepository;
                _transactionCacheRepository = transactionCacheRepository;
                _walletCacheRepository = walletCacheRepository;
                _exchangeRateCacheRepository = exchangeRateCacheRepository;
                _identityKvAdCacheRepository = identityKvAdCacheRepository;
                _mediator = mediator;
            }

            public async Task<UserDashboardDto> Handle(GetUserDashboardQuery request, CancellationToken cancellationToken)
            {
                request.ThrowIfNull(nameof(request));
                UserDashboardDto userdto = new UserDashboardDto();
                var user = await _userManagerCacheRepository.GetByIdAsync(request.UserId);
                if (user == null)
                {
                    return null;
                }
                userdto.Fullname = user.Fullname;
                if (user.IsCSARole)
                {
                    userdto.Status = "Client Support Assistants (CSAs)";
                }
                else
                {
                    userdto.Status = "Social Media Assistants (SMAs)";
                    userdto.Upgrade = true;
                }

                var latestExchange = await _exchangeRateCacheRepository.GetByLatestAsync();
                if (latestExchange != null)
                {
                    userdto.ExchangeRate = latestExchange.Amount;

                }
                var wallet = await _walletCacheRepository.GetByUserIdAsync(request.UserId);
                userdto.Points = wallet.Amount;
                var userads = await _identityKvAdCacheRepository.GetActiveByUserIdAsync(request.UserId);
                userdto.AdsRunning = userads.Count();
                userdto.ListRunningAds = userads.Select(d => new ListRunningAds
                {
                    Image = d.ImageUrl,
                    AdsId = d.Id
                }).ToList();

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
                    TransactionType = x.TransactionType,
                    Note = x.UniqueReference,
                }).ToList();

                userdto.TransactionsCount = await _transactionCacheRepository.TransactionCount(request.UserId);

                GetIdentityKvAdByUserIdListQuery command = new GetIdentityKvAdByUserIdListQuery(request.UserId);

                var adcounts = await _mediator.Send(command);

                userdto.AdsCount = adcounts.Count();
                return userdto;
            }
        }
    }


}
