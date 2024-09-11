using JurayKV.Application.Caching.Handlers;
using JurayKV.Application.Infrastructures;
using JurayKV.Domain.Aggregates.IdentityKvAdAggregate;
using JurayKV.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Application.Commands.IdentityKvAdCommands
{
     public sealed class UpdateIdentityKvAdPointCommand : IRequest
    {
        public UpdateIdentityKvAdPointCommand(
            Guid id,
            int p1,
            int p2,
            int p3, string activity
            )
        {
            Id = id;
            PointOne = p1;
            PointTwo = p2;
            PointThree = p3;
            Activity = activity;
            
        }

        public Guid Id { get; }

        public int PointOne { get; set; }
        public int PointTwo { get; set; }
        public int PointThree { get; set; }
        public string Activity { get; set; }
    }

    internal class UpdateIdentityKvAdPointCommandHandler : IRequestHandler<UpdateIdentityKvAdPointCommand>
    {
        private readonly IIdentityKvAdRepository _identityKvAdRepository;
        private readonly IIdentityKvAdCacheHandler _identityKvAdCacheHandler;
        private readonly IStorageService _storage;

        public UpdateIdentityKvAdPointCommandHandler(
            IIdentityKvAdRepository identityKvAdRepository,
            IIdentityKvAdCacheHandler identityKvAdCacheHandler,
            IStorageService storage)
        {
            _identityKvAdRepository = identityKvAdRepository;
            _identityKvAdCacheHandler = identityKvAdCacheHandler;
            _storage = storage;
        }

        public async Task Handle(UpdateIdentityKvAdPointCommand request, CancellationToken cancellationToken)
        {
            request.ThrowIfNull(nameof(request));

            IdentityKvAd identityKvAdToBeUpdated = await _identityKvAdRepository.GetByIdAsync(request.Id);

            if (identityKvAdToBeUpdated == null)
            {
                throw new EntityNotFoundException(typeof(IdentityKvAd), request.Id);
            }

            
            identityKvAdToBeUpdated.LastModifiedAtUtc = DateTime.UtcNow;
            identityKvAdToBeUpdated.ResultOne = request.PointOne;
            identityKvAdToBeUpdated.ResultTwo = request.PointTwo;
            identityKvAdToBeUpdated.ResultThree = request.PointThree;
            identityKvAdToBeUpdated.Activity = request.Activity;
            identityKvAdToBeUpdated.Active = false;
            
            await _identityKvAdRepository.UpdateAsync(identityKvAdToBeUpdated);

            // Remove the cache
            await _identityKvAdCacheHandler.RemoveListAsync();
            await _identityKvAdCacheHandler.RemoveGetByUserIdAsync(identityKvAdToBeUpdated.UserId);
            await _identityKvAdCacheHandler.RemoveGetActiveByUserIdAsync(identityKvAdToBeUpdated.UserId);
            await _identityKvAdCacheHandler.RemoveDetailsByIdAsync(identityKvAdToBeUpdated.Id);
            await _identityKvAdCacheHandler.RemoveGetAsync(identityKvAdToBeUpdated.Id);
        }
    }

}
