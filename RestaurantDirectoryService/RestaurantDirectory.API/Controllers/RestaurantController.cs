﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using RestaurantDirectory.Command.Commands.Restaurant;
using RestaurantDirectory.Query.Dtos;
using RestaurantDirectory.Query.Queries.Restaurant;
using System.Collections.Generic;
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

        [HttpGet("{id}")]
        public async Task<RestaurantDetailDto> GetRestaurant(int id)
        {
            return await _mediator.Send(new GetRestaurant.Query { Id = id });
        }

        [HttpGet]
        public async Task<IEnumerable<RestaurantListDto>> GetRestaurants()
        {
            return await _mediator.Send(new GetRestaurants.Query());
        }
    }
}
