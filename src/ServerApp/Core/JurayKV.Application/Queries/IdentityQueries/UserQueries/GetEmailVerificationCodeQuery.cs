using JurayKV.Domain.Aggregates.IdentityAggregate;
using MediatR;
using TanvirArjel.ArgumentChecker;
using TanvirArjel.EFCore.GenericRepository;

namespace JurayKV.Application.Queries.IdentityQueries.UserQueries;

public sealed class GetEmailVerificationCodeQuery : IRequest<EmailVerificationCode>
{
    public GetEmailVerificationCodeQuery(string userid)
    {
        UserId = userid.ThrowIfNullOrEmpty(nameof(userid));
    }
     
    public string UserId { get; }

    private class GetEmailVerificationCodeQueryHandler : IRequestHandler<GetEmailVerificationCodeQuery, EmailVerificationCode>
    {
        private readonly IRepository _repository;

        public GetEmailVerificationCodeQueryHandler(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<EmailVerificationCode> Handle(GetEmailVerificationCodeQuery request, CancellationToken cancellationToken)
        {
            request.ThrowIfNull(nameof(request));

            EmailVerificationCode emailVerificationCode = await _repository
            .GetAsync<EmailVerificationCode>(evc => evc.UserId == request.UserId, cancellationToken);

            return emailVerificationCode;
        }
    }
}
