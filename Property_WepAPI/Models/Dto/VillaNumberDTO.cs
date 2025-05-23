﻿using System.ComponentModel.DataAnnotations;

namespace Property_WepAPI.Models.Dto
{
    public class VillaNumberDTO
    {
        [Required]
        public int VillaNo { get; set; }
        [Required]
        public int VillaId { get; set; }
        public string? SpecialDetails { get; set; }

        public VillaDTO villa { get; set; }



    }
}
