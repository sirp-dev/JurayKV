using JurayKV.Application.Caching.Repositories;
using JurayKV.Domain.Aggregates.IdentityAggregate;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanvirArjel.ArgumentChecker;
using TanvirArjel.EFCore.GenericRepository;

namespace JurayKV.Application.Queries.UserManagerQueries
{
 

public sealed class GetUserManagerByPhoneQuery : IRequest<UserManagerDetailsDto>
    {
        public GetUserManagerByPhoneQuery(string phone)
        {
            Phone = phone;
        }

        public string Phone { get; }

        // Handler
        private class GetUserManagerByPhoneQueryHandler : IRequestHandler<GetUserManagerByPhoneQuery, UserManagerDetailsDto>
        {
            private readonly IUserManagerCacheRepository _userManager;
            private readonly IQueryRepository _repository;
            private readonly IMediator _mediator;
            private readonly UserManager<ApplicationUser> _mainUserManager;



            public GetUserManagerByPhoneQueryHandler(IUserManagerCacheRepository userManager, IQueryRepository repository, IMediator mediator, UserManager<ApplicationUser> mainUserManager)
            {
                _userManager = userManager;
                _repository = repository;
                _mediator = mediator;
                _mainUserManager = mainUserManager;
            }

            public async Task<UserManagerDetailsDto> Handle(GetUserManagerByPhoneQuery request, CancellationToken cancellationToken)
            {
                request.ThrowIfNull(nameof(request));
                try { 
                    if(request.Phone != null) { 
                string last10DigitsPhoneNumber1 = request.Phone.Substring(Math.Max(0, request.Phone.Length - 10));
                var user = await _mainUserManager.Users.FirstOrDefaultAsync(x=>x.PhoneNumber.Contains(request.Phone));

                 UserManagerDetailsDto outcome = new UserManagerDetailsDto
                {
                    Id = user.Id,
                    Fullname = user.SurName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    CreationUTC = user.CreationUTC, // Replace with the actual property in ApplicationUser
                    AccountStatus = user.AccountStatus,
                    LastLoggedInAtUtc = user.LastLoggedInAtUtc,
                    Surname = user.SurName,
                    Firstname = user.FirstName,
                    Lastname = user.LastName,  
                };



                return outcome;
                    }
                }catch(Exception c) { }
                return null;
            }
        }
    }

}
