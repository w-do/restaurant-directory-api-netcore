using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantDirectory.Command.Models
{
    [Table("restaurant_x_cuisine")]
    public class RestaurantCuisineModel
    {
        [Column("cuisine_id")]
        public Guid CuisineId { get; set; }
        [Column("restaurant_id")]
        public Guid RestaurantId { get; set; }
    }
}
