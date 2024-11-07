using Property_Wep.Models.Dto;

namespace Property_Wep.Services.IServices
{
    public interface IVillaNumberService : IBaseService
    {
        Task<T> GetAllAsync<T>(string token);
        Task<T> CreateAsync<T>(VillaNumberCreateDTO dto,string token);
        Task<T> UpdateAsync<T>(VillaNumberUpdateDTO dto,string token);
        Task<T>RemoveAsync<T>(int id,string token);
        Task<T> GetAsync<T>(int id,string token);


    }
}
