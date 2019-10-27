using Dapper;
using MediatR;
using RestaurantDirectory.Query.Dtos;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace RestaurantDirectory.Query.Queries.Restaurant
{
    public class GetRestaurant
    {
        public class Query : IRequest<RestaurantDetailDto>
        {
            public Guid Id { get; set; }
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
                var restaurantQuery = @"SELECT  id,
                                                city_id cityid,
                                                name,
                                                notes,
                                                parking_lot parkinglot,
                                                tried,
                                                yelp
                                        FROM    restaurant
                                        WHERE   id = @Id;";
                var cuisinesQuery = @"  SELECT  cuisine_id
                                        FROM    restaurant_x_cuisine
                                        WHERE   restaurant_id = @Id;";

                using (var multi = _connection.QueryMultiple($"{restaurantQuery} {cuisinesQuery}", new { request.Id }))
                {
                    var restaurant = multi.ReadFirst<RestaurantDetailDto>();
                    restaurant.CuisineIds = multi.Read<Guid>();
                    return restaurant;
                }
            }
        }
    }
}
