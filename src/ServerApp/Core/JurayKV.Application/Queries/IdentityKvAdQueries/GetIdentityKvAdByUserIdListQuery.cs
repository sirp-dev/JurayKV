using JurayKV.Application.Caching.Repositories;
using MediatR;
using System.Linq;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Application.Queries.IdentityKvAdQueries;

public sealed class GetIdentityKvAdByUserIdListQuery : IRequest<List<IdentityKvAdListDto>>
{
    public GetIdentityKvAdByUserIdListQuery(Guid userId)
    {
        UserId = userId.ThrowIfEmpty(nameof(userId));
    }

    public Guid UserId { get; }

    public class GetIdentityKvAdByUserIdListQueryHandler : IRequestHandler<GetIdentityKvAdByUserIdListQuery, List<IdentityKvAdListDto>>
    {
        private readonly IIdentityKvAdCacheRepository _departmentCacheRepository;

        public GetIdentityKvAdByUserIdListQueryHandler(IIdentityKvAdCacheRepository departmentCacheRepository)
        {
            _departmentCacheRepository = departmentCacheRepository;
        }

        public async Task<List<IdentityKvAdListDto>> Handle(GetIdentityKvAdByUserIdListQuery request, CancellationToken cancellationToken)
        {
            request.ThrowIfNull(nameof(request));

            List<IdentityKvAdListDto> departmentDtos = await _departmentCacheRepository.GetByUserIdAsync(request.UserId);
            return departmentDtos.OrderByDescending(x=>x.CreatedAtUtc).ToList();
        }
    }
}

