using MediatR;
using RestaurantDirectory.Command.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RestaurantDirectory.Command.Commands.Cuisine
{
    public class AddCuisine
    {
        public class Command : IRequest<Guid>
        {
            public string Name { get; set; }
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
                var cuisine = new CuisineModel
                {
                    Id = Guid.NewGuid(),
                    Name = request.Name
                };

                _context.Cuisines.Add(cuisine);
                _context.SaveChanges();

                return cuisine.Id;
            }
        }
    }
}
