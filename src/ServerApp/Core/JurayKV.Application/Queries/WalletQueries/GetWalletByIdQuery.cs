using System.Linq.Expressions;
using JurayKV.Application.Caching.Repositories;
using JurayKV.Domain.Aggregates.WalletAggregate;
using MediatR;
using TanvirArjel.ArgumentChecker;
using TanvirArjel.EFCore.GenericRepository;

namespace JurayKV.Application.Queries.WalletQueries;

public sealed class GetWalletByIdQuery : IRequest<WalletDetailsDto>
{
    public GetWalletByIdQuery(Guid id)
    {
        Id = id.ThrowIfEmpty(nameof(id));
    }

    public Guid Id { get; }

    // Handler
    private class GetWalletByIdQueryHandler : IRequestHandler<GetWalletByIdQuery, WalletDetailsDto>
    {
        private readonly IWalletCacheRepository _walletCacheRepository;

        public GetWalletByIdQueryHandler(IQueryRepository repository, IWalletCacheRepository walletCacheRepository)
        {
            _walletCacheRepository = walletCacheRepository;
        }

        public async Task<WalletDetailsDto> Handle(GetWalletByIdQuery request, CancellationToken cancellationToken)
        {
            request.ThrowIfNull(nameof(request));
 
            WalletDetailsDto walletDetailsDto = await _walletCacheRepository.GetByIdAsync(request.Id);

            return walletDetailsDto;
        }
    }
}

