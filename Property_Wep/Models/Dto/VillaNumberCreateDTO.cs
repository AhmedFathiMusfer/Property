using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Property_Wep.Models.Dto
{
    public class VillaNumberCreateDTO
    {
        [Required]
        [DisplayName("Villa Number")]
        public int VillaNo { get; set; }
        [Required]
        [DisplayName("Villa Name")]
        public int VillaId { get; set; }
        [DisplayName("Special Details")]
        public string? SpecialDetails { get; set; }



    }
}
