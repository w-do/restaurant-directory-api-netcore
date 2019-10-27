using RestaurantDirectory.Command.Commands.Restaurant;
using RestaurantDirectory.Command.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantDirectory.Command.Models
{
    [Table("restaurant")]
    public class RestaurantModel
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        [Column("city_id")]
        public Guid? CityId { get; set; }
        [MaxLength(128)]
        [Column("name")]
        public string Name { get; set; }
        [MaxLength(1024)]
        [Column("notes")]
        public string Notes { get; set; }
        [Column("parking_lot")]
        public ParkingLot? ParkingLot { get; set; }
        [Column("tried")]
        public bool Tried { get; set; }
        [MaxLength(512)]
        [Column("yelp")]
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
