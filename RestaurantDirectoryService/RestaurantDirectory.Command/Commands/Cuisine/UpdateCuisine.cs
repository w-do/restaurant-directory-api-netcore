using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RestaurantDirectory.Command.Commands.Cuisine
{
    public class UpdateCuisine
    {
        public class Command : IRequest<Unit>
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
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
                    cuisine.Name = request.Name;
                    _context.SaveChanges();
                }

                return Unit.Value;
            }
        }
    }
}
