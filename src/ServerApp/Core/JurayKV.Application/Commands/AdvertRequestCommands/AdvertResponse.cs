using JurayKV.Infrastructure.Flutterwave.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static JurayKV.Domain.Primitives.Enum;

namespace JurayKV.Application.Commands.AdvertRequestCommands
{
    public class AdvertResponse
    {
        public PaymentGateway PaymentGateWay { get; set; }
        public FlutterResponseDto FlutterResponseDto { get;set;}

        public string? BankAccount { get; set; }
        public string? BankName { get; set; }
        public string? BankAccountNumber { get; set; }
    }
}
