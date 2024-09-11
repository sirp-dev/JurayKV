using JurayKV.Application.Infrastructures;
using JurayKV.Application.Queries.UserMessageQueries;
using JurayKV.Domain.Aggregates.UserMessageAggregate;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Application.Commands.UserMessageCommands
{
   
public sealed class MarkAsReadCommand : IRequest
    {
        public MarkAsReadCommand(Guid userMessageId)
        {
            UserMessageId = userMessageId;
         }

        public Guid UserMessageId { get; }
     }

    internal class MarkAsReadCommandHandler : IRequestHandler<MarkAsReadCommand>
    {
        private readonly IUserMessageRepository _userMessageRepository; 

        public MarkAsReadCommandHandler(
            IUserMessageRepository userMessageRepository )
        {
            _userMessageRepository = userMessageRepository;
         }

        public async Task Handle(MarkAsReadCommand request, CancellationToken cancellationToken)
        {
            request.ThrowIfNull(nameof(request));
            var getupdate = await _userMessageRepository.GetByIdAsync(request.UserMessageId);
            if (getupdate != null)
            {
                
                getupdate.Read = true; 
                getupdate.DateRead = DateTime.UtcNow.AddHours(1); 
                await _userMessageRepository.UpdateAsync(getupdate);

            }
        }
    }

}
