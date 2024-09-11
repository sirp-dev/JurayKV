﻿using JurayKV.Domain.Aggregates.IdentityAggregate;
using MediatR;
using TanvirArjel.ArgumentChecker;
using TanvirArjel.EFCore.GenericRepository;

namespace JurayKV.Application.Queries.IdentityQueries.UserQueries;

public sealed class GetRefreshTokenQuery : IRequest<RefreshToken>
{
    public GetRefreshTokenQuery(Guid userId)
    {
        UserId = userId.ThrowIfEmpty(nameof(userId));
    }

    public Guid UserId { get; }

    private class GetRefreshTokenQueryHanlder : IRequestHandler<GetRefreshTokenQuery, RefreshToken>
    {
        private readonly IRepository _repository;

        public GetRefreshTokenQueryHanlder(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<RefreshToken> Handle(GetRefreshTokenQuery request, CancellationToken cancellationToken)
        {
            request.ThrowIfNull(nameof(request));

            RefreshToken refreshToken = await _repository.GetAsync<RefreshToken>(rt => rt.UserId == request.UserId, cancellationToken);

            return refreshToken;
        }
    }
}
