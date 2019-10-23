using Dapper;
using MediatR;
using RestaurantDirectory.Query.Dtos;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace RestaurantDirectory.Query.Queries.City
{
    public class GetCity
    {
        public class Query : IRequest<CityDto>
        {
            public int Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, CityDto>
        {
            private readonly IDbConnection _connection;

            public Handler(IDbConnection connection)
            {
                _connection = connection;
            }

            public async Task<CityDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = @"  SELECT  Id,
                                        Name
                                FROM    City
                                WHERE   Id = @Id";

                return await _connection.QueryFirstAsync<CityDto>(query, new { request.Id });
            }
        }
    }
}
