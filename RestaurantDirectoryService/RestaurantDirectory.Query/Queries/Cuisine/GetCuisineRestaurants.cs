using Dapper;
using MediatR;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace RestaurantDirectory.Query.Queries.Cuisine
{
    public class GetCuisineRestaurants
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
                                FROM        Restaurant_Cuisine rc INNER JOIN
                                            Restaurant r ON r.Id = rc.RestaurantId
                                WHERE       rc.CuisineId = @Id
                                LIMIT       5;";

                return await _connection.QueryAsync<string>(query, new { request.Id });
            }
        }
    }
}
