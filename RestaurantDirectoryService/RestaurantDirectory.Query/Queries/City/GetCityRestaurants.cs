using Dapper;
using MediatR;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace RestaurantDirectory.Query.Queries.City
{
    public class GetCityRestaurants
    {
        public class Query : IRequest<IEnumerable<string>>
        {
            public int Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, IEnumerable<string>>
        {
            private readonly IDbConnection _connection;

            public Handler(IDbConnection connection)
            {
                _connection = connection;
            }

            public async Task<IEnumerable<string>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = @"  SELECT      r.Name
                                FROM        City c INNER JOIN
                                            Restaurant r ON c.Id = r.CityId
                                WHERE       c.Id = @Id
                                LIMIT       5;";

                return await _connection.QueryAsync<string>(query, new { request.Id });
            }
        }
    }
}
