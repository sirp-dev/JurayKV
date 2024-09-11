using JurayKV.Application.Caching.Handlers;
using JurayKV.Application.Infrastructures;
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


    public sealed class CreateUserMessageCommand : IRequest
    {
        public CreateUserMessageCommand(UserMessage userMessage, IFormFile file)
        {
            UserMessage = userMessage;
            File = file;
        }

        public UserMessage UserMessage { get; }
        public IFormFile? File { get; set; }
    }

    internal class CreateUserMessageCommandHandler : IRequestHandler<CreateUserMessageCommand>
    {
        private readonly IUserMessageRepository _userMessageRepository;
         private readonly IStorageService _storage;

        public CreateUserMessageCommandHandler(
                IUserMessageRepository userMessageRepository, 
                IStorageService storage)
        {
            _userMessageRepository = userMessageRepository; 
            _storage = storage;
        }

        public async Task Handle(CreateUserMessageCommand request, CancellationToken cancellationToken)
        {
            _ = request.ThrowIfNull(nameof(request));

            try
            {

                var xresult = await _storage.MainUploadFileReturnUrlAsync("", request.File);
                // 
                if (xresult.Message.Contains("200"))
                {
                    request.UserMessage.FileUrl = xresult.Url;
                    request.UserMessage.FileKey = xresult.Key;
                }

            }
            catch (Exception c)
            {

            }

            request.UserMessage.Date = DateTime.UtcNow;
            await _userMessageRepository.InsertAsync(request.UserMessage);
             

        }
    }
}
