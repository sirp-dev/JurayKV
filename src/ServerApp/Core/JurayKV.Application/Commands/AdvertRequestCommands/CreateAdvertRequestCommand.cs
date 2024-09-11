using JurayKV.Application.Caching.Handlers;
using JurayKV.Domain.Aggregates.IdentityAggregate;
using JurayKV.Domain.Aggregates.AdvertRequestAggregate;
using JurayKV.Domain.Aggregates.WalletAggregate;
using JurayKV.Domain.ValueObjects;
using MediatR;
using TanvirArjel.ArgumentChecker;
using static JurayKV.Domain.Primitives.Enum;
using Microsoft.AspNetCore.Http;
using JurayKV.Application.Infrastructures;

namespace JurayKV.Application.Commands.AdvertRequestCommands;

public sealed class CreateAdvertRequestCommand : IRequest<Guid>
{
    public CreateAdvertRequestCommand(AdvertRequest advertRequest, IFormFile file)
    {
     AdvertRequest = advertRequest;
        File = file;
    }

    public AdvertRequest AdvertRequest { get; set; }
    public IFormFile? File { get; set; }

}

internal class CreateAdvertRequestCommandHandler : IRequestHandler<CreateAdvertRequestCommand, Guid>
{
    private readonly IAdvertRequestRepository _advertRequestRepository;
    private readonly IAdvertRequestCacheHandler _advertRequestCacheHandler;
    private readonly IStorageService _storage;

    public CreateAdvertRequestCommandHandler(
            IAdvertRequestRepository advertRequestRepository,
            IAdvertRequestCacheHandler advertRequestCacheHandler,
            IStorageService storage)
    {
        _advertRequestRepository = advertRequestRepository;
        _advertRequestCacheHandler = advertRequestCacheHandler;
        _storage = storage;
    }

    public async Task<Guid> Handle(CreateAdvertRequestCommand request, CancellationToken cancellationToken)
    {
        _ = request.ThrowIfNull(nameof(request));
        try
        {

            var xresult = await _storage.MainUploadFileReturnUrlAsync("", request.File);
            // 
            if (xresult.Message.Contains("200"))
            {
                request.AdvertRequest.ImageUrl = xresult.Url;
                request.AdvertRequest.ImageKey = xresult.Key;
            }

        }
        catch (Exception c)
        {

        }
        // Persist to the database
        request.AdvertRequest.CreatedAtUtc = DateTime.UtcNow.AddHours(1);
        await _advertRequestRepository.InsertAsync(request.AdvertRequest);

        // Remove the cache
        await _advertRequestCacheHandler.RemoveListAsync();
        await _advertRequestCacheHandler.RemoveGetAsync(request.AdvertRequest.Id);
        await _advertRequestCacheHandler.RemoveDetailsByIdAsync(request.AdvertRequest.Id);
        await _advertRequestCacheHandler.RemoveList10ByCompanyAsync(request.AdvertRequest.CompanyId);

        return request.AdvertRequest.Id;
    }
}