using System.Linq.Expressions;
using JurayKV.Domain.Aggregates.WalletAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using TanvirArjel.ArgumentChecker;
using TanvirArjel.EFCore.GenericRepository;

namespace JurayKV.Application.Queries.WalletQueries;

public sealed class GetWalletUserByIdQuery : IRequest<WalletDetailsDto>
{
    public GetWalletUserByIdQuery(Guid userid)
    {
        UserId = userid.ThrowIfEmpty(nameof(userid));
    }

    public Guid UserId { get; }

    // Handler
    public class GetWalletUserByIdQueryHandler : IRequestHandler<GetWalletUserByIdQuery, WalletDetailsDto>
    {
        private readonly IQueryRepository _repository;

        public GetWalletUserByIdQueryHandler(IQueryRepository repository)
        {
            _repository = repository;
        }

        public async Task<WalletDetailsDto> Handle(GetWalletUserByIdQuery request, CancellationToken cancellationToken)
        {
            request.ThrowIfNull(nameof(request));
            Expression<Func<Wallet, bool>> filterExpression = wallet => wallet.UserId == request.UserId;

            //Expression<Func<Wallet, WalletDetailsDto>> selectExp = d => new WalletDetailsDto
            //{
            //    Id = d.Id,
            //    UserId = d.UserId,
            //    Note = d.Note,
            //    LastUpdateAtUtc = d.LastUpdateAtUtc,
            //    Log = d.Log,
            //    Amount = d.Amount,
            //    CreatedAtUtc = d.CreatedAtUtc,
            //};
            
            var walletDetailsDto = await _repository.GetAsync(filterExpression);

            var outcome = new WalletDetailsDto
            {
                CreatedAtUtc = walletDetailsDto.CreatedAtUtc,
                Amount = walletDetailsDto.Amount,
                LastUpdateAtUtc = walletDetailsDto.LastUpdateAtUtc,
                Id = walletDetailsDto.Id,
                Log = walletDetailsDto.Log,
                Note = walletDetailsDto.Note,
                UserId = walletDetailsDto.UserId,
            };
            WalletDetailsDto c = new WalletDetailsDto();
            return outcome;
        }
    }
}

