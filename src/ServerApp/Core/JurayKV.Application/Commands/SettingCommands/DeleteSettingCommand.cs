using JurayKV.Application.Caching.Handlers;
using JurayKV.Domain.Aggregates.SettingAggregate;
using JurayKV.Domain.Exceptions;
using MediatR;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Application.Commands.SettingCommands;

public sealed class DeleteSettingCommand : IRequest
{
    public DeleteSettingCommand(Guid settingId)
    {
        Id = settingId.ThrowIfEmpty(nameof(settingId));
    }

    public Guid Id { get; }
}

internal class DeleteSettingCommandHandler : IRequestHandler<DeleteSettingCommand>
{
    private readonly ISettingRepository _settingRepository;
    private readonly ISettingCacheHandler _settingCacheHandler;

    public DeleteSettingCommandHandler(ISettingRepository settingRepository, ISettingCacheHandler settingCacheHandler)
    {
        _settingRepository = settingRepository;
        _settingCacheHandler = settingCacheHandler;
    }

    public async Task Handle(DeleteSettingCommand request, CancellationToken cancellationToken)
    {
        _ = request.ThrowIfNull(nameof(request));

        Setting setting = await _settingRepository.GetByIdAsync(request.Id);

        if (setting == null)
        {
            throw new EntityNotFoundException(typeof(Setting), request.Id);
        }

        await _settingRepository.DeleteAsync(setting);
         
        await _settingCacheHandler.RemoveListAsync(); 
        await _settingCacheHandler.RemoveGetByIdAsync(setting.Id);
    }
}