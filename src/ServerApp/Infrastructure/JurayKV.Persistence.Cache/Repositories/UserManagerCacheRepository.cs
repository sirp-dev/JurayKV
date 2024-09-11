using JurayKV.Application.Caching.Repositories;
using JurayKV.Application.Queries.IdentityKvAdQueries;
using JurayKV.Application.Queries.KvPointQueries;
using JurayKV.Application.Queries.TransactionQueries;
using JurayKV.Application.Queries.UserAccountQueries.DashboardQueries;
using JurayKV.Application.Queries.UserManagerQueries;
using JurayKV.Application.Queries.WalletQueries;
using JurayKV.Domain.Aggregates.IdentityAggregate;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using TanvirArjel.EFCore.GenericRepository;
using static JurayKV.Domain.Primitives.Enum;

namespace JurayKV.Persistence.Cache.Repositories
{
    public sealed class UserManagerCacheRepository : IUserManagerCacheRepository
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IQueryRepository _repository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWalletCacheRepository _walletCacheRepository;
        private readonly ITransactionCacheRepository _transactionCacheRepository;
        private readonly IKvPointCacheRepository _kvPointCacheRepository;

        public UserManagerCacheRepository(IDistributedCache distributedCache, IQueryRepository repository, UserManager<ApplicationUser> userManager, IWalletCacheRepository walletCacheRepository, ITransactionCacheRepository transactionCacheRepository, IKvPointCacheRepository kvPointCacheRepository)
        {
            _distributedCache = distributedCache;
            _repository = repository;
            _userManager = userManager;
            _walletCacheRepository = walletCacheRepository;
            _transactionCacheRepository = transactionCacheRepository;
            _kvPointCacheRepository = kvPointCacheRepository;
        }
        public async Task<int> GetListReferralCountAsync(string myphone)
        {
            string last10DigitsPhoneNumber1 = myphone.Substring(Math.Max(0, myphone.Length - 10));
            var userlist = await _userManager.Users.Where(x => x.RefferedByPhoneNumber.Contains(last10DigitsPhoneNumber1)).CountAsync();

            return userlist;
        }
        public async Task<List<UserManagerListDto>> GetListReferralAsync(string myphone)
        {
            if(myphone == null)
            {
                List<UserManagerListDto> listing = new List<UserManagerListDto>();
                return listing;
            }
            string last10DigitsPhoneNumber1 = myphone.Substring(Math.Max(0, myphone.Length - 10));
            var userlist = await _userManager.Users.Where(x => x.RefferedByPhoneNumber.Contains(last10DigitsPhoneNumber1)).ToListAsync();
            // Manual mapping from entities to DTOs
            var list = userlist.Select(entity => new UserManagerListDto
            {
                Id = entity.Id,
                Date = entity.CreationUTC,
                IdNumber = entity.IdNumber,
                Fullname = entity.SurName + " " + entity.FirstName + " " + entity.LastName,
                AccountStatus = entity.AccountStatus,
                Email = entity.Email,
                PhoneNumber = entity.PhoneNumber,
                LastLoggedInAtUtc = entity.LastLoggedInAtUtc,
                CreationUTC = entity.CreationUTC,
                Verified = entity.EmailConfirmed,
                Posted = entity.Posted,
                VideoUpload = entity.VideoUpload,
                SuccessPoint = entity.SuccessPoint,
                Role = entity.Role,
            }).ToList();

            return list.OrderByDescending(x=>x.CreationUTC).ToList();
        }
        public async Task<List<UserManagerListDto>> GetListByStatusAsync(AccountStatus status)
        {
            var userlist = new List<ApplicationUser>();
            if (status == AccountStatus.NotDefind)
            {
                userlist = await _userManager.Users.ToListAsync();

            }
            else
            {
                userlist = await _userManager.Users.Where(x => x.AccountStatus == status).ToListAsync();

            }
            var list = userlist.Select(async entity => new UserManagerListDto
            {
                Id = entity.Id,
                Date = entity.CreationUTC,
                IdNumber = entity.IdNumber,
                Fullname = entity.SurName + " " + entity.FirstName + " " + entity.LastName,
                AccountStatus = entity.AccountStatus,
                Email = entity.Email,
                PhoneNumber = entity.PhoneNumber,
                LastLoggedInAtUtc = entity.LastLoggedInAtUtc,
                CreationUTC = entity.CreationUTC,
                Verified = entity.EmailConfirmed,
                VerificationCode = entity.VerificationCode,
                Role = entity.Role,
                ReferralCount = await GetListReferralCountAsync(entity.PhoneNumber)
            });
            var xlist = (await Task.WhenAll(list)).ToList();
            return xlist;
        }

