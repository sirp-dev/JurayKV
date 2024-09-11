using JurayKV.Domain.Aggregates.AdvertRequestAggregate;
using MediatR;
using TanvirArjel.ArgumentChecker;
using TanvirArjel.EFCore.GenericRepository;

namespace JurayKV.Application.Queries.AdvertRequestQueries;

public sealed class IsAdvertRequestTokenUniqueQuery : IRequest<bool>
{
    public IsAdvertRequestTokenUniqueQuery(Guid advertRequestId, string token)
    {
        Id = advertRequestId.ThrowIfEmpty(nameof(advertRequestId));
        Token = token.ThrowIfNullOrEmpty(nameof(token));
    }

    public Guid Id { get; }

    public string Token { get; }

    private class IsAdvertRequestNameUniqueQueryHandler : IRequestHandler<IsAdvertRequestTokenUniqueQuery, bool>
    {
        private readonly IQueryRepository _repository;

        public IsAdvertRequestNameUniqueQueryHandler(IQueryRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(IsAdvertRequestTokenUniqueQuery request, CancellationToken cancellationToken)
        {
            request.ThrowIfNull(nameof(request));

            bool isExistent = await _repository.ExistsAsync<AdvertRequest>(d => d.Id != request.Id, cancellationToken);
            return !isExistent;
        }
    }
}
