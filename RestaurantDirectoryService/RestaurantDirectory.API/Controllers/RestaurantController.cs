using MediatR;
using Microsoft.AspNetCore.Mvc;
using RestaurantDirectory.Command.Commands.Restaurant;
using System.Threading.Tasks;

namespace RestaurantDirectory.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RestaurantController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<int> CreateRestaurant(AddRestaurant.Command command)
        {
            return await _mediator.Send(command);
        }
    }
}
