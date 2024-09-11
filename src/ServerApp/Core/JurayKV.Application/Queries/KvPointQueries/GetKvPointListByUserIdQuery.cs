using JurayKV.Application.Caching.Repositories;
using MediatR;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Application.Queries.KvPointQueries;

public sealed class GetKvPointListByUserIdQuery : IRequest<List<KvPointListDto>>
{
    public GetKvPointListByUserIdQuery(Guid userId)
    {
        UserId = userId.ThrowIfEmpty(nameof(userId));
    }

    public Guid UserId { get; }
    public class GetKvPointListByUserIdQueryHandler : IRequestHandler<GetKvPointListByUserIdQuery, List<KvPointListDto>>
    {
        private readonly IKvPointCacheRepository _kvPointCacheRepository;

        public GetKvPointListByUserIdQueryHandler(IKvPointCacheRepository kvPointCacheRepository)
        {
            _kvPointCacheRepository = kvPointCacheRepository;
        }

        public async Task<List<KvPointListDto>> Handle(GetKvPointListByUserIdQuery request, CancellationToken cancellationToken)
        {
            request.ThrowIfNull(nameof(request));

            List<KvPointListDto> kvPointDtos = await _kvPointCacheRepository.GetListByUserIdAsync(request.UserId);
            return kvPointDtos;
        }
    }
}

