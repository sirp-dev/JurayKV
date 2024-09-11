using System.Globalization;
using System.Security.Cryptography;
using JurayKV.Application.Infrastructures;
using JurayKV.Application.Services;
using JurayKV.Domain.Aggregates.IdentityAggregate;
using MediatR;
using TanvirArjel.ArgumentChecker;
using TanvirArjel.EFCore.GenericRepository;

namespace JurayKV.Application.Commands.IdentityCommands.UserCommands;

public sealed class SendEmailVerificationCodeCommand : IRequest<bool>
{
    public SendEmailVerificationCodeCommand(string email, string phone, string id, string code)
    {
        Email = email.ThrowIfNotValidEmail(nameof(email));
        Phone = phone.ThrowIfNullOrEmpty(nameof(phone));
        Id = id.ThrowIfNullOrEmpty(nameof(id));
        Code = code.ThrowIfNullOrEmpty(nameof(code));
    }

    public string Email { get; }
    public string Phone { get; }
    public string Id { get; }
    public string Code { get; }

    private class SendEmailVerificationCodeCommandHanlder : IRequestHandler<SendEmailVerificationCodeCommand, bool>
    {
        private readonly IRepository _repository;
        private readonly ViewRenderService _viewRenderService;
        private readonly IEmailSender _emailSender;

        public SendEmailVerificationCodeCommandHanlder(
            IRepository repository,
            ViewRenderService viewRenderService,
            IEmailSender emailSender)
        {
            _repository = repository;
            _viewRenderService = viewRenderService;
            _emailSender = emailSender;
        }

        public async Task<bool> Handle(SendEmailVerificationCodeCommand request, CancellationToken cancellationToken)
        {
            request.ThrowIfNull(nameof(request));

            
            EmailVerificationCode emailVerificationCode = new EmailVerificationCode()
            {
                Code = request.Code,
                Email = request.Email,
                SentAtUtc = DateTime.UtcNow,
                PhoneNumber = request.Phone,
                UserId = request.Id
            };

            await _repository.AddAsync(emailVerificationCode, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);
             
            
          return false;
        }
    }
}
