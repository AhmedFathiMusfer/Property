using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Property_WepAPI.Models
{
    public class VillaNumber
    {
        [Key ,DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int VillaNo { get; set; }
        public string SpecialDetails { get; set; }
        [ForeignKey("villa")]
        public int VillaId { get; set; }
        public Villa villa { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdateDate { get; set; }




    }
}
