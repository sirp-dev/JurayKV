using JurayKV.Domain.Aggregates.IdentityAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore.Storage;
using TanvirArjel.ArgumentChecker;
using TanvirArjel.EFCore.GenericRepository;

namespace JurayKV.Application.Commands.IdentityCommands.UserCommands;

public sealed class UpdateVerificationCodeCommand : IRequest
{
    public UpdateVerificationCodeCommand(Guid userId, string code, Guid id)
    {
        UserId = userId.ThrowIfEmpty(nameof(UserId));
        Code = code.ThrowIfNullOrEmpty(nameof(Code));
        Id = id;
    }

    public Guid Id { get; }
    public Guid UserId { get; }

    public string Code { get; }

    private class UpdateVerificationCodeCommandHandler : IRequestHandler<UpdateVerificationCodeCommand>
    {
        private readonly IRepository _repository;

        public UpdateVerificationCodeCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(UpdateVerificationCodeCommand request, CancellationToken cancellationToken)
        {
            request.ThrowIfNull(nameof(request));

             
            try
            {
                EmailVerificationCode emailVerificationCode = await _repository
                .GetAsync<EmailVerificationCode>(evc => evc.Id == request.Id, cancellationToken);

                 

                emailVerificationCode.UsedAtUtc = DateTime.UtcNow;
                _repository.Update(emailVerificationCode);
                 
            }
            catch (Exception)
            { 
                throw;
            }
        }
    }
}
