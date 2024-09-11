using JurayKvV.Infrastructure.Interswitch.Repositories;
using JurayKvV.Infrastructure.Interswitch.ResponseModel;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Application.Interswitch
{
    public sealed class ValidateBankAccountCommand : IRequest<AccountVerificationResponse>
    {
        public ValidateBankAccountCommand(string accountNumber, string bankCode)
        {
            AccountNumber = accountNumber;
            BankCode = bankCode;
        }

        public string AccountNumber { get; }
        public string BankCode { get; }
    }

    internal class ValidateBankAccountCommandHandler : IRequestHandler<ValidateBankAccountCommand, AccountVerificationResponse>
    {
        private readonly ISwitchRepository _switchRepository;

        public ValidateBankAccountCommandHandler(ISwitchRepository switchRepository)
        {
            _switchRepository = switchRepository;
        }

        public async Task<AccountVerificationResponse> Handle(ValidateBankAccountCommand request, CancellationToken cancellationToken)
        {
            _ = request.ThrowIfNull(nameof(request));

            var response = await _switchRepository.ValidateBankAccount(request.AccountNumber, request.BankCode);
            return response;
        }
    }
}
