using Dapper;
using MediatR;
using RestaurantDirectory.Query.Dtos;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace RestaurantDirectory.Query.Queries.Cuisine
{
    public class GetCuisine
    {
        public class Query : IRequest<CuisineDto>
        {
            public int Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, CuisineDto>
        {
            private readonly IDbConnection _connection;

            public Handler(IDbConnection connection)
            {
                _connection = connection;
            }

            public async Task<CuisineDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = @"  SELECT  Id,
                                        Name
                                FROM    Cuisine
                                WHERE   Id = @Id";

                return await _connection.QueryFirstAsync<CuisineDto>(query, new { request.Id });
            }
        }
    }
}
