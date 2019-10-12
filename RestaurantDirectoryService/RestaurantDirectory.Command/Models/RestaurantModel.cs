using RestaurantDirectory.Command.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantDirectory.Command.Models
{
    [Table("Restaurant")]
    public class RestaurantModel
    {
        public int Id { get; set; }
        [ForeignKey("CityId")]
        public CityModel City { get; set; }
        public int? CityId { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public ParkingLot? ParkingLot { get; set; }
        public bool Tried { get; set; }
        public string Yelp { get; set; }
    }
}
