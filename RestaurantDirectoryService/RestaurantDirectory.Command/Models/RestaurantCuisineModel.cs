using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantDirectory.Command.Models
{
    [Table("Restaurant_Cuisine")]
    public class RestaurantCuisineModel
    {
        [ForeignKey("CuisineId")]
        public CuisineModel Cuisine { get; set; }
        public int CuisineId { get; set; }
        [ForeignKey("RestaurantId")]
        public RestaurantModel Restaurant { get; set; }
        public int RestaurantId { get; set; }
    }
}
