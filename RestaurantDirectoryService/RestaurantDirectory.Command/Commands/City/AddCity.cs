using MediatR;
using RestaurantDirectory.Command.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RestaurantDirectory.Command.Commands.City
{
    public class AddCity
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
                var city = new CityModel
                {
                    Id = Guid.NewGuid(),
                    Name = request.Name
                };

                _context.Cities.Add(city);
                _context.SaveChanges();

                return city.Id;
            }
        }
    }
}
