using JurayKV.Application.Infrastructures;
using JurayKV.Application.Services;
using JurayKV.Domain.Aggregates.IdentityAggregate;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanvirArjel.ArgumentChecker;
using TanvirArjel.EFCore.GenericRepository;

namespace JurayKV.Application.Commands.IdentityCommands.UserCommands
{
    public sealed class UpdateSendEmailVerificationCodeCommand : IRequest<bool>
    {
        public UpdateSendEmailVerificationCodeCommand(string email, string phone, Guid id, string code, string userId)
        {
            Email = email.ThrowIfNotValidEmail(nameof(email));
            Phone = phone.ThrowIfNullOrEmpty(nameof(phone));
            Id = id;
            UserId = userId.ThrowIfNullOrEmpty(nameof(userId));
            Code = code.ThrowIfNullOrEmpty(nameof(code));
        }

        public string Email { get; }
        public string Phone { get; }
        public Guid Id { get; }
        public string Code { get; }
        public string UserId { get; }

        private class UpdateSendEmailVerificationCodeCommandHanlder : IRequestHandler<UpdateSendEmailVerificationCodeCommand, bool>
        {
            private readonly IRepository _repository;
            private readonly ViewRenderService _viewRenderService;
            private readonly IEmailSender _emailSender;

            public UpdateSendEmailVerificationCodeCommandHanlder(
                IRepository repository,
                ViewRenderService viewRenderService,
                IEmailSender emailSender)
            {
                _repository = repository;
                _viewRenderService = viewRenderService;
                _emailSender = emailSender;
            }

            public async Task<bool> Handle(UpdateSendEmailVerificationCodeCommand request, CancellationToken cancellationToken)
            {
                request.ThrowIfNull(nameof(request));
                EmailVerificationCode emailVerificationCode = await _repository
            .GetAsync<EmailVerificationCode>(evc => evc.UserId == request.UserId, cancellationToken);

                emailVerificationCode.Code = request.Code;
                    emailVerificationCode.Email = request.Email;
                    emailVerificationCode.SentAtUtc = DateTime.UtcNow;
                    emailVerificationCode.PhoneNumber = request.Phone;
                    emailVerificationCode.UserId = request.UserId;
                    emailVerificationCode.Id = request.Id;


                await _repository.UpdateAsync(emailVerificationCode, cancellationToken);
                await _repository.SaveChangesAsync(cancellationToken);


                return false;
            }
        }
    }

}
