using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantDirectory.Query.Dtos
{
    public class RestaurantDto
    {
        public int Id { get; set; }
        public string CityName { get; set; }
        public string Cuisines { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public int? ParkingLot { get; set; }
        public bool Tried { get; set; }
        public string Yelp { get; set; }
    }
}
