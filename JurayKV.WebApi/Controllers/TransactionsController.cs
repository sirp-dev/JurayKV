using JurayKV.Application.Interswitch;
using JurayKV.Application.Queries.KvPointQueries;
using JurayKV.Application.Queries.OtherQueries;
using JurayKV.Application.Queries.SettingQueries;
using JurayKV.Application.Queries.TransactionQueries;
using JurayKV.Application.Queries.UserManagerQueries;
using JurayKV.Application.Queries.WalletQueries;
using JurayKV.Application.VtuServices;
using JurayKV.Domain.Aggregates.VariationAggregate;
using JurayKV.WebApi.Dto;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace JurayKV.WebApi.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {

        private readonly IMediator _mediator;

        public TransactionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("userpointshistory/{userId}")]//user point history
        public async Task<IActionResult> GetPointsAsync(Guid userId)
        {
            try
            {
                GetKvPointListByUserIdQuery command = new GetKvPointListByUserIdQuery(userId);
                var points = await _mediator.Send(command);

                return Ok(points); // Return points data
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, new { Error = "An error occurred while processing your request." });
            }
        }


        [HttpGet("billers")]//list all billers like airtime, data, dstv etc
        public async Task<IActionResult> GetBillersAsync()
        {
            try
            {
                GetSettingDefaultQuery settingCommand = new GetSettingDefaultQuery();
                var settingDetails = await _mediator.Send(settingCommand);

                if (settingDetails == null)
                {
                    return BadRequest("Unable to validate.");
                }

                if (settingDetails.BillGateway == Domain.Primitives.Enum.BillGateway.VTU)
                {
                    GetVariationCategoryCommand categoryCommand = new GetVariationCategoryCommand(Domain.Primitives.Enum.BillGateway.VTU);
                    var categoryVariations = await _mediator.Send(categoryCommand);
                    return Ok(new { CategoryVariations = categoryVariations });
                }
                else
                {
                    ListBillersCategoryQuery getCommand = new ListBillersCategoryQuery();
                    var billers = await _mediator.Send(getCommand);
                    return Ok(new { Billers = billers });
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, new { Error = "An error occurred while processing your request." });
            }
        }

        [HttpGet("getairtimebyid/{id}")]//get biller details to retrieve image, and id for request
        public async Task<IActionResult> GetAirtimeVariationAsync(Guid id)
        {
            try
            {
                GetCategoryVariationByIdQuery command = new GetCategoryVariationByIdQuery(id);
                var billerdetails = await _mediator.Send(command);
                if (billerdetails == null)
                {
                    return NotFound(); // Return 404 Not Found if the category variation is not found
                }

                return Ok(billerdetails); // Return category variation if found
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, new { Error = "An error occurred while processing your request." });
            }
        }

        [HttpPost("buyairtime/{userId}")]
        public async Task<IActionResult> PurchaseAirtimeAsync(Guid userId, [FromBody] AirtimeRequestDto request)
        {
            try
            {
                GetCategoryVariationByIdQuery command = new GetCategoryVariationByIdQuery(request.BillerId);
                var billerdetails = await _mediator.Send(command);
                var walletDetails = await _mediator.Send(new GetWalletUserByIdQuery(userId));

                decimal amount = 0;

                var setting = await _mediator.Send(new GetSettingDefaultQuery());
                if (setting.DisableAirtime == true)
                {
                    return BadRequest("Unable to process request. Try again.");
                }

                try
                {
                    amount = Convert.ToDecimal(request.Amount);
                    if (amount < 20)
                    {
                        return BadRequest("Minimum amount is 20.");
                    }
                }
                catch (Exception)
                {
                    return BadRequest("Invalid amount.");
                }

                if (walletDetails == null || walletDetails.Amount < amount)
                {
                    return BadRequest("Insufficient balance.");
                }

                var airtimeCommand = new AirtimeCommad(request.PhoneNumber, request.Network, request.Amount, userId.ToString());
                var result = await _mediator.Send(airtimeCommand);

                if (result.code == "success")
                {
                    return Ok("Successful.");
                }
                else if (result.code == "processing")
                {
                    return Ok("Processing.");
                }
                else if (result.code == "failure")
                {
                    if (result.message.Contains("DUPLICATE"))
                    {
                        return BadRequest("Duplicate order. Please wait for 3 minutes before placing another airtime order of the same amount to the same phone number.");
                    }
                    else
                    {
                        return BadRequest("Network unavailable or incorrect phone number. Please try again.");
                    }
                }
                else if (result.code == "limit")
                {
                    return BadRequest("Kindly upgrade to Tier 2 from your profile.");
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "Validation failed.");
            }
            return StatusCode(500, "Validation failed.");
        }

        [HttpGet("getbillerdropdown/{id}")]
        public async Task<IActionResult> GetVariationDetailsAsync(Guid id)
        {
            try
            {
                GetCategoryVariationByIdQuery variationcommand = new GetCategoryVariationByIdQuery(id);
                var categoryVariation = await _mediator.Send(variationcommand);
                //
                var variationsDropdown = await _mediator.Send(new GetVariationsByCategoryAndActiveQuery(id));

                // Map Variation data to SelectListItem
                var listVariations = variationsDropdown.Where(x => x.Active == true).Select(x =>
                    new SelectListItem
                    {
                        Value = x.Id.ToString(), // Assuming Id is an integer
                        Text = x.Name + " (₦" + x.Amount + ")",
                    }).ToList();

                return Ok(new { CategoryVariation = categoryVariation, ListVariations = listVariations });
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, new { Error = "An error occurred while processing your request." });
            }
        }


        [HttpPost("buydata/{userId}")]
        public async Task<IActionResult> PurchaseDataAsync(Guid userId, [FromBody] DataRequestDto request)
        {
            try
            {

                var walletDetails = await _mediator.Send(new GetWalletUserByIdQuery(userId));
                var UserData = await _mediator.Send(new GetUserManagerByIdQuery(userId));
                var variationData = await _mediator.Send(new GetVariationByIdQuery(Guid.Parse(request.VariationId)));

                var setting = await _mediator.Send(new GetSettingDefaultQuery());

                if (setting.DisableData == true)
                {
                    return BadRequest("Unable to process request. Try again.");
                }

                decimal amount = Convert.ToDecimal(variationData.Amount);
                if (UserData.Tier != variationData.Tier && UserData.Tier != Domain.Primitives.Enum.Tier.Tier2)
                {
                    if (UserData.Tier == Domain.Primitives.Enum.Tier.Tier1)
                    {
                        return Ok("Unable to process request. Upgrade to Tier 2.");
                    }
                }

                if (walletDetails == null || walletDetails.Amount < amount)
                {
                    return Ok("Insufficient balance.");
                }

                var dataCommand = new DataCommand(request.PhoneNumber, request.Network, request.VariationId, userId.ToString());
                var result = await _mediator.Send(dataCommand);

                if (result.code == "success")
                {
                    return Ok("Successful.");
                }
                else if (result.code == "processing")
                {
                    return Ok("Processing.");
                }
                else if (result.code == "failure")
                {
                    if (result.message.Contains("DUPLICATE"))
                    {
                        return Ok("Duplicate order. Please wait for 3 minutes before placing another data order of the same amount to the same phone number.");
                    }
                    else
                    {
                        return Ok("Network unavailable. Please try again.");
                    }
                }
                else if (result.code == "null")
                {
                    return Ok("Service unavailable. Please try again.");
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "Network unavailable or incorrect phone number. Please try again.");
            }
            return StatusCode(500, "Service unavailable. Please try again.");
        }


        [HttpGet("transactions/{userId}")]//transactions by user Id
        public async Task<IActionResult> GetTransactionsAsync(Guid userId)
        {
            try
            {
                var command = new GetTransactionListByUserQuery(userId);
                var transactionListDto = await _mediator.Send(command);
                return Ok(transactionListDto);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("transactions/{id}")]
        public async Task<IActionResult> GetTransactionByIdAsync(Guid id)
        {
            try
            {
                var command = new GetTransactionByIdQuery(id);
                var transactionDetailsDto = await _mediator.Send(command);
                return Ok(transactionDetailsDto);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("userreferrals/{phonenumber}")]//transactions by user Id
        public async Task<IActionResult> GetUserManagerByReferralListAsync(string phoneNumber)
        {
            try
            {
                var command = new GetUserManagerByReferralListQuery(phoneNumber);
                var userData = await _mediator.Send(command);
                return Ok(userData);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
