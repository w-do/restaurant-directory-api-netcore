using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantDirectory.Command.Models
{
    [Table("City")]
    public class CityModel
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(128)]
        public string Name { get; set; }
    }
}
