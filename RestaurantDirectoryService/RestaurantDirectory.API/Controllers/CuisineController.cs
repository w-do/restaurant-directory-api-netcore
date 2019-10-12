using MediatR;
using Microsoft.AspNetCore.Mvc;
using RestaurantDirectory.Command.Commands.Cuisine;
using System.Threading.Tasks;

namespace RestaurantDirectory.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CuisineController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CuisineController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<int> CreateCuisine(AddCuisine.Command command)
        {
            return await _mediator.Send(command);
        }
    }
}
