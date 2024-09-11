using JurayKV.Domain.Aggregates.StateLgaAggregate;
using MediatR;
using System.Linq;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Application.Queries.StateLgaQueries
{

    public sealed class StateQuery : IRequest<List<StateDto>>
    {
        private class StateQueryHandler : IRequestHandler<StateQuery, List<StateDto>>
        {
            private readonly IStateRepository _stateRepository;

            public StateQueryHandler(IStateRepository stateRepository)
            {
                _stateRepository = stateRepository;
            }

            public async Task<List<StateDto>> Handle(StateQuery request, CancellationToken cancellationToken)
            {
                request.ThrowIfNull(nameof(request));

                var states = await _stateRepository.GetAllState();
                var list = states.Select(d => new StateDto
                {
                    Id = d.Id,
                    State = d.StateName,

                }).ToList();
                return list.OrderBy(x => x.State).ToList();
            }
        }
    }

}