        public async Task<UserListPagedDto> GetListByStatusAndDateAsync(AccountStatus status, DateTime? startdate, DateTime? enddate, int pageSize, int pageNumber, string? searchstring, int sortOrder)
        {
            var userlist = new List<ApplicationUser>();
            var filteredUsers = _userManager.Users.AsQueryable();
             
            // Step 1: Sort the numbers
            var sortedPhoneNumbers = filteredUsers
                .OrderBy(u => u.PhoneNumber)
                .Select(u => u.PhoneNumber)
                .ToList();

            // Step 2: Take the last 10 digits and remove spaces
            var cleanedPhoneNumbers = sortedPhoneNumbers
                .Select(phone => new string(phone.Replace(" ", "").ToArray()))
                .Select(phone => phone.Substring(Math.Max(0, phone.Length - 10)))
                .ToList();

            // Step 3: Find duplicates
            var duplicates = cleanedPhoneNumbers.GroupBy(phone => phone).Where(g => g.Count() > 1).ToList();


            if (status == AccountStatus.NotDefind)
            {
                if (startdate != null)
                {
                    userlist = await filteredUsers.Where(x => x.CreationUTC.Date >= startdate.Value.Date && x.CreationUTC.Date <= enddate.Value.Date).ToListAsync();
                }
                else
                {
                    userlist = await filteredUsers.ToListAsync();

                }
            }
            else
            {
                if (startdate != null)
                {
                    userlist = await filteredUsers.Where(x => x.AccountStatus == status).Where(x => x.CreationUTC.Date >= startdate.Value.Date && x.CreationUTC.Date <= enddate.Value.Date).ToListAsync();

                }
                else
                {
                    userlist = await filteredUsers.Where(x => x.AccountStatus == status).ToListAsync();

                }

            }
            if (!string.IsNullOrEmpty(searchstring))
            {
                // Add search criteria based on your requirements
                userlist = userlist
            .Where(x =>
                (!string.IsNullOrEmpty(x.FirstName) && x.FirstName.ToLower().Contains(searchstring.ToLower())) ||
                (!string.IsNullOrEmpty(x.LastName) && x.LastName.ToLower().Contains(searchstring.ToLower())) ||
                (!string.IsNullOrEmpty(x.PhoneNumber) && x.PhoneNumber.ToLower().Contains(searchstring.Replace(" ", "").Substring(Math.Max(0, searchstring.Length - 10)))) ||
                 (!string.IsNullOrEmpty(x.State) && x.State.ToLower().Contains(searchstring.ToLower())) ||
                (!string.IsNullOrEmpty(x.LGA) && x.LGA.ToLower().Contains(searchstring.ToLower())) ||
                (!string.IsNullOrEmpty(x.AccountName) && x.AccountName.ToLower().Contains(searchstring.ToLower())) ||
                (!string.IsNullOrEmpty(x.AccountNumber) && x.AccountNumber.ToLower().Contains(searchstring.ToLower())) ||
                (!string.IsNullOrEmpty(x.BVN) && x.BVN.ToLower().Contains(searchstring.ToLower())) ||
                (!string.IsNullOrEmpty(x.BankName) && x.BankName.ToLower().Contains(searchstring.ToLower())) ||
                (!string.IsNullOrEmpty(x.Occupation) && x.Occupation.ToLower().Contains(searchstring.ToLower())) ||
                (!string.IsNullOrEmpty(x.Email) && x.Email.ToLower().Contains(searchstring.ToLower())))
            .ToList();



            }
            UserListPagedDto data = new UserListPagedDto();
            var list = new List<UserManagerListDto>();
            foreach (var entity in userlist.Skip((pageNumber - 1) * pageSize).Take(pageSize))
            {
                var referralCount = await GetListReferralCountAsync(entity.PhoneNumber);
                var walletbalance = await _walletCacheRepository.GetByUserIdAsync(entity.Id);
                var gettotalTransactions = await _transactionCacheRepository.GetListByUserIdAsync(entity.Id);
                var totaltransactionDebit = gettotalTransactions.Where(x => x.TransactionType == TransactionTypeEnum.Debit).Sum(x => x.Amount);
                var totaltransactionCredit = gettotalTransactions.Where(x => x.TransactionType == TransactionTypeEnum.Credit).Sum(x => x.Amount);
                var totaltransactionCount = gettotalTransactions.Count();
                var referraltransaction = await _transactionCacheRepository.GetReferralListByUserIdAsync(entity.Id);
                var totalRefTransaction = referraltransaction.Sum(x => x.Amount);
                var totalpoints = await _kvPointCacheRepository.GetListByUserIdAsync(entity.Id);
                var sumpoints = totalpoints.Sum(x => x.Point);
                var dto = new UserManagerListDto
                {
                    Id = entity.Id,
                    Date = entity.CreationUTC,
                    IdNumber = entity.IdNumber,
                    Fullname = entity.SurName + " " + entity.FirstName + " " + entity.LastName,
                    AccountStatus = entity.AccountStatus,
                    Email = entity.Email,
                    PhoneNumber = entity.PhoneNumber,
                    LastLoggedInAtUtc = entity.LastLoggedInAtUtc,
                    CreationUTC = entity.CreationUTC,
                    Verified = entity.EmailConfirmed,
                    VerificationCode = entity.VerificationCode,
                    Role = entity.Role,
                    ReferralCount = referralCount,
                    WalletBalance = walletbalance.Amount,
                    TotalTransactionCredit = totaltransactionCredit,
                    TotalTransactionDebit = totaltransactionDebit,
                    TotalPoints = sumpoints,
                    TotalReferralAmount = totalRefTransaction,
                    Tier = entity.Tier,
                    RequestDateTie2Upgraded = entity.RequestDateTie2Upgraded, 
                };

                list.Add(dto);
            }
            if (sortOrder == 1)
            {
                data.UserManagerListDto = list.OrderBy(x => x.Fullname).ToList();
            }else if (sortOrder == 0)
            {
                data.UserManagerListDto = list.OrderByDescending(x => x.CreationUTC).ToList();
            }
            else if (sortOrder == 2)
            {
                data.UserManagerListDto = list.OrderBy(x => x.Email).ToList();
            }
            else if (sortOrder == 3)
            {
                data.UserManagerListDto = list.OrderBy(x => x.PhoneNumber).ToList();
            }
            else if (sortOrder == 4)
            {
                data.UserManagerListDto = list.OrderByDescending(x => x.CreationUTC).ToList();
            }
            else if (sortOrder == 5)
            {
                data.UserManagerListDto = list.OrderByDescending(x => x.WalletBalance).ToList();
            }
            else if (sortOrder == 6)
            {
                data.UserManagerListDto = list.OrderByDescending(x => x.LastLoggedInAtUtc).ToList();
            }
            else if (sortOrder == 7)
            {
                data.UserManagerListDto = list.OrderByDescending(x => x.TotalTransactionCredit).ToList();
            }
            else if (sortOrder == 14)
            {
                data.UserManagerListDto = list.OrderByDescending(x => x.TotalTransactionDebit).ToList();
            }
            else if (sortOrder == 8)
            {
                data.UserManagerListDto = list.OrderByDescending(x => x.TotalPoints).ToList();
            }
            else if (sortOrder == 9)
            {
                data.UserManagerListDto = list.OrderBy(x => x.Role).ToList();
            }
            else if (sortOrder == 10)
            {
                data.UserManagerListDto = list.OrderByDescending(x => x.AccountStatus).ToList();
            }
            else if (sortOrder == 11)
            {
                data.UserManagerListDto = list.OrderByDescending(x => x.ReferralCount).ToList();
            }
            else if (sortOrder == 12)
            {
                data.UserManagerListDto = list.OrderByDescending(x => x.TotalReferralAmount).ToList();
            }
            else if (sortOrder == 13)
            {
                data.UserManagerListDto = list.OrderBy(x => x.Verified).ToList();
            }
            else if (sortOrder == 15)
            {
                data.UserManagerListDto = list.OrderByDescending(x => x.Tier).ToList();
            }
            else if (sortOrder == 16)
            {
                data.UserManagerListDto = list.OrderBy(x => x.RequestDateTie2Upgraded).ToList();
            }
     //       else if (sortOrder == 17)
     //       { 
     //           data.UserManagerListDto = list
     //.OrderByDescending(x => int.Parse(x.IdNumber.TrimStart('0')))
     //.ToList();
     //       }
            data.TotalCount = userlist.Count;

            // Combine emails and counts into a string
            // Step 4: List the duplicates in the specified format
            string phoneNumberResult = "";
            foreach (var group in duplicates)
            {
                phoneNumberResult += $" ({group.Key} - {group.Count()})";
            }


            data.DistintPhone = phoneNumberResult;
            data.DistintPhoneCountActive = filteredUsers.Where(x => x.PhoneNumber == data.DistintPhone && x.AccountStatus == AccountStatus.Active).Count();
            data.DistintEmailCountActive = filteredUsers.Where(x => x.Email == data.DistintEmail && x.AccountStatus == AccountStatus.Active).Count();


            return data;
        }
        public static string CleanPhoneNumber(string phoneNumber)
        {
            if (phoneNumber == null)
            {
                return null;
            }

            // Remove non-digit characters and take the last 10 digits

            string xphoneNumber = new string(phoneNumber.Replace(" ", "")).Substring(Math.Max(0, phoneNumber.Length - 10));
            return xphoneNumber;
        }
        public async Task<List<UserManagerListDto>> GetListAsync()
        {
            
            var userlist = await _userManager.Users.ToListAsync();
             var list = userlist.Select(entity => new UserManagerListDto
            {
                Id = entity.Id,
                Date = entity.CreationUTC,
                IdNumber = entity.IdNumber,
                Fullname = entity.SurName + " " + entity.FirstName + " " + entity.LastName,
                AccountStatus = entity.AccountStatus,
                Email = entity.Email,
                PhoneNumber = entity.PhoneNumber,
                LastLoggedInAtUtc = entity.LastLoggedInAtUtc,
                CreationUTC = entity.CreationUTC,
                Verified = entity.EmailConfirmed,
                VerificationCode = entity.VerificationCode,
                 Role = entity.Role,
                 Tier = entity.Tier,
                 // Map other properties as needed
             }).ToList();
            //    await _distributedCache.SetAsync(cacheKey, list);
            //}

            return list;
        }
        public async Task<UserManagerDetailsDto> GetReferralInfoByPhoneAsync(string phone)
        {
            try
            {
                UserManagerDetailsDto userManager = new UserManagerDetailsDto();
                if (phone != null)
                {
                    string last10DigitsPhoneNumber1 = phone.Substring(Math.Max(0, phone.Length - 10));
                    var entity = await _userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber.Contains(last10DigitsPhoneNumber1));
                    if (entity != null)
                    {
                        userManager = new UserManagerDetailsDto
                        {
                            Id = entity.Id,

                            Fullname = entity.SurName + " " + entity.FirstName + " " + entity.LastName,

                            Email = entity.Email,
                            PhoneNumber = entity.PhoneNumber,

                        };
                    }
                }
                //    await _distributedCache.SetAsync(cacheKey, userManager);
                //}

                return userManager;
            }
            catch (Exception a)
            {
                return null;
            }
        }

        public async Task<UserManagerDetailsDto> GetByIdAsync(Guid userManagerId)
        {
            //string cacheKey = UserManagerCacheKeys.GetDetailsKey(userManagerId);
            //UserManagerDetailsDto userManager = await _distributedCache.GetAsync<UserManagerDetailsDto>(cacheKey);
            UserManagerDetailsDto userManager = new UserManagerDetailsDto();

            //if (userManager == null)
            //{
            var entity = await _userManager.FindByIdAsync(userManagerId.ToString());
            if (entity != null)
            {
                userManager = new UserManagerDetailsDto
                {
                    Id = entity.Id,
                    CreationUTC = entity.CreationUTC,
                    IdNumber = entity.IdNumber,
                    Fullname = entity.SurName + " " + entity.FirstName + " " + entity.LastName,
                    AccountStatus = entity.AccountStatus,
                    Email = entity.Email,
                    PhoneNumber = entity.PhoneNumber,
                    LastLoggedInAtUtc = entity.LastLoggedInAtUtc,
                    Surname = entity.SurName,
                    Firstname = entity.FirstName,
                    Lastname = entity.LastName,
                    RefferedBy = entity.RefferedByPhoneNumber,
                    IsCompany = await _userManager.IsInRoleAsync(entity, "Company"),
                    IsCSARole = await _userManager.IsInRoleAsync(entity, "CSA"),

                    Tier = entity.Tier,
                    RequestDateTie2Upgraded = entity.RequestDateTie2Upgraded,
                    About = entity.About,
                    AlternativePhone = entity.AlternativePhone,
                    Address = entity.Address,
                    State = entity.State,
                    LGA = entity.LGA,
                    Occupation = entity.Occupation,
                    FbHandle = entity.FbHandle,
                    InstagramHandle = entity.InstagramHandle,
                    TwitterHandle = entity.TwitterHandle,
                    TiktokHandle = entity.TiktokHandle,
                    IDCardKey = entity.IDCardKey,
                    IDCardUrl = entity.IDCardUrl,
                    PassportUrl = entity.PassportUrl,
                    PassportKey = entity.PassportKey,
                    AccountName = entity.AccountName,
                    AccountNumber = entity.AccountNumber,
                    BankName = entity.BankName,
                    BVN = entity.BVN,
                    DateUpgraded = entity.DateUpgraded,
                    ResponseOnCsaRequest = entity.ResponseOnCsaRequest,
                    CsaRequest = entity.CsaRequest,
                    Role = entity.Role, 
                    EmailComfirmed = entity.EmailConfirmed,
                    TwoFactorEnable = entity.TwoFactorEnabled,
                    NinNumber = entity.NinNumber
                };
            }

            //    await _distributedCache.SetAsync(cacheKey, userManager);
            //}

            return userManager;
        }

        public async Task<UserManagerDetailsDto> GetDetailsByIdAsync(Guid userManagerId)
        {
            //string cacheKey = UserManagerCacheKeys.GetDetailsKey(userManagerId);
            //UserManagerDetailsDto userManager = await _distributedCache.GetAsync<UserManagerDetailsDto>(cacheKey);
            UserManagerDetailsDto userManager = new UserManagerDetailsDto();
            //if (userManager == null)
            //{
            var entity = await _userManager.FindByIdAsync(userManagerId.ToString());
            if (entity != null)
            {
                userManager = new UserManagerDetailsDto
                {
                    Id = entity.Id,
                    IdNumber = entity.IdNumber,
                    CreationUTC = entity.CreationUTC,
                    Fullname = entity.SurName + " " + entity.FirstName + " " + entity.LastName,
                    AccountStatus = entity.AccountStatus,
                    Email = entity.Email,
                    PhoneNumber = entity.PhoneNumber,
                    LastLoggedInAtUtc = entity.LastLoggedInAtUtc,
                    Role = entity.Role,
                    Tier = entity.Tier,
                };
            }

            //    await _distributedCache.SetAsync(cacheKey, userManager);
            //}

            return userManager;
        }

        public async Task<UserDashboardDto> GetUserDashboardDto(Guid userId, CancellationToken cancellationToken)
        {
            var getuser = await _userManager.FindByIdAsync(userId.ToString());

            //
            Specification<KvPointListDto> kvpointspec = new Specification<KvPointListDto>();
            kvpointspec.Conditions.Add(x => x.UserId == userId);
            kvpointspec.Take = 10;
            List<KvPointListDto> pointlist = await _repository.GetListAsync<KvPointListDto>(kvpointspec, cancellationToken);

            //
            Specification<TransactionListDto> transspec = new Specification<TransactionListDto>();
            transspec.Conditions.Add(x => x.UserId == userId);
            transspec.Take = 10;
            List<TransactionListDto> translist = await _repository.GetListAsync<TransactionListDto>(transspec, cancellationToken);

            //

            Specification<IdentityKvAdListDto> useradsspec = new Specification<IdentityKvAdListDto>();
            useradsspec.Conditions.Add(x => x.UserId == userId && x.Active == true);

            int count = await _repository.GetCountAsync<IdentityKvAdListDto>(useradsspec.Conditions, cancellationToken);
            //

            Specification<WalletDetailsDto> walletspec = new Specification<WalletDetailsDto>();
            walletspec.Conditions.Add(x => x.UserId == userId);
            WalletDetailsDto UserWallet = await _repository.GetAsync<WalletDetailsDto>(walletspec, cancellationToken);
             
            return null;
        }

        public async Task<UserListPagedDto> ListGetListByStatusAndDateAsync(AccountStatus status, DateTime? startdate, DateTime? enddate, int pageSize, int pageNumber, string searchstring, int sortOrder)
        {
            var userlist = new List<ApplicationUser>();
            var filteredUsers = _userManager.Users.AsQueryable();

             

            return null;
        }

        public async Task<IEnumerable<UserManagerListDto>> ListAsync(AccountStatus? status)
        {
            if (status == null) { 
            var data = _userManager.Users.AsEnumerable();
            return data.Select(entity => new UserManagerListDto
            {
                Id = entity.Id,
                Date = entity.CreationUTC,
                IdNumber = entity.IdNumber,
                Surname = entity.SurName,
                Firstname = entity.FirstName,
                Lastname = entity.LastName,
                AccountStatus = entity.AccountStatus,
                Email = entity.Email,
                PhoneNumber = entity.PhoneNumber,
                LastLoggedInAtUtc = entity.LastLoggedInAtUtc,
                CreationUTC = entity.CreationUTC,
                Verified = entity.EmailConfirmed,
                VerificationCode = entity.VerificationCode,
                Tie2Request = entity.Tie2Request,
                Role = entity.Role,
                Tier = entity.Tier,
            }).AsEnumerable();
            }
            else
            {
                var data = _userManager.Users.Where(x=>x.AccountStatus == status).AsEnumerable();
                return data.Select(entity => new UserManagerListDto
                {
                    Id = entity.Id,
                    Date = entity.CreationUTC,
                    IdNumber = entity.IdNumber,
                    Surname = entity.SurName,
                    Firstname = entity.FirstName,
                    AccountStatus = entity.AccountStatus,
                    Email = entity.Email,
                    PhoneNumber = entity.PhoneNumber,
                    LastLoggedInAtUtc = entity.LastLoggedInAtUtc,
                    CreationUTC = entity.CreationUTC,
                    Verified = entity.EmailConfirmed,
                    VerificationCode = entity.VerificationCode,
                    Tie2Request = entity.Tie2Request,
                    Role = entity.Role,
                    Tier = entity.Tier,
                }).AsEnumerable();
            }
        }
    }

}
