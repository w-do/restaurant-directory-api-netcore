using MediatR;
using RestaurantDirectory.Command.Enums;
using RestaurantDirectory.Command.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RestaurantDirectory.Command.Commands.Restaurant
{
    public class AddRestaurant
    {
        public class Command : IRequest<int>
        {
            public int? CityId { get; set; }
            public IEnumerable<int> CuisineIds { get; set; }
            public string Name { get; set; }
            public string Notes { get; set; }
            public ParkingLot? ParkingLot { get; set; }
            public bool Tried { get; set; }
            public string Yelp { get; set; }
        }

        public class Handler: IRequestHandler<Command, int>
        {
            private readonly RestaurantDbContext _context;

            public Handler(RestaurantDbContext context)
            {
                _context = context;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    var restaurant = new RestaurantModel
                    {
                        CityId = request.CityId,
                        Name = request.Name,
                        Notes = request.Notes,
                        ParkingLot = request.ParkingLot,
                        Tried = request.Tried,
                        Yelp = request.Yelp
                    };

                    _context.Restaurants.Add(restaurant);
                    _context.SaveChanges();

                    if (request.CuisineIds.Any())
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
