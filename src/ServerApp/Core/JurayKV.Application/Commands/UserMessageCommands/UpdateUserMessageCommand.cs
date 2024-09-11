using JurayKV.Application.Caching.Handlers;
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
    
public sealed class UpdateUserMessageCommand : IRequest
    {
        public UpdateUserMessageCommand(UserMessage userMessage, IFormFile file)
        {
            UserMessage = userMessage;
            File = file;  
        }

        public UserMessage UserMessage { get; }
        public IFormFile? File { get; set; } 
    }

    internal class UpdateUserMessageCommandHandler : IRequestHandler<UpdateUserMessageCommand>
    {
        private readonly IUserMessageRepository _userMessageRepository;
         private readonly IStorageService _storage;

        public UpdateUserMessageCommandHandler(
            IUserMessageRepository userMessageRepository, 
            IStorageService storage)
        {
            _userMessageRepository = userMessageRepository;
             _storage = storage;
        }

        public async Task Handle(UpdateUserMessageCommand request, CancellationToken cancellationToken)
        {
            request.ThrowIfNull(nameof(request));
            var getupdate = await _userMessageRepository.GetByIdAsync(request.UserMessage.Id);
            if (getupdate != null)
            {
                try
                {
                    if (request.File != null)
                    {
                        var xresult = await _storage.MainUploadFileReturnUrlAsync(request.UserMessage.FileKey, request.File);
                        // 
                        if (xresult.Message.Contains("200"))
                        {

                            getupdate.FileUrl = xresult.Url;
                            getupdate.FileKey = xresult.Key;
                        }
                    }

                }
                catch (Exception c)
                {

                }
            getupdate.Message = request.UserMessage.Message;
                getupdate.Title = request.UserMessage.Title;
                getupdate.Read = request.UserMessage.Read;
                getupdate.DateRead = request.UserMessage.DateRead;
                getupdate.UserId = request.UserMessage.UserId;
                getupdate.Disable = request.UserMessage.Disable;
                getupdate.All = request.UserMessage.All;
                

                await _userMessageRepository.UpdateAsync(getupdate);
                 
            }
        }
    }

}
