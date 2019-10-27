using System;
using System.Collections.Generic;

namespace RestaurantDirectory.Query.Dtos
{
    public class RestaurantDetailDto
    {
        public Guid Id { get; set; }
        public Guid? CityId { get; set; }
        public IEnumerable<Guid> CuisineIds { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public int? ParkingLot { get; set; }
        public bool Tried { get; set; }
        public string Yelp { get; set; }
    }
}
