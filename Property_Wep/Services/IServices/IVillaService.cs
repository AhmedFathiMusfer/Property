using Property_Wep.Models.Dto;

namespace Property_Wep.Services.IServices
{
    public interface IVillaService
    {
        Task<T> GetAllAsync<T>();
        Task<T> CreateAsync<T>(VillaCreateDTO dto);
        Task<T> UpdateAsync<T>(VillaUpdateDTO dto);
        Task<T>RemoveAsync<T>(int id);
        Task<T> GetAsync<T>(int id);


    }
}
