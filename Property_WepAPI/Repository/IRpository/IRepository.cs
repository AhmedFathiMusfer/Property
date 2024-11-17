using Property_WepAPI.Models;
using System.Linq.Expressions;

namespace Property_WepAPI.Repository.IRpository
{
    public interface IRepository<T> where T:class
    {
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null,string? includeProperties=null,int pageSize=0,int pageNumber=1);
        Task<T> GetAsync(Expression<Func<T, bool>>? filter = null, bool Tracked = true, string? includeProperties = null);
        Task CreateAsync(T villa);
       // Task UpdateAsync(T villa);
        Task RemoveAsync(T villa);
        Task SaveAsync();
    }
}
