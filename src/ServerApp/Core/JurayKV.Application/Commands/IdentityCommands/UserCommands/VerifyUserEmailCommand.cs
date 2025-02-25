﻿using System.Data;
using JurayKV.Domain.Aggregates.IdentityAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore.Storage;
using TanvirArjel.ArgumentChecker;
using TanvirArjel.EFCore.GenericRepository;

namespace JurayKV.Application.Commands.IdentityCommands.UserCommands;

public sealed class VerifyUserEmailCommand : IRequest
{
    public VerifyUserEmailCommand(string email, string code)
    {
        Email = email.ThrowIfNotValidEmail(nameof(email));
        Code = code.ThrowIfNullOrEmpty(nameof(code));
    }

    public string Email { get; }

    public string Code { get; }

    private class VerifyUserEmailCommandHandler : IRequestHandler<VerifyUserEmailCommand>
    {
        private readonly IRepository _repository;

        public VerifyUserEmailCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(VerifyUserEmailCommand request, CancellationToken cancellationToken)
        {
            request.ThrowIfNull(nameof(request));

            IDbContextTransaction dbContextTransaction = await _repository
                .BeginTransactionAsync(IsolationLevel.Unspecified, cancellationToken);

            try
            {
                EmailVerificationCode emailVerificationCode = await _repository
                .GetAsync<EmailVerificationCode>(evc => evc.Email == request.Email && evc.Code == request.Code && evc.UsedAtUtc == null, cancellationToken);

                if (emailVerificationCode == null)
                {
                    throw new InvalidOperationException("Either email or password reset code is incorrect.");
                }

                if (DateTime.UtcNow > emailVerificationCode.SentAtUtc.AddMinutes(5))
                {
                    throw new InvalidOperationException("The code is expired.");
                }

                ApplicationUser applicationUser = await _repository.GetAsync<ApplicationUser>(au => au.Email == request.Email, cancellationToken);

                if (applicationUser == null)
                {
                    throw new InvalidOperationException("The provided email is not related to any account.");
                }

                applicationUser.EmailConfirmed = true;
                _repository.Update(applicationUser);

                emailVerificationCode.UsedAtUtc = DateTime.UtcNow;
                _repository.Update(emailVerificationCode);

                await dbContextTransaction.CommitAsync(cancellationToken);
            }
            catch (Exception)
            {
                await dbContextTransaction.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }
}
