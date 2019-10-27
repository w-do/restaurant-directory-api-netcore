using Dapper;
using MediatR;
using RestaurantDirectory.Query.Dtos;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace RestaurantDirectory.Query.Queries.Cuisine
{
    public class GetCuisine
    {
        public class Query : IRequest<CuisineDto>
        {
            public Guid Id { get; set; }
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
                var query = @"  SELECT  id,
                                        name
                                FROM    cuisine
                                WHERE   id = @Id;";

                return await _connection.QueryFirstAsync<CuisineDto>(query, new { request.Id });
            }
        }
    }
}
