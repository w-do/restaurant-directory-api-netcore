using System.Collections.Generic;

namespace RestaurantDirectory.Query.Dtos
{
    public class RestaurantDetailDto
    {
        public int Id { get; set; }
        public int? CityId { get; set; }
        public IEnumerable<int> CuisineIds { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public int? ParkingLot { get; set; }
        public bool Tried { get; set; }
        public string Yelp { get; set; }
    }
}
