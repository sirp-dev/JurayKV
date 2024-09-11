using JurayKV.Application.Caching.Repositories;
using JurayKV.Application.Queries.SettingQueries;
using JurayKV.Domain.Aggregates.SettingAggregate;
using JurayKV.Persistence.Cache.Keys;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TanvirArjel.EFCore.GenericRepository;
using TanvirArjel.Extensions.Microsoft.Caching;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace JurayKV.Persistence.Cache.Repositories
{
    public sealed class SettingCacheRepository : ISettingCacheRepository
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IQueryRepository _repository;
        private readonly ISettingRepository _settingRepository;

        public SettingCacheRepository(IDistributedCache distributedCache, IQueryRepository repository, ISettingRepository settingRepository)
        {
            _distributedCache = distributedCache;
            _repository = repository;
            _settingRepository = settingRepository;
        }

        public async Task<List<SettingDetailsDto>> GetListAsync()
        {
            string cacheKey = SettingCacheKeys.ListKey;
            List<SettingDetailsDto> list = await _distributedCache.GetAsync<List<SettingDetailsDto>>(cacheKey);

            if (list == null)
            {
                Expression<Func<Setting, SettingDetailsDto>> selectExp = d => new SettingDetailsDto
                {
                    Id = d.Id,
                    DefaultAmountPerView = d.DefaultAmountPerView,
                    MinimumAmountBudget = d.MinimumAmountBudget,
                    DefaultReferralAmmount = d.DefaultReferralAmmount,
                    SendCount = d.SendCount,
                    PaymentGateway = d.PaymentGateway,
                    BillGateway = d.BillGateway,
                    DisableAirtime = d.DisableAirtime,
                    DisableBetting = d.DisableBetting,
                    DisableData = d.DisableData,
                    DisableElectricity = d.DisableElectricity,
                    DisableReferralBonus = d.DisableReferralBonus,
                    DisableTV = d.DisableTV,


                    AirtimeMaxRechargeTieOne = d.AirtimeMaxRechargeTieOne,
                    AirtimeMaxRechargeTieTwo = d.AirtimeMaxRechargeTieTwo,
                    AirtimeMinRecharge = d.AirtimeMinRecharge,

                    BankAccount = d.BankAccount,
                    BankAccountNumber = d.BankAccountNumber,
                    BankName = d.BankName,
                     
                };

                list = await _repository.GetListAsync(selectExp);

                await _distributedCache.SetAsync(cacheKey, list);
            }

            return list;
        }

        public async Task<SettingDetailsDto> GetByIdAsync(Guid settingId)
        {
            string cacheKey = SettingCacheKeys.GetKey(settingId);
            SettingDetailsDto setting = await _distributedCache.GetAsync<SettingDetailsDto>(cacheKey);
            if (setting == null)
            {
                Expression<Func<Setting, SettingDetailsDto>> selectExp = d => new SettingDetailsDto
                {
                    Id = d.Id,
                    DefaultAmountPerView = d.DefaultAmountPerView,
                    MinimumAmountBudget = d.MinimumAmountBudget,
                    DefaultReferralAmmount = d.DefaultReferralAmmount,
                    PaymentGateway = d.PaymentGateway,
                    BillGateway = d.BillGateway,
                    SendCount = d.SendCount,

                    DisableAirtime = d.DisableAirtime,
                    DisableBetting = d.DisableBetting,
                    DisableData = d.DisableData,
                    DisableElectricity = d.DisableElectricity,
                    DisableReferralBonus = d.DisableReferralBonus,
                    DisableTV = d.DisableTV,

                    AirtimeMaxRechargeTieOne = d.AirtimeMaxRechargeTieOne,
                    AirtimeMaxRechargeTieTwo = d.AirtimeMaxRechargeTieTwo,
                    AirtimeMinRecharge = d.AirtimeMinRecharge,

                    BankAccount = d.BankAccount,
                    BankAccountNumber = d.BankAccountNumber,
                    BankName = d.BankName,
                     
                };

                setting = await _repository.GetByIdAsync(settingId, selectExp);

                await _distributedCache.SetAsync(cacheKey, setting);
            }

            return setting;

        }

        public async Task<SettingDetailsDto> GetSettingAsync()
        {

            var d = await _settingRepository.GetSettingAsync();
            if (d != null)
            {
                var setting = new SettingDetailsDto
                {
                    Id = d.Id,
                    DefaultAmountPerView = d.DefaultAmountPerView,
                    MinimumAmountBudget = d.MinimumAmountBudget,
                    PaymentGateway = d.PaymentGateway,
                    BillGateway = d.BillGateway,
                    DefaultReferralAmmount = d.DefaultReferralAmmount,
                    SendCount = d.SendCount,
                    DisableAirtime = d.DisableAirtime,
                    DisableBetting = d.DisableBetting,
                    DisableData = d.DisableData,
                    DisableElectricity = d.DisableElectricity,
                    DisableReferralBonus = d.DisableReferralBonus,
                    DisableTV = d.DisableTV,


                    AirtimeMaxRechargeTieOne = d.AirtimeMaxRechargeTieOne,
                    AirtimeMaxRechargeTieTwo = d.AirtimeMaxRechargeTieTwo,
                    AirtimeMinRecharge = d.AirtimeMinRecharge,
                    BankAccount = d.BankAccount,
                    BankAccountNumber = d.BankAccountNumber,
                    BankName = d.BankName,
                     
                };
                return setting;
            }
            return null;
        }
    }

}
