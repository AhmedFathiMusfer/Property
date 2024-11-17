using AutoMapper;
using Property_WepAPI.Models;
using Property_WepAPI.Models.Dto;

namespace Property_WepAPI.MapppingConfig
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            //Mapping The Villa
            CreateMap<Villa, VillaDTO>().ReverseMap();
            CreateMap<Villa, VillaCreateDTO>().ReverseMap();
            CreateMap<Villa, VillaUpdateDTO>().ReverseMap();

            //Mapping The VillaNumber
            CreateMap<VillaNumber, VillaNumberUpdateDTO>().ReverseMap();
            CreateMap<VillaNumber, VillaNumberCreateDTO>().ReverseMap();
            CreateMap<VillaNumber, VillaNumberDTO>().ReverseMap();

            //Mapping The User
            CreateMap<UserDTO,ApplicationUser>().ReverseMap();
        }

    }
}
