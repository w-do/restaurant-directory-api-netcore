using Dapper;
using MediatR;
using RestaurantDirectory.Query.Dtos;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace RestaurantDirectory.Query.Queries.Restaurant
{
    public class GetRestaurant
    {
        public class Query : IRequest<RestaurantDetailDto>
        {
            public int Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, RestaurantDetailDto>
        {
            private readonly IDbConnection _connection;

            public Handler(IDbConnection connection)
            {
                _connection = connection;
            }

            public async Task<RestaurantDetailDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var restaurantQuery = @"SELECT  Id,
                                                CityId,
                                                Name,
                                                Notes,
                                                ParkingLot,
                                                Tried,
                                                Yelp
                                        FROM    Restaurant
                                        WHERE   Id = @Id;";
                var cuisinesQuery = @"  SELECT  CuisineId
                                        FROM    Restaurant_Cuisine
                                        WHERE   RestaurantId = @Id;";

                using (var multi = _connection.QueryMultiple($"{restaurantQuery} {cuisinesQuery}", new { request.Id }))
                {
                    var restaurant = multi.ReadFirst<RestaurantDetailDto>();
                    restaurant.CuisineIds = multi.Read<int>();
                    return restaurant;
                }
            }
        }
    }
}
