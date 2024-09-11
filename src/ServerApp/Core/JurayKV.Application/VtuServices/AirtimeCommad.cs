using JurayKV.Application.Commands.TransactionCommands;
using JurayKV.Application.Commands.WalletCommands;
using JurayKV.Application.Queries.TransactionQueries;
using JurayKV.Application.Queries.WalletQueries;
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
    public sealed class AirtimeCommad : IRequest<AirtimeResponse>
    {
        public AirtimeCommad(string phone, string network, string amount, string userId)
        {
            PhoneNumber = phone;
            Network = network;
            Amount = amount;
            UserId = userId;
        }

        public string PhoneNumber { get; set; }
        public string Network { get; set; }
        public string Amount { get; set; }
        public string UserId { get; set; }

    }

    internal class AirtimeCommadHandler : IRequestHandler<AirtimeCommad, AirtimeResponse>
    {
        private readonly IVtuService _vtuService;
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public AirtimeCommadHandler(

                IVtuService vtuService, IMediator mediator, IHttpContextAccessor httpContextAccessor)
        {
            _vtuService = vtuService;
            _mediator = mediator;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<AirtimeResponse> Handle(AirtimeCommad request, CancellationToken cancellationToken)
        {
            _ = request.ThrowIfNull(nameof(request));
            AirtimeResponse result = new AirtimeResponse();
            GetTransactionSumAirtimeAboveThousand tieCommand = new GetTransactionSumAirtimeAboveThousand("AIRTIME", Guid.Parse(request.UserId));
            var reachLimit = await _mediator.Send(tieCommand);
            if (reachLimit == false)
            {
                decimal Amount = 0;
                try
                {
                    Amount = Convert.ToDecimal(request.Amount);
                }
                catch (Exception c)
                {
                    return null;
                }
                AirtimeRequest data = new AirtimeRequest();
                data.PhoneNumber = request.PhoneNumber;
                data.Network = request.Network;
                data.Amount = request.Amount;
                 result = await _vtuService.Airtime(data);
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
                        Guid transaction = Guid.NewGuid();
                        try { 
                        //create transaction
                        CreateTransactionCommand createtransaction = new CreateTransactionCommand(userwallet.Id, userwallet.UserId, "AIRTIME", "", Amount, TransactionTypeEnum.Debit, EntityStatus.Successful, result.data.order_id, "AIRTIME PURCHASE", result.data.order_id);
                         transaction = await _mediator.Send(createtransaction);
                        }catch(Exception c)
                        {

                        }
                        //get the transaction by id
                       
                        GetTransactionByIdQuery gettranCommand = new GetTransactionByIdQuery(transaction);
                        var thetransaction = await _mediator.Send(gettranCommand);
                        //update wallet


                        userwallet.Amount = userwallet.Amount - Amount;

                        var loguserId = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
                        userwallet.Log = "<li>Wallet Update from " + thetransaction.Description + " " + thetransaction.Id + " ::Amount: " + thetransaction.Amount + " ::Balance: " + userwallet.Amount + " :: Date: " + userwallet.LastUpdateAtUtc + ":: Loggedin User: " + loguserId + "</li>" + userwallet.Log ;
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
                    CreateTransactionCommand createtransaction = new CreateTransactionCommand(userwallet.Id, userwallet.UserId, "AIRTIME", "", Amount, TransactionTypeEnum.Debit, EntityStatus.Pending, "XXXXXXX", "AIRTIME PURCHASE", "XXXXXX");
                    var transaction = await _mediator.Send(createtransaction);

                    //get the transaction by id
                    GetTransactionByIdQuery gettranCommand = new GetTransactionByIdQuery(transaction);
                    var thetransaction = await _mediator.Send(gettranCommand);
                    //update wallet


                    userwallet.Amount = userwallet.Amount - Amount;

                    var loguserId = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
                    userwallet.Log = "<li>Wallet Update from " + thetransaction.Description + " " + thetransaction.Id + " ::Amount: " + thetransaction.Amount + " ::Balance: " + userwallet.Amount + " :: Date: " + userwallet.LastUpdateAtUtc + ":: Loggedin User: " + loguserId + "</li>" + userwallet.Log;
                    //userwallet = null;
                    UpdateWalletCommand updatewalletcommand = new UpdateWalletCommand(userwallet.UserId, "Validate Transaction", userwallet.Log, userwallet.Amount);
                    await _mediator.Send(updatewalletcommand);
                }
            }
            else
            {
                result.code = "limit";
            }
            return result;
        }
    }
}
