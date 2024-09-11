using JurayKV.Application.Caching.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanvirArjel.ArgumentChecker;
using TanvirArjel.EFCore.GenericRepository;

namespace JurayKV.Application.Queries.SettingQueries
{ 
    public sealed class GetSettingDefaultQuery : IRequest<SettingDetailsDto>
    {
        

        // Handler
        public class GetSettingDefaultQueryHandler : IRequestHandler<GetSettingDefaultQuery, SettingDetailsDto>
        {
            private readonly IQueryRepository _repository;
            private readonly ISettingCacheRepository _settingCacheRepository;

            public GetSettingDefaultQueryHandler(IQueryRepository repository, ISettingCacheRepository settingCacheRepository)
            {
                _repository = repository;
                _settingCacheRepository = settingCacheRepository;
            }

            public async Task<SettingDetailsDto> Handle(GetSettingDefaultQuery request, CancellationToken cancellationToken)
            {
                request.ThrowIfNull(nameof(request));


                SettingDetailsDto settingDetailsDto = await _settingCacheRepository.GetSettingAsync();

                return settingDetailsDto;
            }
        }
    }

}
