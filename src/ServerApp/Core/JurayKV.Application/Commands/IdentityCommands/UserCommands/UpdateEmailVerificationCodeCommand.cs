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
    public sealed class UpdateEmailVerificationCodeCommand : IRequest<bool>
    {
        public UpdateEmailVerificationCodeCommand(string email, string phone, string userId, string code, Guid id, DateTime? usedAt)
        {
            Email = email.ThrowIfNotValidEmail(nameof(email));
            Phone = phone.ThrowIfNullOrEmpty(nameof(phone));
            UserId = userId.ThrowIfNullOrEmpty(nameof(userId));
            Id = id;
            Code = code.ThrowIfNullOrEmpty(nameof(code));
            UseDate = usedAt;
        }

        public string Email { set; get; }
        public string Phone { set; get; }
        public string UserId { set; get; }
        public string Code { set; get; }
        public Guid Id { set; get; }
        public DateTime? UseDate { set; get; }
        private class UpdateEmailVerificationCodeCommandHanlder : IRequestHandler<UpdateEmailVerificationCodeCommand, bool>
        {
            private readonly IRepository _repository;
            private readonly ViewRenderService _viewRenderService;
            private readonly IEmailSender _emailSender;

            public UpdateEmailVerificationCodeCommandHanlder(
                IRepository repository,
                ViewRenderService viewRenderService,
                IEmailSender emailSender)
            {
                _repository = repository;
                _viewRenderService = viewRenderService;
                _emailSender = emailSender;
            }

            public async Task<bool> Handle(UpdateEmailVerificationCodeCommand request, CancellationToken cancellationToken)
            {
                request.ThrowIfNull(nameof(request));
                var existingEntity = await _repository.GetAsync<EmailVerificationCode>(au => au.Id == request.Id, cancellationToken);
                
                if (existingEntity != null)
                {

                    EmailVerificationCode emailVerificationCode = new EmailVerificationCode()
                    {
                        Id = existingEntity.Id,
                        Code = request.Code,
                        Email = request.Email,
                        SentAtUtc = DateTime.UtcNow,
                        PhoneNumber = request.Phone,
                        UserId = request.UserId,
                        UsedAtUtc = request.UseDate
                    };

                    await _repository.UpdateAsync(emailVerificationCode, cancellationToken);
                    await _repository.SaveChangesAsync(cancellationToken);
                }

                return false;
            }
        }
    }

}
