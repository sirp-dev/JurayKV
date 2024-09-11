using JurayKV.Application.Caching.Handlers;
using JurayKV.Application.Caching.Repositories;
using JurayKV.Application.Infrastructures;
using JurayKV.Domain.Aggregates.IdentityAggregate;
using JurayKV.Domain.Aggregates.IdentityKvAdAggregate;
using JurayKV.Domain.Exceptions;
using JurayKV.Domain.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Application.Commands.IdentityKvAdCommands;

public sealed class UpdateIdentityKvAdCommand : IRequest
{
    public UpdateIdentityKvAdCommand(
        Guid id,
        IFormFile videoFile)
    {
        Id = id;
        VideoFile = videoFile;
    }

    public Guid Id { get; }

    public IFormFile VideoFile { get; set; }
}

internal class UpdateIdentityKvAdCommandHandler : IRequestHandler<UpdateIdentityKvAdCommand>
{
    private readonly IIdentityKvAdRepository _identityKvAdRepository;
    private readonly IIdentityKvAdCacheHandler _identityKvAdCacheHandler;
    private readonly IStorageService _storage;
    private readonly IIdentityKvAdCacheRepository _kvAdCacheRepository;
    private readonly UserManager<ApplicationUser> _userManager;

    public UpdateIdentityKvAdCommandHandler(
        IIdentityKvAdRepository identityKvAdRepository,
        IIdentityKvAdCacheHandler identityKvAdCacheHandler,
        IStorageService storage,
        IIdentityKvAdCacheRepository kvAdCacheRepository,
        UserManager<ApplicationUser> userManager)
    {
        _identityKvAdRepository = identityKvAdRepository;
        _identityKvAdCacheHandler = identityKvAdCacheHandler;
        _storage = storage;
        _kvAdCacheRepository = kvAdCacheRepository;
        _userManager = userManager;
    }

    public async Task Handle(UpdateIdentityKvAdCommand request, CancellationToken cancellationToken)
    {
        request.ThrowIfNull(nameof(request));

        IdentityKvAd identityKvAdToBeUpdated = await _identityKvAdRepository.GetByIdAsync(request.Id);

        if (identityKvAdToBeUpdated == null)
        {
            throw new EntityNotFoundException(typeof(IdentityKvAd), request.Id);
        }

        //run the video upload;
        var videourl = String.Empty;
        var videokey = String.Empty;
        try
        {

            var xresult = await _storage.MainUploadFileReturnUrlAsync(identityKvAdToBeUpdated.VideoKey, request.VideoFile);
            // 
            if (xresult.Message.Contains("200"))
            {
                
                identityKvAdToBeUpdated.VideoUrl = xresult.Url;
                identityKvAdToBeUpdated.VideoKey = xresult.Key;
            }

        }
        catch (Exception c)
        {

        }
       

        identityKvAdToBeUpdated.LastModifiedAtUtc = DateTime.UtcNow;

        await _identityKvAdRepository.UpdateAsync(identityKvAdToBeUpdated);
        //
        var check = await _kvAdCacheRepository.CheckVideoIdnetityKvIdFirstTime(identityKvAdToBeUpdated.UserId);
        if (check == true)
        {
            var userUpdate = await _userManager.FindByIdAsync(identityKvAdToBeUpdated.UserId.ToString());
            if (userUpdate != null)
            {
                userUpdate.VideoUpload = true;
                await _userManager.UpdateAsync(userUpdate);
            }
        }
        // Remove the cache
        await _identityKvAdCacheHandler.RemoveListAsync();
        await _identityKvAdCacheHandler.RemoveGetByUserIdAsync(identityKvAdToBeUpdated.UserId);
        await _identityKvAdCacheHandler.RemoveGetActiveByUserIdAsync(identityKvAdToBeUpdated.UserId);
        await _identityKvAdCacheHandler.RemoveDetailsByIdAsync(identityKvAdToBeUpdated.Id);
        await _identityKvAdCacheHandler.RemoveGetAsync(identityKvAdToBeUpdated.Id);
    }
}
