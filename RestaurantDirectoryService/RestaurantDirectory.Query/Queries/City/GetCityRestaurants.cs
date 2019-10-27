using Dapper;
using MediatR;
using System;
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
            public Guid Id { get; set; }
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
                var query = @"  SELECT      r.name
                                FROM        city c INNER JOIN
                                            restaurant r ON c.id = r.city_id
                                WHERE       c.id = @Id
                                LIMIT       5;";

                return await _connection.QueryAsync<string>(query, new { request.Id });
            }
        }
    }
}
