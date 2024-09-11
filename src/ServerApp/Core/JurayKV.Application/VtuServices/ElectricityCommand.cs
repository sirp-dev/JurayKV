using JurayKV.Application.Commands.TransactionCommands;
using JurayKV.Application.Commands.WalletCommands;
using JurayKV.Application.Queries.TransactionQueries;
using JurayKV.Application.Queries.WalletQueries;
using JurayKV.Domain.Aggregates.VariationAggregate;
using JurayKV.Infrastructure.VTU.Repository;
using JurayKV.Infrastructure.VTU.RequestDto;
using JurayKV.Infrastructure.VTU.ResponseDto;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanvirArjel.ArgumentChecker;
using static JurayKV.Domain.Primitives.Enum;

namespace JurayKV.Application.VtuServices
{
    public sealed class ElectricityCommand : IRequest<ElectricityResponse>
    {
        public ElectricityCommand(string phone, string meternumber, string serviceid, string variationid, string amount, string userId)
        {
            PhoneNumber = phone;
            MeterNumber = meternumber;
            ServiceId = serviceid;
            VariationId = variationid;
            Amount = amount;
            UserId = userId;
        }

        public string PhoneNumber { get; set; }
        public string MeterNumber { get; set; }
        public string ServiceId { get; set; }
        public string VariationId { get; set; }
        public string Amount { get; set; }
        public string UserId { get; set; }

    }
    internal class ElectricityCommandHandler : IRequestHandler<ElectricityCommand, ElectricityResponse>
    {
        private readonly IVtuService _vtuService;
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IVariationRepository _variationRepository;


        public ElectricityCommandHandler(
                IVtuService vtuService, IMediator mediator, IHttpContextAccessor httpContextAccessor, IVariationRepository variationRepository)
        {
            _vtuService = vtuService;
            _mediator = mediator;
            _httpContextAccessor = httpContextAccessor;
            _variationRepository = variationRepository;
        }

        public async Task<ElectricityResponse> Handle(ElectricityCommand request, CancellationToken cancellationToken)
        {
            _ = request.ThrowIfNull(nameof(request));
            //get variation amount
             ElectricityRequest data = new ElectricityRequest();
            data.PhoneNumber = request.PhoneNumber;
            data.MeterNumber = request.MeterNumber;
            data.ServiceId = request.ServiceId;
            data.VariationId = request.VariationId;
            data.Amount = request.Amount;
            var result = await _vtuService.Electricity(data);
            //getwallet
            GetWalletUserByIdQuery getwalletcommand = new GetWalletUserByIdQuery(Guid.Parse(request.UserId));
            var userwallet = await _mediator.Send(getwalletcommand);
            if (userwallet == null)
            {
                //send log request
            }
            if (result.code == "success")
            {
                try
                {


                    //create transaction
                    CreateTransactionCommand createtransaction = new CreateTransactionCommand(userwallet.Id, userwallet.UserId, "ELECTRICITY", "", Convert.ToDecimal(request.Amount), TransactionTypeEnum.Debit, EntityStatus.Successful, result.data.order_id, "ELECTRICITY PURCHASE <br>" + result.data.token, result.data.order_id);
                    var transaction = await _mediator.Send(createtransaction);

                    //get the transaction by id
                    GetTransactionByIdQuery gettranCommand = new GetTransactionByIdQuery(transaction);
                    var thetransaction = await _mediator.Send(gettranCommand);
                    //update wallet
                    result.TransactionId = thetransaction.Id;

                    userwallet.Amount = userwallet.Amount - Convert.ToDecimal(request.Amount);

                    var loguserId = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
                    userwallet.Log = "<li>Wallet Update from " + thetransaction.Description + " " + thetransaction.Id + " ::Amount: " + thetransaction.Amount + " ::Balance: " + userwallet.Amount + " :: Date: " + userwallet.LastUpdateAtUtc + ":: Loggedin User: " + loguserId + "</li>" + userwallet.Log;
                    //userwallet = null;
                    UpdateWalletCommand updatewalletcommand = new UpdateWalletCommand(userwallet.UserId, "Validate Transaction", userwallet.Log, userwallet.Amount);
                    await _mediator.Send(updatewalletcommand);

                }
                catch (Exception c)
                {
                    //loog error
                }
            }
            else if (result.code == "processing")
            {
                //create transaction
                CreateTransactionCommand createtransaction = new CreateTransactionCommand(userwallet.Id, userwallet.UserId, "ELECTRICITY", "", Convert.ToDecimal(request.Amount), TransactionTypeEnum.Debit, EntityStatus.Pending, "XXXXXXX", "ELECTRICITY PURCHASE", "XXXXXX");
                var transaction = await _mediator.Send(createtransaction);

                //get the transaction by id
                GetTransactionByIdQuery gettranCommand = new GetTransactionByIdQuery(transaction);
                var thetransaction = await _mediator.Send(gettranCommand);
                //update wallet

                result.TransactionId = thetransaction.Id;

                userwallet.Amount = userwallet.Amount - Convert.ToDecimal(request.Amount);

                var loguserId = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
                userwallet.Log =   "<li>Wallet Update from " + thetransaction.Description + " " + thetransaction.Id + " ::Amount: " + thetransaction.Amount + " ::Balance: " + userwallet.Amount + " :: Date: " + userwallet.LastUpdateAtUtc + ":: Loggedin User: " + loguserId +"</li>"+ userwallet.Log;
                //userwallet = null;
                UpdateWalletCommand updatewalletcommand = new UpdateWalletCommand(userwallet.UserId, "Validate Transaction", userwallet.Log, userwallet.Amount);
                await _mediator.Send(updatewalletcommand);
            }

            return result;
        }
    }

    
}
