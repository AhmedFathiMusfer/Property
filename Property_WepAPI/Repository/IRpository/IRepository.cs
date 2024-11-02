using Property_WepAPI.Models;
using System.Linq.Expressions;

namespace Property_WepAPI.Repository.IRpository
{
    public interface IRepository<T> where T:class
    {
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null);
        Task<T> GetAsync(Expression<Func<T, bool>>? filter = null, bool Tracked = true);
        Task CreateAsync(T villa);
       // Task UpdateAsync(T villa);
        Task RemoveAsync(T villa);
        Task SaveAsync();
    }
}
