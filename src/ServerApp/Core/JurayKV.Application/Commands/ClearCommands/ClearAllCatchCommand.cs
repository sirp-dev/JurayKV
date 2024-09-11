using JurayKV.Application.Caching.Handlers;
using JurayKV.Application.Commands.BucketCommands;
using JurayKV.Domain.Aggregates.BucketAggregate;
using JurayKV.Domain.Aggregates.CompanyAggregate;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Application.Commands.ClearCommands
{

    public sealed class ClearAllCatchCommand : IRequest
    {

    }

    internal class ClearAllCatchCommandHandler : IRequestHandler<ClearAllCatchCommand>
    {
         private readonly IClearAllCacheHandler _cacheHandler;

        public ClearAllCatchCommandHandler(
IClearAllCacheHandler cacheHandler)
        {
            _cacheHandler = cacheHandler;
        }

        public async Task Handle(ClearAllCatchCommand request, CancellationToken cancellationToken)
        {

            // Remove the cache
            await _cacheHandler.ClearCacheAsync();
             

        }
    }
}
