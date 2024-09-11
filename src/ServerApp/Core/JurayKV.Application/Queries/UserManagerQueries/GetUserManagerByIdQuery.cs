using System.Linq.Expressions;
using JurayKV.Application.Caching.Repositories;
using JurayKV.Application.Queries.WalletQueries;
using JurayKV.Domain.Aggregates.IdentityAggregate;
using JurayKV.Domain.Aggregates.WalletAggregate;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TanvirArjel.ArgumentChecker;
using TanvirArjel.EFCore.GenericRepository;

namespace JurayKV.Application.Queries.UserManagerQueries;

public sealed class GetUserManagerByIdQuery : IRequest<UserManagerDetailsDto>
{
    public GetUserManagerByIdQuery(Guid id)
    {
        Id = id.ThrowIfEmpty(nameof(id));
    }

    public Guid Id { get; }

    // Handler
    public class GetUserManagerByIdQueryHandler : IRequestHandler<GetUserManagerByIdQuery, UserManagerDetailsDto>
    {
        private readonly IUserManagerCacheRepository _userManager;
        private readonly IQueryRepository _repository;
        private readonly IMediator _mediator;
        private readonly UserManager<ApplicationUser> _user;



        public GetUserManagerByIdQueryHandler(IUserManagerCacheRepository userManager, IQueryRepository repository, IMediator mediator, UserManager<ApplicationUser> user)
        {
            _userManager = userManager;
            _repository = repository;
            _mediator = mediator;
            _user = user;
        }

        public async Task<UserManagerDetailsDto> Handle(GetUserManagerByIdQuery request, CancellationToken cancellationToken)
        {
            request.ThrowIfNull(nameof(request));

            var user = await _userManager.GetByIdAsync(request.Id);
            var reff = await _userManager.GetReferralInfoByPhoneAsync(user.RefferedBy);
            string last10DigitsPhoneNumber1 = "";
            try { 
             last10DigitsPhoneNumber1 = user.PhoneNumber.Substring(Math.Max(0, user.PhoneNumber.Length - 10));

            }catch(Exception ex) { }
            UserManagerDetailsDto outcome = new UserManagerDetailsDto
            {
                Id = user.Id,
                Fullname = user.Fullname,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                CreationUTC = user.CreationUTC, // Replace with the actual property in ApplicationUser
                AccountStatus = user.AccountStatus,
                LastLoggedInAtUtc = user.LastLoggedInAtUtc,
                Surname = user.Surname,
                Firstname = user.Firstname, Lastname = user.Lastname,
                IsCompany = user.IsCompany,
                RefferedBy = reff.Fullname,
                PhoneOfRefferedBy = user.RefferedBy,
                Tier = user.Tier,
                RequestDateTie2Upgraded = user.RequestDateTie2Upgraded,
                About = user.About,
                AlternativePhone = user.AlternativePhone,
                Address = user.Address,
                State = user.State,
                LGA = user.LGA,
                Occupation = user.Occupation,
                FbHandle = user.FbHandle,
                InstagramHandle = user.InstagramHandle,
                TwitterHandle = user.TwitterHandle,
                TiktokHandle = user.TiktokHandle,
                IDCardKey = user.IDCardKey,
                IDCardUrl = user.IDCardUrl,
                PassportUrl = user.PassportUrl, 
                PassportKey = user.PassportKey,
                AccountName = user.AccountName,
                AccountNumber = user.AccountNumber,
                BankName    = user.BankName,
                BVN = user.BVN,
                DateUpgraded = user.DateUpgraded,
                ResponseOnCsaRequest = user.ResponseOnCsaRequest,
                CsaRequest = user.CsaRequest, 
                IsCSARole = user.IsCSARole,
                DateOfBirth = user.DateOfBirth,
                StateOfOrigin = user.StateOfOrigin,
                LGA_Of_Origin = user.LGA_Of_Origin,
                ResponseOnTieRequest = user.ResponseOnTieRequest,
                Tie2Request = user.Tie2Request,
                TwoFactorEnable = user.TwoFactorEnable,
                EmailComfirmed = user.EmailComfirmed,
                RefCode = last10DigitsPhoneNumber1,
               
            };

            

            return outcome;
        }
    }
}

