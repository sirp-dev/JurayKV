using System.Linq.Expressions;
using JurayKV.Application.Caching.Repositories;
using JurayKV.Domain.Aggregates.SettingAggregate;
using MediatR;
using TanvirArjel.ArgumentChecker;
using TanvirArjel.EFCore.GenericRepository;

namespace JurayKV.Application.Queries.SettingQueries;

public sealed class GetSettingByIdQuery : IRequest<SettingDetailsDto>
{
    public GetSettingByIdQuery(Guid id)
    {
        Id = id.ThrowIfEmpty(nameof(id));
    }

    public Guid Id { get; }

    // Handler
    private class GetSettingByIdQueryHandler : IRequestHandler<GetSettingByIdQuery, SettingDetailsDto>
    {
        private readonly IQueryRepository _repository;
        private readonly ISettingCacheRepository _settingCacheRepository;

        public GetSettingByIdQueryHandler(IQueryRepository repository, ISettingCacheRepository settingCacheRepository)
        {
            _repository = repository;
            _settingCacheRepository = settingCacheRepository;
        }

        public async Task<SettingDetailsDto> Handle(GetSettingByIdQuery request, CancellationToken cancellationToken)
        {
            request.ThrowIfNull(nameof(request));
 
             
            SettingDetailsDto settingDetailsDto = await _settingCacheRepository.GetByIdAsync(request.Id);

            return settingDetailsDto;
        }
    }
}
