using System.ComponentModel.DataAnnotations;

namespace Property_Wep.Models.Dto
{
    public class VillaUpdateDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }
        public string? Details { get; set; }
        [Required]
        public int Rate { get; set; }
        [Required]
        public int Occupancy { get; set; }
        [Required]
        public int Sqft { get; set; }
        public IFormFile? Image { get; set; }
        public string? ImageUrl { get; set; }
        public string ?LocalImagePath { get; set; } 
        public string? Amenity { get; set; }

       

    }
}
