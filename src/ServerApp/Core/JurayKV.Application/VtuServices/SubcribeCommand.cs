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
      public sealed class SubcribeCommand : IRequest<CableTvResponse>
    {
        public SubcribeCommand(string phone, string customerid, string variationid, string serviceid, string userId)
        {
            PhoneNumber = phone; 
            CustomerId = customerid;
            VariationId = variationid; 
            ServiceId = serviceid;
            UserId = userId;
        }

        
        public string PhoneNumber { get; set; }
        public string CustomerId { get; set; }
        public string VariationId { get; set; }
        public string ServiceId { get; set; }
        public string UserId { get; set; }

    }
    internal class SubcribeCommandHandler : IRequestHandler<SubcribeCommand, CableTvResponse>
    {
        private readonly IVtuService _vtuService;
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IVariationRepository _variationRepository;


        public SubcribeCommandHandler(
                IVtuService vtuService, IMediator mediator, IHttpContextAccessor httpContextAccessor, IVariationRepository variationRepository)
        {
            _vtuService = vtuService;
            _mediator = mediator;
            _httpContextAccessor = httpContextAccessor;
            _variationRepository = variationRepository;
        }

        public async Task<CableTvResponse> Handle(SubcribeCommand request, CancellationToken cancellationToken)
        {
            _ = request.ThrowIfNull(nameof(request));
            //get variation amount
            //get variation amount
            var variationData = await _variationRepository.GetByIdAsync(Guid.Parse(request.VariationId));
            CableTvRequest data = new CableTvRequest();
            data.PhoneNumber = request.PhoneNumber;
            data.CustomerId = request.CustomerId;
            data.VariationId = variationData.Code;
            data.ServiceId = request.ServiceId;
            var result = await _vtuService.SubscribeTV(data);
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
                    CreateTransactionCommand createtransaction = new CreateTransactionCommand(userwallet.Id, userwallet.UserId, "TV SUBSCRIPTION", "", Convert.ToDecimal(variationData.Amount), TransactionTypeEnum.Debit, EntityStatus.Successful, result.data.order_id, "TV SUBSCRIPTION", result.data.order_id);
                    var transaction = await _mediator.Send(createtransaction);

                    //get the transaction by id
                    GetTransactionByIdQuery gettranCommand = new GetTransactionByIdQuery(transaction);
                    var thetransaction = await _mediator.Send(gettranCommand);
                    //update wallet


                    userwallet.Amount = userwallet.Amount - Convert.ToDecimal(variationData.Amount);

                    var loguserId = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
                    userwallet.Log =  "<li>Wallet Update from " + thetransaction.Description + " " + thetransaction.Id + " ::Amount: " + thetransaction.Amount + " ::Balance: " + userwallet.Amount + " :: Date: " + userwallet.LastUpdateAtUtc + ":: Loggedin User: " + loguserId + "</li>" + userwallet.Log;
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
                CreateTransactionCommand createtransaction = new CreateTransactionCommand(userwallet.Id, userwallet.UserId, "TV SUBSCRIPTION", "", Convert.ToDecimal(variationData.Amount), TransactionTypeEnum.Debit, EntityStatus.Pending, "XXXXXXX", "TV SUBSCRIPTION", "XXXXXX");
                var transaction = await _mediator.Send(createtransaction);

                //get the transaction by id
                GetTransactionByIdQuery gettranCommand = new GetTransactionByIdQuery(transaction);
                var thetransaction = await _mediator.Send(gettranCommand);
                //update wallet


                userwallet.Amount = userwallet.Amount - Convert.ToDecimal(variationData.Amount);

                var loguserId = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
                userwallet.Log = "<li>Wallet Update from " + thetransaction.Description + " " + thetransaction.Id + " ::Amount: " + thetransaction.Amount + " ::Balance: " + userwallet.Amount + " :: Date: " + userwallet.LastUpdateAtUtc + ":: Loggedin User: " + loguserId+"</li>"+ userwallet.Log;
                //userwallet = null;
                UpdateWalletCommand updatewalletcommand = new UpdateWalletCommand(userwallet.UserId, "Validate Transaction", userwallet.Log, userwallet.Amount);
                await _mediator.Send(updatewalletcommand);
            }

            return result;
        }
    }

    
}
