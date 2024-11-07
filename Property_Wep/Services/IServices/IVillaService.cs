using Property_Wep.Models.Dto;

namespace Property_Wep.Services.IServices
{
    public interface IVillaService:IBaseService
    {
        Task<T> GetAllAsync<T>(string token);
        Task<T> CreateAsync<T>(VillaCreateDTO dto, string token);
        Task<T> UpdateAsync<T>(VillaUpdateDTO dto,string token);
        Task<T>RemoveAsync<T>(int id,string token);
        Task<T> GetAsync<T>(int id, string token);


    }
}
