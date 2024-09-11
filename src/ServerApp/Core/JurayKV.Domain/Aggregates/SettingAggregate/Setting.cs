using JurayKV.Domain.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static JurayKV.Domain.Primitives.Enum;

namespace JurayKV.Domain.Aggregates.SettingAggregate
{
    public sealed class Setting : AggregateRoot
    {
        public Setting(Guid id)
        {
            Id = id;

        }

        // This is needed for EF Core query mapping or deserialization.
        public Setting()
        {
        }

        public int SendCount { get; set; }
        public decimal DefaultAmountPerView { get; set; }
        public decimal MinimumAmountBudget { get; set; }
        public decimal DefaultReferralAmmount { get; set; }
        public PaymentGateway PaymentGateway { get; set; }
        public BillGateway BillGateway { get; set; }
        public bool DisableReferralBonus { get; set; }
        public bool DisableAirtime {  get; set; }
        public bool DisableData {  get; set; }
        public bool DisableElectricity {  get; set; }
        public bool DisableBetting {  get; set; }
        public bool DisableTV {  get; set; }

        public decimal AirtimeMaxRechargeTieOne { get; set; }
        public decimal AirtimeMaxRechargeTieTwo { get; set; }
        public decimal AirtimeMinRecharge { get; set; }


        public string? BankAccount { get; set; }    
        public string? BankName { get; set; }    
        public string? BankAccountNumber { get; set; }
         
    }
}
