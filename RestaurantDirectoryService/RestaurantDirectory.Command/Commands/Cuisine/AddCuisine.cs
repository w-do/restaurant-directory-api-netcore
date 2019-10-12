using MediatR;
using RestaurantDirectory.Command.Models;
using System.Threading;
using System.Threading.Tasks;

namespace RestaurantDirectory.Command.Commands.Cuisine
{
    public class AddCuisine
    {
        public class Command : IRequest<int>
        {
            public string Name { get; set; }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly RestaurantDbContext _context;

            public Handler(RestaurantDbContext context)
            {
                _context = context;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var cuisine = new CuisineModel { Name = request.Name };

                _context.Cuisines.Add(cuisine);
                _context.SaveChanges();

                return cuisine.Id;
            }
        }
    }
}
