using Property_WepAPI.Models;
using Property_WepAPI.Repository.IRpository;
using System.Linq.Expressions;

namespace Property_WepAPI.Repositry.IRpositry
{
    public interface IVillaRepository:IRepository<Villa>
    {
   
    
        Task UpdateAsync(Villa villa);
       

    }
}
