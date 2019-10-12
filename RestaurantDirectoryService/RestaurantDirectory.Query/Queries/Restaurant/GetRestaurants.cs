using Dapper;
using MediatR;
using RestaurantDirectory.Query.Dtos;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace RestaurantDirectory.Query.Queries.Restaurant
{
    public class GetRestaurants
    {
        public class Query : IRequest<IEnumerable<RestaurantDto>>
        {
            // introduce filters later
        }

        public class Handler : IRequestHandler<Query, IEnumerable<RestaurantDto>>
        {
            private readonly IDbConnection _connection;

            public Handler(IDbConnection connection)
            {
                _connection = connection;
            }

            public async Task<IEnumerable<RestaurantDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = @"  SELECT		r.Id,
			                                ci.Name AS CityName,
                                            group_concat(cu.Name SEPARATOR ', ') Cuisines,
			                                r.Name,
                                            r.Notes,
                                            r.ParkingLot,
                                            r.Tried,
                                            r.Yelp
                                FROM		Restaurant r INNER JOIN
			                                Restaurant_Cuisine rc ON r.Id = rc.RestaurantId INNER JOIN
			                                Cuisine cu ON cu.Id = rc.CuisineId INNER JOIN
			                                City ci ON ci.Id = r.CityId
                                GROUP BY	r.Id";

                return await _connection.QueryAsync<RestaurantDto>(query);
            }
        }
    }
}
