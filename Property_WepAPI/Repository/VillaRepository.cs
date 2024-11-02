using Microsoft.EntityFrameworkCore;
using Property_WepAPI.Data;
using Property_WepAPI.Models;
using Property_WepAPI.Repositry.IRpositry;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace Property_WepAPI.Repository
{
    public class VillaRepository :Repository<Villa>, IVillaRepository
    {
        private readonly ApplicationDbContext _db;
        public VillaRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }
       
        public async Task UpdateAsync(Villa villa)
        {
             _db.villas.Update(villa);
            await SaveAsync();

        }
    
       
    }
}
