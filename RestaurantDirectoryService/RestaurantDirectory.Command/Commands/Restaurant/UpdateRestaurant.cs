using MediatR;
using Microsoft.EntityFrameworkCore;
using RestaurantDirectory.Command.Enums;
using RestaurantDirectory.Command.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RestaurantDirectory.Command.Commands.Restaurant
{
    public class UpdateRestaurant
    {
        public class Command : IRequest<Unit>
        {
            public int Id { get; set; }
            public int? CityId { get; set; }
            public IEnumerable<int> CuisineIds { get; set; }
            public string Name { get; set; }
            public string Notes { get; set; }
            public ParkingLot? ParkingLot { get; set; }
            public bool Tried { get; set; }
            public string Yelp { get; set; }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly RestaurantDbContext _context;

            public Handler(RestaurantDbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var restaurant = _context.Restaurants.Find(request.Id);

                if (restaurant == null)
                {
                    // if I wanted to be truly RESTful I'd create a restaurant here but. eh.
                    return Unit.Value;
                }

                var cuisinesTask = _context.RestaurantCuisines
                    .Where(x => x.RestaurantId == request.Id)
                    .ToListAsync();

                restaurant.Update(request);

                var cuisines = await cuisinesTask;

                var newCuisineIds = request.CuisineIds.Where(x => !cuisines.Select(y => y.CuisineId).Contains(x));
                var cuisinesToDelete = cuisines.Where(x => !request.CuisineIds.Contains(x.CuisineId));

                if (newCuisineIds.Any())
                {
                    var newCuisines = newCuisineIds.Select(x => new RestaurantCuisineModel
                    {
                        CuisineId = x,
                        RestaurantId = request.Id
                    });

                    _context.RestaurantCuisines.AddRange(newCuisines);
                }

                if (cuisinesToDelete.Any())
                {
                    _context.RestaurantCuisines.RemoveRange(cuisinesToDelete);
                }

                _context.SaveChanges();

                return Unit.Value;
            }
        }
    }
}
