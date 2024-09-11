using JurayKV.Application.Caching.Repositories;
using JurayKV.Domain.Aggregates.UserMessageAggregate;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Application.Queries.UserMessageQueries
{
    
    public sealed class GetMessageByIdQuery : IRequest<UserMessage>
    {
        public GetMessageByIdQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
        private class GetMessageByIdQueryHandler : IRequestHandler<GetMessageByIdQuery, UserMessage>
        {
            private readonly IUserMessageRepository _userMessage;

            public GetMessageByIdQueryHandler(IUserMessageRepository userMessage)
            {
                _userMessage = userMessage;
            }

            public async Task<UserMessage> Handle(GetMessageByIdQuery request, CancellationToken cancellationToken)
            {
                request.ThrowIfNull(nameof(request));

                UserMessage dataMessage = await _userMessage.GetById(request.Id);
                return dataMessage;
            }
        }
    }
}
