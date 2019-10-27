using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RestaurantDirectory.Command.Commands.Restaurant
{
    public class DeleteRestaurant
    {
        public class Command : IRequest<Unit>
        {
            public Guid Id { get; set; }
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

                if (restaurant != null)
                {
                    var restaurantCuisines = _context.RestaurantCuisines
                        .Where(x => x.RestaurantId == request.Id)
                        .ToListAsync();

                    _context.Restaurants.Remove(restaurant);

                    _context.SaveChanges();
                }

                return Unit.Value;
            }
        }
    }
}
