using JurayKV.Application.Caching.Handlers;
using JurayKV.Application.Commands.IdentityCommands.UserCommands;
using JurayKV.Application.Infrastructures;
using JurayKV.Application.Queries.IdentityQueries.UserQueries;
using JurayKV.Application.Services;
using JurayKV.Domain.Aggregates.IdentityAggregate;
using JurayKV.Domain.Aggregates.NotificationAggregate;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Globalization;
using System.Security.Cryptography;
using TanvirArjel.ArgumentChecker;
using TanvirArjel.EFCore.GenericRepository;
using static JurayKV.Domain.Primitives.Enum;

namespace JurayKV.Application.Commands.NotificationCommands;

public sealed class CreateNotificationCommand : IRequest<DataResponseDto>
{
    public CreateNotificationCommand(Guid userId, NotificationType notificationType)
    {
        UserId = userId;
        NotificationType = notificationType;
    }

    public Guid UserId { get; }

    public NotificationType NotificationType { get; }
}

internal class CreateNotificationCommandHandler : IRequestHandler<CreateNotificationCommand, DataResponseDto>
{
    private readonly IMediator _mediator;
    public readonly UserManager<ApplicationUser> _userManager;
    private readonly INotificationRepository _notificationRepository;
    private readonly INotificationCacheHandler _notificationCacheHandler;
    private readonly ISmsSender _smsSender;
    private readonly IEmailSender _emailSender;
    private readonly IVoiceSender _voiceSender;
    private readonly IRepository _repository;
    private readonly IWhatsappOtp _whatsappOtp;
    public CreateNotificationCommandHandler(
            INotificationRepository notificationRepository,
            INotificationCacheHandler notificationCacheHandler,
            ISmsSender smsSender,
            IEmailSender emailSender,
            IVoiceSender voiceSender,
            IMediator mediator,
            UserManager<ApplicationUser> userManager,
            IRepository repository,
            IWhatsappOtp whatsappOtp)
    {
        _notificationRepository = notificationRepository;
        _notificationCacheHandler = notificationCacheHandler;
        _smsSender = smsSender;
        _emailSender = emailSender;
        _voiceSender = voiceSender;
        _mediator = mediator;
        _userManager = userManager;
        _repository = repository;
        _whatsappOtp = whatsappOtp;
    }

    public async Task<DataResponseDto> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
    {
        _ = request.ThrowIfNull(nameof(request));

        IDbContextTransaction dbContextTransaction = await _repository
               .BeginTransactionAsync(IsolationLevel.Unspecified, cancellationToken);

        try
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            bool result = false;
            int randomNumber = RandomNumberGenerator.GetInt32(0, 1000000);
            string vCode = randomNumber.ToString("D6", CultureInfo.InvariantCulture);
            string verificationCode = string.Join("", vCode.ToCharArray());
            verificationCode = verificationCode.TrimEnd();
            string vcode = $"Your Koboview OTP is {verificationCode}";
            string numbercode = verificationCode;

            GetEmailVerificationCodeQuery command = new GetEmailVerificationCodeQuery(user.Id.ToString());

            EmailVerificationCode getexistingVcode = await _mediator.Send(command);
            if (getexistingVcode == null)
            {

                SendEmailVerificationCodeCommand verificationcommand = new SendEmailVerificationCodeCommand(user.Email, user.PhoneNumber, user.Id.ToString(), verificationCode);
                bool verificationresult = await _mediator.Send(verificationcommand);
                user.VerificationCode = vcode;
            }
            else
            {
                //UpdateSendEmailVerificationCodeCommand verificationcommand = new UpdateSendEmailVerificationCodeCommand(user.Email, user.PhoneNumber, getexistingVcode.Id, verificationCode, user.Id.ToString());
                //bool verificationresult = await _mediator.Send(verificationcommand);
                user.VerificationCode = $"Your Koboview OTP is {getexistingVcode.Code}" ;
            }
            //else if (DateTime.UtcNow > getexistingVcode.SentAtUtc.AddMinutes(15))
            //{
            //    //DateTime.UtcNow > result.SentAtUtc.AddMinutes(5)
            //    vCode = randomNumber.ToString("D6", CultureInfo.InvariantCulture);
            //    verificationCode = string.Join(" ", vCode.ToCharArray());
            //    verificationCode = verificationCode.TrimEnd();
            //    vcode = $"Your KoboView OTP is {verificationCode}.";
            //    numbercode = verificationCode;

            //    UpdateEmailVerificationCodeCommand resendcommand = new UpdateEmailVerificationCodeCommand(user.Email, user.PhoneNumber, user.Id.ToString(), verificationCode, getexistingVcode.Id, null);
            //    bool verificationresult = await _mediator.Send(resendcommand);
            //}


            //user.VerificationCode = vcode;
            await _userManager.UpdateAsync(user);

            DataResponseDto responseDto = new DataResponseDto();
            responseDto.Code = numbercode;
            if (request.NotificationType == NotificationType.SMS)
            {
                result = await _smsSender.SendAsync(user.VerificationCode, request.UserId.ToString());
            }
            else if (request.NotificationType == NotificationType.Email)
            {
                result = await _emailSender.SendAsync(user.VerificationCode, request.UserId.ToString(), "Email Comfirmation");
            }
            else if (request.NotificationType == NotificationType.Voice)
            {
                result = await _voiceSender.SendAsync(user.VerificationCode, request.UserId.ToString());
            }
            else if (request.NotificationType == NotificationType.Whatsapp)
            {
                result = await _whatsappOtp.SendAsync(user.VerificationCode, request.UserId.ToString());
            }
            Notification notification = new Notification();
            notification.UserId = request.UserId;
            notification.NotificationType = request.NotificationType;
            notification.AddedAtUtc = DateTime.UtcNow.AddHours(1);
            notification.Message = vcode;
            if (result == true)
            {
                notification.NotificationStatus = NotificationStatus.Sent;
                notification.SentAtUtc = DateTime.UtcNow.AddHours(1);
                responseDto.BoolResponse = true;
            }
            else
            {
                notification.NotificationStatus = NotificationStatus.NotSent;
            }

            // Persist to the database
            await _notificationRepository.InsertAsync(notification);

            // Remove the cache
            await _notificationCacheHandler.RemoveListAsync();
            await _notificationCacheHandler.RemoveDetailsByIdAsync(notification.Id);
            await _notificationCacheHandler.RemoveGetAsync(notification.Id);

            await dbContextTransaction.CommitAsync(cancellationToken);

            return responseDto;

        }
        catch (Exception)
        {
            await dbContextTransaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}