using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantDirectory.Command.Models
{
    [Table("Cuisine")]
    public class CuisineModel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
