using JurayKV.Application.Queries.SliderQueries;
using JurayKV.Domain.Aggregates.SliderAggregate;
using JurayKV.UI.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace JurayKV.UI.Pages
{
    public class IndexModel : BasePageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IMediator _mediator;

        public IndexModel(ILogger<IndexModel> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }
        public string RefX { get; set; }
         
            public async Task<IActionResult> OnGetAsync(string refx = null)
        {
            GetSliderListQuery command = new GetSliderListQuery();
            Slider = await _mediator.Send(command);
                RefX = refx;
                return Page();
        }
        public List<SliderDetailsDto> Slider = new List<SliderDetailsDto>();

        public async Task<IActionResult> OnPostAsync()
        {
            //CreateDepartmentCommand command = new CreateDepartmentCommand("sdhjys", "sdiusuhiu jkli kjjkjkj ukj iuiusd");
            _logger.LogInformation("Home get method Starting.");
            //Guid departmentId = await Mediator.Send(command);
            return Page();
         }
    }
}
 