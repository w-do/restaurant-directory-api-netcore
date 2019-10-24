using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RestaurantDirectory.Command.Commands.Cuisine
{
    public class DeleteCuisine
    {
        public class Command : IRequest<Unit>
        {
            public int Id { get; set; }
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
                var cuisine = _context.Cuisines.Find(request.Id);

                if (cuisine != null)
                {
                    var restaurantCuisines = _context.RestaurantCuisines
                        .Where(x => x.CuisineId == request.Id)
                        .ToListAsync();

                    _context.Cuisines.Remove(cuisine);
                    _context.RestaurantCuisines.RemoveRange(await restaurantCuisines);

                    _context.SaveChanges();
                }

                return Unit.Value;
            }
        }
    }
}
