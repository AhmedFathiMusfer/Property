using System.ComponentModel.DataAnnotations;

namespace Property_WepAPI.Models
{
    public class Villa
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Details{ get; set; }
        public int Rate { get; set; }
        public int Occupancy { get; set; }
        public int Sqft { get; set; }
        public string? ImageUrl { get; set; }
        public string? LocalImagePath { get; set; }
        public string? Amenity { get; set; }
        public ICollection<VillaNumber> villaNumbers { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UbdatedDate { get; set; }
    }
   
}
