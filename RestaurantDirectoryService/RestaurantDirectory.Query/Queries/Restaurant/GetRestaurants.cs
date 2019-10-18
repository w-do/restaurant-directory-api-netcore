using Dapper;
using MediatR;
using RestaurantDirectory.Query.Dtos;
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
            public IEnumerable<int> CityIds { get; set; }
            public IEnumerable<int> CuisineIds { get; set; }
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
                    whereConditions.Add("(r.CityId IN @CityIds)");
                }
                if (query.CuisineIds != null && query.CuisineIds.Any())
                {
                    whereConditions.Add("(rc.CuisineId IN @CuisineIds)");
                }
                if (query.ParkingLot != null && query.ParkingLot.Any())
                {
                    whereConditions.Add("(r.ParkingLot IN @ParkingLot)");
                }
                if (!string.IsNullOrWhiteSpace(query.SearchTerm))
                {
                    whereConditions.Add("(r.Name LIKE '%' + @SearchTerm + '%' OR r.Notes LIKE '%' + @SearchTerm + '%')");
                }
                if (query.Tried != null)
                {
                    whereConditions.Add("(r.Tried = @Tried)");
                }

                var whereClause = whereConditions.Any()
                    ? $"WHERE {string.Join(" AND ", whereConditions)}"
                    : "";

                // currently if user filters by cuisine and a restaurant with multiple cuisines
                // is included, only the cuisines specified in the filter will be displayed.
                // look into this later
                return $@"  SELECT      r.Id,
                                        ci.Name City,
                                        group_concat(cu.Name SEPARATOR ', ') Cuisines,
                                        r.Name,
                                        r.Notes,
                                        r.ParkingLot,
                                        r.Tried,
                                        r.Yelp
                            FROM        Restaurant r LEFT OUTER JOIN
                                        Restaurant_Cuisine rc ON r.Id = rc.RestaurantId LEFT OUTER JOIN
                                        Cuisine cu ON cu.Id = rc.CuisineId LEFT OUTER JOIN
                                        City ci ON ci.Id = r.CityId
                            {whereClause}
                            GROUP BY    r.Id
                            LIMIT       100;";

                //return $@"  SELECT TOP(100)
                //                        r.Id,
                //                        ci.Name City,
                //                        STRING_AGG(cu.Name, ', ') Cuisines,
                //                        r.Name,
                //                        r.Notes,
                //                        r.ParkingLot,
                //                        r.Tried,
                //                        r.Yelp
                //            FROM        Restaurant r LEFT OUTER JOIN
                //                        Restaurant_Cuisine rc ON r.Id = rc.RestaurantId LEFT OUTER JOIN
                //                        Cuisine cu ON cu.Id = rc.CuisineId LEFT OUTER JOIN
                //                        City ci ON ci.Id = r.CityId
                //            {whereClause}
                //            GROUP BY    r.Id, ci.Name, r.Name, r.Notes, r.ParkingLot, r.Tried, r.Yelp;";
            }
        }
    }
}
