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
                var query = @"  SELECT      r.Id,
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
                                WHERE       (@SearchTerm IS NULL OR
                                                LOWER(r.Name) LIKE CONCAT('%', @SearchTerm, '%') OR
                                                LOWER(r.Notes) LIKE CONCAT('%', @SearchTerm, '%')) AND
                                            (@Tried IS NULL OR r.Tried = @Tried)
                                GROUP BY    r.Id;";
                //var query = @"  SELECT      r.Id,
                //                            ci.Name City,
                //                            STRING_AGG(cu.Name, ', ') Cuisines,
                //                            r.Name,
                //                            r.Notes,
                //                            r.ParkingLot,
                //                            r.Tried,
                //                            r.Yelp
                //                FROM        Restaurant r LEFT OUTER JOIN
                //                            Restaurant_Cuisine rc ON r.Id = rc.RestaurantId LEFT OUTER JOIN
                //                            Cuisine cu ON cu.Id = rc.CuisineId LEFT OUTER JOIN
                //                            City ci ON ci.Id = r.CityId
                //                WHERE       --(city stuff) AND
                //                            --(cuisine stuff) AND
                //                            --(parking lot stuff) AND
                //                            (@SearchTerm IS NULL OR
                //                                LOWER(r.Name) LIKE '%' + @SearchTerm + '%' OR
                //                                LOWER(r.Notes) LIKE '%' + @SearchTerm + '%') AND
                //                            (@Tried IS NULL OR r.Tried = @Tried)
                //                GROUP BY    r.Id, ci.Name, r.Name, r.Notes, r.ParkingLot, r.Tried, r.Yelp;";

                request.SearchTerm = string.IsNullOrWhiteSpace(request.SearchTerm)
                    ? null
                    : request.SearchTerm.ToLower();

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
        }
    }
}
