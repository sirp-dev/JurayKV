using JurayKV.Application.Caching.Handlers;
using JurayKV.Domain.Aggregates.UserMessageAggregate;
using JurayKV.Domain.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Application.Commands.UserMessageCommands
{

    public sealed class DeleteUserMessageCommand : IRequest
    {
        public DeleteUserMessageCommand(Guid userMessageId)
        {
            Id = userMessageId.ThrowIfEmpty(nameof(userMessageId));
        }

        public Guid Id { get; }
    }

    internal class DeleteUserMessageCommandHandler : IRequestHandler<DeleteUserMessageCommand>
    {
        private readonly IUserMessageRepository _userMessageRepository;
 
        public DeleteUserMessageCommandHandler(IUserMessageRepository userMessageRepository)
        {
            _userMessageRepository = userMessageRepository;
         }

        public async Task Handle(DeleteUserMessageCommand request, CancellationToken cancellationToken)
        {
            _ = request.ThrowIfNull(nameof(request));

            UserMessage userMessage = await _userMessageRepository.GetByIdAsync(request.Id);

            if (userMessage == null)
            {
                throw new EntityNotFoundException(typeof(UserMessage), request.Id);
            }

            await _userMessageRepository.DeleteAsync(userMessage);
             
        }
    }

}