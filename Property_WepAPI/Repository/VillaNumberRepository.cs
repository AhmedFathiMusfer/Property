using Microsoft.EntityFrameworkCore;
using Property_WepAPI.Data;
using Property_WepAPI.Models;
using Property_WepAPI.Repositry.IRpositry;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace Property_WepAPI.Repository
{
    public class VillaNumberRepository :Repository<VillaNumber>, IVillaNumberRepository
    {
        private readonly ApplicationDbContext _db;
        public VillaNumberRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }
       
        public async Task UpdateAsync(VillaNumber villaNumber)
        {
             _db.villaNumbers.Update(villaNumber);
            await SaveAsync();

        }
    
       
    }
}
