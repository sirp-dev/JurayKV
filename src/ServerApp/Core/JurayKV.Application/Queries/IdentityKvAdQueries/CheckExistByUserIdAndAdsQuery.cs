using JurayKV.Domain.Aggregates.IdentityKvAdAggregate;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanvirArjel.ArgumentChecker;
using TanvirArjel.EFCore.GenericRepository;

namespace JurayKV.Application.Queries.IdentityKvAdQueries;

public sealed class CheckExistByUserIdAndAdsQuery : IRequest<bool>
{
    public CheckExistByUserIdAndAdsQuery(Guid userId, Guid kvAdsId)
    {
        KvAdsId = kvAdsId.ThrowIfEmpty(nameof(kvAdsId));
        UserId = userId.ThrowIfEmpty(nameof(userId));
    }

    public Guid UserId { get; }
    public Guid KvAdsId { get; }

    private class CheckExistByUserIdAndAdsQueryHandler : IRequestHandler<CheckExistByUserIdAndAdsQuery, bool>
    {
        private readonly IIdentityKvAdRepository _repository;

        public CheckExistByUserIdAndAdsQueryHandler(IIdentityKvAdRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(CheckExistByUserIdAndAdsQuery request, CancellationToken cancellationToken)
        {
            request.ThrowIfNull(nameof(request));

            bool isExists = await _repository.CheckExist(request.UserId, request.KvAdsId);
            return isExists;
        }
    }
}

