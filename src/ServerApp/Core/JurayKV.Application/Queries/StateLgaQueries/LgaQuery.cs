using JurayKV.Domain.Aggregates.StateLgaAggregate;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Application.Queries.StateLgaQueries
{
       public sealed class LgaQuery : IRequest<List<LgaDto>>
    {
        public LgaQuery(Guid id)
        {
            Id = id.ThrowIfEmpty(nameof(id));
        }

        public Guid Id { get; }
        private class LgaQueryHandler : IRequestHandler<LgaQuery, List<LgaDto>>
        {
            private readonly ILgaRepository _lgaRepository;

            public LgaQueryHandler(ILgaRepository lgaRepository)
            {
                _lgaRepository = lgaRepository;
            }

            public async Task<List<LgaDto>> Handle(LgaQuery request, CancellationToken cancellationToken)
            {
                request.ThrowIfNull(nameof(request));

                var states = await _lgaRepository.GetAllLGA(request.Id);
                var list = states.Select(d => new LgaDto
                {
                    LGA = d.LGAName
                }).ToList();
                return list.OrderBy(x => x.LGA).ToList();
            }
        }
    }
}
