using JurayKV.Application.Caching.Handlers;
using JurayKV.Application.Infrastructures;
using JurayKV.Application.Queries.AdvertRequestQueries;
using JurayKV.Domain.Aggregates.AdvertRequestAggregate;
using JurayKV.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using TanvirArjel.ArgumentChecker;
using static JurayKV.Domain.Primitives.Enum;

namespace JurayKV.Application.Commands.AdvertRequestCommands;

public sealed class UpdateAdvertRequestCommand : IRequest
{
    public UpdateAdvertRequestCommand(AdvertRequestDetailsDto advertRequest, IFormFile file)
    {
      AdvertRequest = advertRequest; 
        File = file;

    }
    public AdvertRequestDetailsDto AdvertRequest { get; set; }
    public IFormFile? File { get; set; }

}

internal class UpdateAdvertRequestCommandHandler : IRequestHandler<UpdateAdvertRequestCommand>
{
    private readonly IAdvertRequestRepository _advertRequestRepository;
    private readonly IAdvertRequestCacheHandler _advertRequestCacheHandler;
    private readonly IStorageService _storage;

    public UpdateAdvertRequestCommandHandler(
        IAdvertRequestRepository advertRequestRepository,
        IAdvertRequestCacheHandler advertRequestCacheHandler,
        IStorageService storage)
    {
        _advertRequestRepository = advertRequestRepository;
        _advertRequestCacheHandler = advertRequestCacheHandler;
        _storage = storage;
    }

    public async Task Handle(UpdateAdvertRequestCommand request, CancellationToken cancellationToken)
    {
        request.ThrowIfNull(nameof(request));

        AdvertRequest advertRequestToBeUpdated = await _advertRequestRepository.GetByIdAsync(request.AdvertRequest.Id);

        if (advertRequestToBeUpdated == null)
        {
            throw new NotFiniteNumberException(nameof(request));
        }

        try
        {
            if (request.File != null)
            {
                var xresult = await _storage.MainUploadFileReturnUrlAsync(request.AdvertRequest.ImageKey, request.File);
                // 
                if (xresult.Message.Contains("200"))
                {

                    advertRequestToBeUpdated.ImageUrl = xresult.Url;
                    advertRequestToBeUpdated.ImageKey = xresult.Key;
                }
            }

        }
        catch (Exception c)
        {

        }
        advertRequestToBeUpdated.CompanyId = request.AdvertRequest.CompanyId;
         advertRequestToBeUpdated.TransactionReference = request.AdvertRequest.TransactionReference;
         advertRequestToBeUpdated.Status = request.AdvertRequest.Status;
         advertRequestToBeUpdated.Amount = request.AdvertRequest.Amount;
        advertRequestToBeUpdated.Note = request.AdvertRequest.Note;
        advertRequestToBeUpdated.CreatedAtUtc = request.AdvertRequest.CreatedAtUtc;

        await _advertRequestRepository.UpdateAsync(advertRequestToBeUpdated);

        // Remove the cache
        await _advertRequestCacheHandler.RemoveListAsync();
        await _advertRequestCacheHandler.RemoveGetAsync(advertRequestToBeUpdated.Id);
        await _advertRequestCacheHandler.RemoveDetailsByIdAsync(advertRequestToBeUpdated.Id);
        await _advertRequestCacheHandler.RemoveList10ByCompanyAsync(advertRequestToBeUpdated.CompanyId);

    }
}
