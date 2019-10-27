using Dapper;
using MediatR;
using RestaurantDirectory.Query.Dtos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RestaurantDirectory.Query.Queries.Restaurant
{
    public class GetRestaurants
    {
        public class Query : IRequest<IEnumerable<RestaurantListDto>>
        {
            public IEnumerable<Guid> CityIds { get; set; }
            public IEnumerable<Guid> CuisineIds { get; set; }
            public IEnumerable<int> ParkingLot { get; set; }
            public string SearchTerm { get; set; }
            public bool? Tried { get; set; }
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
                var query = CreateSql(request);

                var queryParams = new
                {
                    request.CityIds,
                    request.CuisineIds,
                    request.ParkingLot,
                    request.SearchTerm,
                    request.Tried
                };

                return await _connection.QueryAsync<RestaurantListDto>(query, queryParams);
            }

            private string CreateSql(Query query)
            {
                var whereConditions = new List<string>();

                if (query.CityIds != null && query.CityIds.Any())
                {
                    whereConditions.Add("(r.city_id IN @CityIds)");
                }
                if (query.CuisineIds != null && query.CuisineIds.Any())
                {
                    whereConditions.Add("(rc.cuisine_id IN @CuisineIds)");
                }
                if (query.ParkingLot != null && query.ParkingLot.Any())
                {
                    whereConditions.Add("(r.parking_lot IN @ParkingLot)");
                }
                if (!string.IsNullOrWhiteSpace(query.SearchTerm))
                {
                    whereConditions.Add("(r.name LIKE '%' + @SearchTerm + '%' OR r.notes LIKE '%' + @SearchTerm + '%')");
                }
                if (query.Tried != null)
                {
                    whereConditions.Add("(r.tried = @Tried)");
                }

                var whereClause = whereConditions.Any()
                    ? $"WHERE {string.Join(" AND ", whereConditions)}"
                    : "";

                // currently if user filters by cuisine and a restaurant with multiple cuisines
                // is included, only the cuisines specified in the filter will be displayed.
                // look into this later
                return $@"  SELECT      r.id,
                                        ci.name city,
                                        string_agg(cu.name, ', ') cuisines,
                                        r.name,
                                        r.notes,
                                        r.parking_lot parkinglot,
                                        r.tried,
                                        r.yelp
                            FROM        restaurant r LEFT OUTER JOIN
                                        restaurant_x_cuisine rc ON r.id = rc.restaurant_id LEFT OUTER JOIN
                                        cuisine cu ON cu.id = rc.cuisine_id LEFT OUTER JOIN
                                        city ci ON ci.id = r.city_id
                            {whereClause}
                            GROUP BY    r.id, ci.id
                            LIMIT       100;";
            }
        }
    }
}
