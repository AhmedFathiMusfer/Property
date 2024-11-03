using System.ComponentModel.DataAnnotations;

namespace Property_WepAPI.Models.Dto
{
    public class VillaCreateDTO
    {
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }
        public string? Details { get; set; }
        [Required]
        public int Rate { get; set; }
        public int Occupancy { get; set; }
        public int Sqft { get; set; }
        public string? ImageUrl { get; set; }
        public string? Amenity { get; set; }

        

    }
}
