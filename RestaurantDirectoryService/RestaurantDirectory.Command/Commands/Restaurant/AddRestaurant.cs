using MediatR;
using RestaurantDirectory.Command.Enums;
using RestaurantDirectory.Command.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RestaurantDirectory.Command.Commands.Restaurant
{
    public class AddRestaurant
    {
        public class Command : IRequest<Guid>
        {
            public Guid? CityId { get; set; }
            public IEnumerable<Guid> CuisineIds { get; set; }
            public string Name { get; set; }
            public string Notes { get; set; }
            public ParkingLot? ParkingLot { get; set; }
            public bool Tried { get; set; }
            public string Yelp { get; set; }
        }

        public class Handler : IRequestHandler<Command, Guid>
        {
            private readonly RestaurantDbContext _context;

            public Handler(RestaurantDbContext context)
            {
                _context = context;
            }

            public async Task<Guid> Handle(Command request, CancellationToken cancellationToken)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    var restaurant = new RestaurantModel
                    {
                        Id = Guid.NewGuid(),
                        CityId = request.CityId,
                        Name = request.Name,
                        Notes = request.Notes,
                        ParkingLot = request.ParkingLot,
                        Tried = request.Tried,
                        Yelp = request.Yelp
                    };

                    _context.Restaurants.Add(restaurant);
                    _context.SaveChanges();

                    if (request.CuisineIds?.Any() ?? false)
                    {
                        var restaurantCuisines = request.CuisineIds.Select(x => new RestaurantCuisineModel
                        {
                            CuisineId = x,
                            RestaurantId = restaurant.Id
                        });

                        _context.RestaurantCuisines.AddRange(restaurantCuisines);
                        _context.SaveChanges();
                    }

                    transaction.Commit();

                    return restaurant.Id;
                }
            }
        }
    }
}
