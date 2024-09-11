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
    public sealed class GetMessageListQuery : IRequest<List<UserMessage>>
    {

        private class GetMessageListByUserIdQueryHandler : IRequestHandler<GetMessageListByUserIdQuery, List<UserMessage>>
        {
            private readonly IUserMessageRepository _userMessageCacheRepository;

            public GetMessageListByUserIdQueryHandler(IUserMessageRepository userMessageCacheRepository)
            {
                _userMessageCacheRepository = userMessageCacheRepository;
            }

            public async Task<List<UserMessage>> Handle(GetMessageListByUserIdQuery request, CancellationToken cancellationToken)
            {
                request.ThrowIfNull(nameof(request));

                List<UserMessage> kvPointDtos = await _userMessageCacheRepository.ListAllAsync();
                return kvPointDtos;
            }
        }
    }

}
