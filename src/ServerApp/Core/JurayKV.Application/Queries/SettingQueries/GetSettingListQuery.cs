using JurayKV.Application.Caching.Repositories;
using MediatR;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Application.Queries.SettingQueries;

public sealed class GetSettingListQuery : IRequest<List<SettingDetailsDto>>
{
    private class GetSettingListQueryHandler : IRequestHandler<GetSettingListQuery, List<SettingDetailsDto>>
    {
        private readonly ISettingCacheRepository _settingCacheRepository;

        public GetSettingListQueryHandler(ISettingCacheRepository settingCacheRepository)
        {
            _settingCacheRepository = settingCacheRepository;
        }

        public async Task<List<SettingDetailsDto>> Handle(GetSettingListQuery request, CancellationToken cancellationToken)
        {
            request.ThrowIfNull(nameof(request));

            List<SettingDetailsDto> settingDtos = await _settingCacheRepository.GetListAsync();
            return settingDtos;
        }
    }
}
 