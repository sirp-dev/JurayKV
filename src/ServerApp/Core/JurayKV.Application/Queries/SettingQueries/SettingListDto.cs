using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static JurayKV.Domain.Primitives.Enum;

namespace JurayKV.Application.Queries.SettingQueries
{
    public class SettingListDto
    {
        public Guid Id { get; set; }
        [Display(Name = "Default Amount Per View")]
        public decimal DefaultAmountPerView { get; set; }

        [Display(Name = "Minimum Amount Budget")]
        public decimal MinimumAmountBudget { get; set; }

        public PaymentGateway PaymentGateway { get; set; }
        public BillGateway BillGateway { get; set; }
        public int SendCount { get; set; }
        public bool DisableReferralBonus { get; set; }
        public bool DisableAirtime { get; set; }
        public bool DisableData { get; set; }
        public bool DisableElectricity { get; set; }
        public bool DisableBetting { get; set; }
        public bool DisableTV { get; set; }

        public decimal AirtimeMaxRechargeTieOne { get; set; }
        public decimal AirtimeMaxRechargeTieTwo { get; set; }
        public decimal AirtimeMinRecharge { get; set; }


        public string? BankAccount { get; set; }
        public string? BankName { get; set; }
        public string? BankAccountNumber { get; set; }
         

    }
}
