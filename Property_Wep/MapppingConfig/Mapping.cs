using AutoMapper;
using Property_Wep.Models;
using Property_Wep.Models.Dto;

namespace Property_Wep.MapppingConfig
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            //Mapping the Villa
           
            CreateMap<VillaDTO, VillaCreateDTO>().ReverseMap();
            CreateMap<VillaDTO, VillaUpdateDTO>().ReverseMap();

            //Mapping the VillaNumber
            CreateMap<VillaNumberDTO, VillaNumberUpdateDTO>().ReverseMap();
            CreateMap<VillaNumberDTO , VillaNumberCreateDTO>().ReverseMap();
         
        }

    }
}
