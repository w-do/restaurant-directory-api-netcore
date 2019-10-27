﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantDirectory.Command.Models
{
    [Table("city")]
    public class CityModel
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        [MaxLength(128)]
        [Column("name")]
        public string Name { get; set; }
    }
}
