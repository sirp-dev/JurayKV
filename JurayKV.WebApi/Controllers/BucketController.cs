using JurayKV.Application.Commands.IdentityCommands.UserCommands;
using JurayKV.Application.Queries.BucketQueries;
using JurayKV.WebApi.Dto;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JurayKV.WebApi.Controllers
{
    //[Authorize]
    [ApiController] 
    [Route("api/[controller]")]

    public class BucketController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BucketController(IMediator mediator)
        {
            _mediator = mediator;
        }
     

        [HttpGet("getallbucket")]
        //[Authorize]
        public async Task<ActionResult<List<BucketListDto>>> GetAllBucketList()
        {
            var query = new GetAllBucketListQuery();
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBucketById(Guid id)
        {
            var query = new GetBucketByIdQuery(id);
            var result = await _mediator.Send(query);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet("dropdown")]
        public async Task<IActionResult> GetBucketDropdownList()
        {
            var query = new GetBucketDropdownListQuery();
            var result = await _mediator.Send(query);

            return Ok(result);
        }
        [HttpGet("list")]
        public async Task<IActionResult> GetBucketList()
        {
            var query = new GetBucketListQuery();
            var result = await _mediator.Send(query);

            return Ok(result);
        }

    }
}
