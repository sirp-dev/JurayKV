using JurayKV.Application.Caching.Repositories;
using MediatR;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Application.Queries.WalletQueries;

public sealed class GetWalletListQuery : IRequest<List<WalletDetailsDto>>
{
    private class GetWalletListQueryHandler : IRequestHandler<GetWalletListQuery, List<WalletDetailsDto>>
    {
        private readonly IWalletCacheRepository _walletCacheRepository;

        public GetWalletListQueryHandler(IWalletCacheRepository walletCacheRepository)
        {
            _walletCacheRepository = walletCacheRepository;
        }

        public async Task<List<WalletDetailsDto>> Handle(GetWalletListQuery request, CancellationToken cancellationToken)
        {
            request.ThrowIfNull(nameof(request));

            List<WalletDetailsDto> walletDtos = await _walletCacheRepository.GetListAsync();
            return walletDtos;
        }
    }
}

