using JurayKV.Application.Commands.IdentityKvAdCommands;
using JurayKV.Application.Queries.DashboardQueries;
using JurayKV.Application.Queries.IdentityKvAdQueries;
using JurayKV.Application.Queries.KvAdQueries;
using JurayKV.Application.Queries.KvPointQueries;
using JurayKV.Application.Queries.UserAccountQueries.DashboardQueries;
using JurayKV.Application.Queries.UserManagerQueries;
using JurayKV.Application.Queries.WalletQueries;
using JurayKV.Domain.ValueObjects;
using JurayKV.Infrastructure.Flutterwave.Dtos;
using JurayKV.WebApi.Dto;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace JurayKV.WebApi.Controllers
{


    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {

        private readonly IMediator _mediator;

        public DashboardController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("dashboard/{userId}")]//dashboard data
        public async Task<IActionResult> GetDashboardData(Guid userId)
        {
            GetUserDashboardQuery query = new GetUserDashboardQuery(userId);
             var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpGet("runningads/{userId}")]//running advert data
        public async Task<IActionResult> GetActiveAdsByUserId(Guid userId)
        {
            var query = new GetIdentityKvAdActiveByUserIdListQuery(userId);
            var result = await _mediator.Send(query);

            return Ok(result);
        }
        [HttpPost("uploadproof")]//upload proof and also check if advert time range has elapsed
        public async Task<IActionResult> UpdateAdAsync(Guid userAdvertId, IFormFile videofile)
        {

            try
            {
                GetIdentityKvAdByIdQuery command = new GetIdentityKvAdByIdQuery(userAdvertId);
              var  identityKvAdDetailsDto = await _mediator.Send(command);


                DateTime currentDate = DateTime.UtcNow.AddHours(1);
                DateTime nextDay6AM = currentDate.Date.AddDays(1).AddHours(6);

                bool isLinkEnabled = identityKvAdDetailsDto.CreatedAtUtc < nextDay6AM;
                if (!isLinkEnabled)
                {
                    return Ok($"Ad with ID {userAdvertId} Time has elapsed.");
                    
                }

            }catch(Exception c)
            {
                return StatusCode(500, new { Error = "An error occurred while processing your request." });

            }
            try
            {
                UpdateIdentityKvAdCommand command = new UpdateIdentityKvAdCommand(userAdvertId, videofile);
                await _mediator.Send(command);
                return Ok($"Ad with ID {userAdvertId} updated successfully."); // Return string value indicating success
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, new { Error = "An error occurred while processing your request." });
            }
        }
        [HttpGet("adspostedhistory/{userId}")]//posted advert history
        public async Task<IActionResult> GetAdsByUserId(Guid userId)
        {
            var query = new GetIdentityKvAdByUserIdListQuery(userId);
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpGet("bucketsanduserpostingstatus/{userId}")]//bucket for advert and check if the user has posted any of the advert
        public async Task<IActionResult> GetBucketsAndUserPostingStatusAsync(Guid userId)
        {
            try
            {
                 
                DateTime currentDate = DateForSix.GetTheDateBySix(DateTime.UtcNow.AddHours(1));
                GetKvAdActiveListAllBucketQuery command = new GetKvAdActiveListAllBucketQuery(currentDate);
                var kvAds = await _mediator.Send(command);

                GetIdentityKvAdActiveByUserIdListQuery kvcommand = new GetIdentityKvAdActiveByUserIdListQuery(userId);
                var ads = await _mediator.Send(kvcommand);

                var myActiveAds = ads.Select(a => a.KvAdId).ToList();

                kvAds.ForEach(ad =>
                {
                    ad.MyActiveAdvert = myActiveAds.Contains(ad.Id);
                });

                return Ok(new { KvAds = kvAds});
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, new { Error = "An error occurred while processing your request." });
            }
        }

        [HttpPost("postadvert/{userId}")]//register the advert before sending to share via whatsapp or social media
        public async Task<IActionResult> PostAsync(Guid userId, [FromBody] PostAdvertDto model)
        {
            try
            { 
                CreateIdentityKvAdCommand command = new CreateIdentityKvAdCommand(null, userId, model.AdvertId, model.AdvertDate);

                var outcome = await _mediator.Send(command);

                if (outcome != Guid.Empty)
                {
                   
                    GetKvAdByIdQuery getad = new GetKvAdByIdQuery(model.AdvertId);
                    var data = await _mediator.Send(getad);
                    return Ok(data.ImageUrl);
                }
                else
                {
                    return Ok("Unable to Register Ad or Ad Already Posted");
                    
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, new { Error = "An error occurred while processing your request." });
            }
        }

        [HttpGet("wallet/{userId}")]//wallet by user Id
        public async Task<IActionResult> GetWalletDetailsAsync(Guid userId)
        {
            try
            { 
                GetWalletUserByIdQuery getWalletCommand = new GetWalletUserByIdQuery(userId);
                var walletDetails = await _mediator.Send(getWalletCommand);
                return Ok(walletDetails);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, new { Error = "An error occurred while processing your request." });
            }
        }

        [HttpGet("profile/{userId}")]//wallet by user Id
        public async Task<IActionResult> GeProfileDetailsAsync(Guid userId)
        {
            try
            {
                GetUserManagerByIdQuery command = new GetUserManagerByIdQuery(userId);
                var details = await _mediator.Send(command);
                return Ok(details);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, new { Error = "An error occurred while processing your request." });
            }
        }



    }
}
