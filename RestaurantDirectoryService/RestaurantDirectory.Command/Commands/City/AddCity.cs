using MediatR;
using RestaurantDirectory.Command.Models;
using System.Threading;
using System.Threading.Tasks;

namespace RestaurantDirectory.Command.Commands.City
{
    public class AddCity
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
                var city = new CityModel { Name = request.Name };

                _context.Cities.Add(city);
                _context.SaveChanges();

                return city.Id;
            }
        }
    }
}
