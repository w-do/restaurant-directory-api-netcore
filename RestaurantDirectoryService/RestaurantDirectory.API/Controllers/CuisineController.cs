using MediatR;
using Microsoft.AspNetCore.Mvc;
using RestaurantDirectory.Command.Commands.Cuisine;
using RestaurantDirectory.Query.Dtos;
using RestaurantDirectory.Query.Queries.Cuisine;
using System.Collections.Generic;
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCuisine(int id)
        {
            await _mediator.Send(new DeleteCuisine.Command { Id = id });
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<CuisineDto> GetCuisine(int id)
        {
            return await _mediator.Send(new GetCuisine.Query { Id = id });
        }

        [HttpGet("{id}/restaurants")]
        public async Task<IEnumerable<string>> GetCuisineRestaurants(int id)
        {
            return await _mediator.Send(new GetCuisineRestaurants.Query { Id = id });
        }

        [HttpGet]
        public async Task<IEnumerable<CuisineDto>> GetCuisines()
        {
            return await _mediator.Send(new GetCuisines.Query());
        }

        [HttpPut("{id}")]
        public async Task UpdateCuisine(int id, UpdateCuisine.Command command)
        {
            command.Id = id;
            await _mediator.Send(command);
        }
    }
}
