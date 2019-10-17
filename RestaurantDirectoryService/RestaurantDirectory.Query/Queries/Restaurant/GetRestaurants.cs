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
        public class Query : IRequest<IEnumerable<RestaurantListDto>>
        {
            // introduce filters later
        }

        public class Handler : IRequestHandler<Query, IEnumerable<RestaurantListDto>>
        {
            private readonly IDbConnection _connection;

            public Handler(IDbConnection connection)
            {
                _connection = connection;
            }

            public async Task<IEnumerable<RestaurantListDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = @"  SELECT		r.Id,
			                                ci.Name City,
                                            --group_concat(cu.Name SEPARATOR ', ') Cuisines,
                                            STRING_AGG(cu.Name, ', ') Cuisines,
			                                r.Name,
                                            r.Notes,
                                            r.ParkingLot,
                                            r.Tried,
                                            r.Yelp
                                FROM		Restaurant r LEFT OUTER JOIN
			                                Restaurant_Cuisine rc ON r.Id = rc.RestaurantId LEFT OUTER JOIN
			                                Cuisine cu ON cu.Id = rc.CuisineId LEFT OUTER JOIN
			                                City ci ON ci.Id = r.CityId
                                --GROUP BY	r.Id;
                                GROUP BY    r.Id, ci.Name, r.Name, r.Notes, r.ParkingLot, r.Tried, r.Yelp;";

                return await _connection.QueryAsync<RestaurantListDto>(query);
            }
        }
    }
}
