using MediatR;
using Microsoft.AspNetCore.Mvc;
using RestaurantDirectory.Command.Commands.City;
using RestaurantDirectory.Query.Dtos;
using RestaurantDirectory.Query.Queries.City;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantDirectory.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CityController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<Guid> CreateCity(AddCity.Command command)
        {
            return await _mediator.Send(command);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCity(Guid id)
        {
            await _mediator.Send(new DeleteCity.Command { Id = id });
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetCities()
        {
            try
            {
                return Ok(await _mediator.Send(new GetCities.Query()));
            }
            catch (Exception e)
            {
                return Ok(e);
            }
        }

        [HttpGet("{id}")]
        public async Task<CityDto> GetCity(Guid id)
        {
            return await _mediator.Send(new GetCity.Query { Id = id });
        }

        [HttpGet("{id}/restaurants")]
        public async Task<IEnumerable<string>> GetCityRestaurants(Guid id)
        {
            return await _mediator.Send(new GetCityRestaurants.Query { Id = id });
        }

        [HttpPut("{id}")]
        public async Task UpdateCity(Guid id, UpdateCity.Command command)
        {
            command.Id = id;
            await _mediator.Send(command);
        }
    }
}
