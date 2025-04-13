using Property_Wep.Models.Dto;

namespace Property_Wep.Services.IServices
{
    public interface IVillaNumberService 
    {
        Task<T> GetAllAsync<T>();
        Task<T> CreateAsync<T>(VillaNumberCreateDTO dto);
        Task<T> UpdateAsync<T>(VillaNumberUpdateDTO dto);
        Task<T>RemoveAsync<T>(int id);
        Task<T> GetAsync<T>(int id);


    }
}
