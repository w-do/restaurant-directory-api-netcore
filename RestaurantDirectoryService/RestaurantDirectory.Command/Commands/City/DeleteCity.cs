using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace RestaurantDirectory.Command.Commands.City
{
    public class DeleteCity
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
                var city = _context.Cities.Find(request.Id);

                if (city != null)
                {
                    _context.Cities.Remove(city);
                    _context.SaveChanges();
                }

                return Unit.Value;
            }
        }
    }
}
