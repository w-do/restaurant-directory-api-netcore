using Dapper;
using MediatR;
using RestaurantDirectory.Query.Dtos;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace RestaurantDirectory.Query.Queries.City
{
    public class GetCities
    {
        public class Query : IRequest<IEnumerable<CityDto>> { }

        public class Handler : IRequestHandler<Query, IEnumerable<CityDto>>
        {
            private readonly IDbConnection _connection;

            public Handler(IDbConnection connection)
            {
                _connection = connection;
            }

            public async Task<IEnumerable<CityDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = @"  SELECT      id,
                                            name
                                FROM        city
                                ORDER BY    name;";

                return await _connection.QueryAsync<CityDto>(query);
            }
        }
    }
}
