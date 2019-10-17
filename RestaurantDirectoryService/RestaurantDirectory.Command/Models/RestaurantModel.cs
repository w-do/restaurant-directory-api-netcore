using RestaurantDirectory.Command.Commands.Restaurant;
using RestaurantDirectory.Command.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantDirectory.Command.Models
{
    [Table("Restaurant")]
    public class RestaurantModel
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("CityId")]
        public CityModel City { get; set; }
        public int? CityId { get; set; }
        [MaxLength(128)]
        public string Name { get; set; }
        [MaxLength(1024)]
        public string Notes { get; set; }
        public ParkingLot? ParkingLot { get; set; }
        public bool Tried { get; set; }
        [MaxLength(512)]
        public string Yelp { get; set; }

        public void Update(UpdateRestaurant.Command update)
        {
            CityId = update.CityId;
            Name = update.Name;
            Notes = update.Notes;
            ParkingLot = update.ParkingLot;
            Tried = update.Tried;
            Yelp = update.Yelp;
        }
    }
}
