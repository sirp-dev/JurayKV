﻿using JurayKV.Domain.Aggregates.IdentityAggregate;
using MediatR;
using TanvirArjel.ArgumentChecker;
using TanvirArjel.EFCore.GenericRepository;

namespace JurayKV.Application.Commands.IdentityCommands.UserCommands;

public sealed class StoreRefreshTokenCommand : IRequest<RefreshToken>
{
    public StoreRefreshTokenCommand(Guid userId, string token)
    {
        UserId = userId.ThrowIfEmpty(nameof(userId));
        Token = token.ThrowIfNullOrEmpty(nameof(token));
    }

    public Guid UserId { get; }

    public string Token { get; }

    private class StoreRefreshTokenCommandHandler : IRequestHandler<StoreRefreshTokenCommand, RefreshToken>
    {
        private readonly IRepository _repository;

        public StoreRefreshTokenCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<RefreshToken> Handle(StoreRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            request.ThrowIfNull(nameof(request));

            RefreshToken refreshToken = new RefreshToken()
            {
                UserId = request.UserId,
                Token = request.Token,
                CreatedAtUtc = DateTime.UtcNow,
                ExpireAtUtc = DateTime.UtcNow.AddDays(30)
            };

            _repository.Add(refreshToken);
            await _repository.SaveChangesAsync(cancellationToken);
            return refreshToken;
        }
    }
}
