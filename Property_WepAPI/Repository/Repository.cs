using Microsoft.EntityFrameworkCore;
using Property_WepAPI.Data;
using Property_WepAPI.Models;
using Property_WepAPI.Repository.IRpository;
using System.Linq;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Property_WepAPI.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        private DbSet<T> Entity;

        public Repository(ApplicationDbContext db)
        {
            _db = db;
            Entity = _db.Set<T>();
        }

        public async Task CreateAsync(T villa)
        {
            await Entity.AddAsync(villa);
            await SaveAsync();
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null, int pageSize = 0, int pageNumber = 1)
        {
            IQueryable<T> Query = Entity;
            if (includeProperties != null)
            {
                foreach (var includeproperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                  Query=  Query.Include(includeproperty);
                }
            }
            if (filter != null)
            {
                Query = Query.Where(filter);
            }
            if (pageSize > 0)
            {
                if (pageSize > 100)
                {
                    pageSize = 100;
                 
                }
                Query = Query.Skip(pageSize * (pageNumber - 1)).Take(pageSize);
            }
            return await Query.ToListAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>>? filter = null, bool Tracked = true, string? includeProperties = null)
        {
            IQueryable<T> Query = Entity;
            if (includeProperties != null)
            {
                foreach (var includeproperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                   Query= Query.Include(includeproperty);
                }
            }
            if (Tracked != true)
            {
                Query = Query.AsNoTracking();
            }
            if (filter != null)
            {
                Query = Query.Where(filter);
            }

            return await Query.FirstOrDefaultAsync();
        }

        public async Task RemoveAsync(T villa)
        {
            Entity.Remove(villa);
            await SaveAsync();
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
