using Amazon.Runtime.Internal;
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
using TanvirArjel.ArgumentChecker;
using static JurayKV.Domain.Primitives.Enum;

namespace JurayKV.Application.VtuServices
{
    public sealed class DataCommand : IRequest<DataResponse>
    {
        public DataCommand(string phone, string network, string variationId, string userId)
        {
            PhoneNumber = phone;
            Network = network;
            VariationId = variationId;
            UserId = userId;
        }

        public string PhoneNumber { get; set; }
        public string Network { get; set; }
        public string VariationId { get; set; }
        public string UserId { get; set; }

    }

    internal class DataCommandHandler : IRequestHandler<DataCommand, DataResponse>
    {
        private readonly IVtuService _vtuService;
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IVariationRepository _variationRepository;


        public DataCommandHandler(

                IVtuService vtuService, IMediator mediator, IHttpContextAccessor httpContextAccessor, IVariationRepository variationRepository)
        {
            _vtuService = vtuService;
            _mediator = mediator;
            _httpContextAccessor = httpContextAccessor;
            _variationRepository = variationRepository;
        }

        public async Task<DataResponse> Handle(DataCommand request, CancellationToken cancellationToken)
        {
            _ = request.ThrowIfNull(nameof(request));
            //get variation amount
            var variationData = await _variationRepository.GetByIdAsync(Guid.Parse(request.VariationId));
            DataRequest data = new DataRequest();
            DataResponse result = new DataResponse();
            data.PhoneNumber = request.PhoneNumber;
            data.Network = request.Network;
            data.VariationId = variationData.Code;
            result = await _vtuService.DataRequest(data);
            //getwallet
            GetWalletUserByIdQuery getwalletcommand = new GetWalletUserByIdQuery(Guid.Parse(request.UserId));
            var userwallet = await _mediator.Send(getwalletcommand);
            if (userwallet == null)
            {
                //send log request
            }
            if (result != null)
            {
                if (result.code == "success")
                {
                    try
                    {


                        //create transaction
                        CreateTransactionCommand createtransaction = new CreateTransactionCommand(userwallet.Id, userwallet.UserId, "DATA", "", Convert.ToDecimal(variationData.Amount), TransactionTypeEnum.Debit, EntityStatus.Successful, result.data.order_id, "DATA PURCHASE", result.data.order_id);
                        var transaction = await _mediator.Send(createtransaction);

                        //get the transaction by id
                        GetTransactionByIdQuery gettranCommand = new GetTransactionByIdQuery(transaction);
                        var thetransaction = await _mediator.Send(gettranCommand);
                        //update wallet


                        userwallet.Amount = userwallet.Amount - Convert.ToDecimal(variationData.Amount);

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
                    CreateTransactionCommand createtransaction = new CreateTransactionCommand(userwallet.Id, userwallet.UserId, "DATA", "", Convert.ToDecimal(variationData.Amount), TransactionTypeEnum.Debit, EntityStatus.Pending, "XXXXXXX", "DATA PURCHASE", "XXXXXX");
                    var transaction = await _mediator.Send(createtransaction);

                    //get the transaction by id
                    GetTransactionByIdQuery gettranCommand = new GetTransactionByIdQuery(transaction);
                    var thetransaction = await _mediator.Send(gettranCommand);
                    //update wallet


                    userwallet.Amount = userwallet.Amount - Convert.ToDecimal(variationData.Amount);

                    var loguserId = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
                    userwallet.Log = "<li>Wallet Update from " + thetransaction.Description + " " + thetransaction.Id + " ::Amount: " + thetransaction.Amount + " ::Balance: " + userwallet.Amount + " :: Date: " + userwallet.LastUpdateAtUtc + ":: Loggedin User: " + loguserId + "</li>" + userwallet.Log;
                    //userwallet = null;
                    UpdateWalletCommand updatewalletcommand = new UpdateWalletCommand(userwallet.UserId, "Validate Transaction", userwallet.Log, userwallet.Amount);
                    await _mediator.Send(updatewalletcommand);
                }
                else
                {
                    result.code = "null";
                }
            }
            else
            {
                result.code = "null";
            }
            return result;
        }
    }
}
