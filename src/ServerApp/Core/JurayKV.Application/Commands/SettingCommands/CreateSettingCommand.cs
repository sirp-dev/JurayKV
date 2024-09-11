using JurayKV.Application.Caching.Handlers;
using JurayKV.Application.Infrastructures;
using JurayKV.Domain.Aggregates.SettingAggregate;
using JurayKV.Domain.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Http;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Application.Commands.SettingCommands;

public sealed class CreateSettingCommand : IRequest
{
    public CreateSettingCommand(Setting setting)
    {
        Setting = setting;
        
    }

    public Setting Setting { get; }
    
}

internal class CreateSettingCommandHandler : IRequestHandler<CreateSettingCommand>
{
    private readonly ISettingRepository _settingRepository;
    private readonly ISettingCacheHandler _settingCacheHandler;
    private readonly IStorageService _storage;

    public CreateSettingCommandHandler(
            ISettingRepository settingRepository,
            ISettingCacheHandler settingCacheHandler,
            IStorageService storage)
    {
        _settingRepository = settingRepository;
        _settingCacheHandler = settingCacheHandler;
        _storage = storage;
    }

    public async Task Handle(CreateSettingCommand request, CancellationToken cancellationToken)
    {
        _ = request.ThrowIfNull(nameof(request));
         
        

        await _settingRepository.Create(request.Setting);

        // Remove the cache
        await _settingCacheHandler.RemoveListAsync();

    }
}