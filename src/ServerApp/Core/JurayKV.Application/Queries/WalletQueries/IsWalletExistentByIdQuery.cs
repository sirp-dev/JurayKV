using JurayKV.Domain.Aggregates.WalletAggregate;
using MediatR;
using TanvirArjel.ArgumentChecker;
using TanvirArjel.EFCore.GenericRepository;

namespace JurayKV.Application.Queries.WalletQueries;

public sealed class IsWalletExistentByIdQuery : IRequest<bool>
{
    public IsWalletExistentByIdQuery(Guid departmetnId)
    {
        Id = departmetnId.ThrowIfEmpty(nameof(departmetnId));
    }

    public Guid Id { get; }

    private class IsWalletExistentByIdQueryHandler : IRequestHandler<IsWalletExistentByIdQuery, bool>
    {
        private readonly IQueryRepository _repository;

        public IsWalletExistentByIdQueryHandler(IQueryRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(IsWalletExistentByIdQuery request, CancellationToken cancellationToken)
        {
            request.ThrowIfNull(nameof(request));

            bool isExists = await _repository.ExistsAsync<Wallet>(d => d.Id == request.Id, cancellationToken);
            return isExists;
        }
    }
}
