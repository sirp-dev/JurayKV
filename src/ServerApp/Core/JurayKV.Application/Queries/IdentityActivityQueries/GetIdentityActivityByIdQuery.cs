using System.Linq.Expressions;
using JurayKV.Domain.Aggregates.IdentityActivityAggregate;
using MediatR;
using TanvirArjel.ArgumentChecker;
using TanvirArjel.EFCore.GenericRepository;

namespace JurayKV.Application.Queries.IdentityActivityQueries;

public sealed class GetIdentityActivityByIdQuery : IRequest<IdentityActivityListDto>
{
    public GetIdentityActivityByIdQuery(Guid id)
    {
        Id = id.ThrowIfEmpty(nameof(id));
    }

    public Guid Id { get; }

    // Handler
    private class GetIdentityActivityByIdQueryHandler : IRequestHandler<GetIdentityActivityByIdQuery, IdentityActivityListDto>
    {
        private readonly IQueryRepository _repository;

        public GetIdentityActivityByIdQueryHandler(IQueryRepository repository)
        {
            _repository = repository;
        }

        public async Task<IdentityActivityListDto> Handle(GetIdentityActivityByIdQuery request, CancellationToken cancellationToken)
        {
            request.ThrowIfNull(nameof(request));

            Expression<Func<IdentityActivity, IdentityActivityListDto>> selectExp = d => new IdentityActivityListDto
            {
                Id = d.Id,
               Activity = d.Activity,
               CreatedAtUtc = d.CreatedAtUtc,
               UserId = d.UserId,
               Fullname = d.User.SurName
            };

            IdentityActivityListDto identityActivityDetailsDto = await _repository.GetByIdAsync(request.Id, selectExp, cancellationToken);

            return identityActivityDetailsDto;
        }
    }
}

 