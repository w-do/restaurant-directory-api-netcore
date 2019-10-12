using Dapper;
using MediatR;
using RestaurantDirectory.Query.Dtos;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace RestaurantDirectory.Query.Queries.Cuisine
{
    public class GetCuisines
    {
        public class Query : IRequest<IEnumerable<CuisineDto>> { }

        public class Handler : IRequestHandler<Query, IEnumerable<CuisineDto>>
        {
            private readonly IDbConnection _connection;

            public Handler(IDbConnection connection)
            {
                _connection = connection;
            }

            public async Task<IEnumerable<CuisineDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = @"  SELECT  Id,
                                        Name
                                FROM    City;";

                return await _connection.QueryAsync<CuisineDto>(query);
            }
        }
    }
}
