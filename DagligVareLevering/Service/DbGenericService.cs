using DagligVareLevering.EFDbContext;
using DagligVareLevering.Models;
using Microsoft.EntityFrameworkCore;

namespace DagligVareLevering.Service
{
    public class DbGenericService<T> : IService<T> where T : class
    {
        private readonly AppDbContext _context;

        public DbGenericService(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddObjectAsync(T obj)
        {
            await _context.Set<T>().AddAsync(obj);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteObjectAsync(T obj)
        {
            _context.Set<T>().Remove(obj);
            await _context.SaveChangesAsync();
        }

        public async Task<T> GetObjectByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetObjectsAsync()
        {
            return await _context.Set<T>().AsNoTracking().ToListAsync();
        }

        public async Task UpdateObjectAsync(T obj)
        {
            _context.Set<T>().Update(obj);
            await _context.SaveChangesAsync();
        }

        public IQueryable<T> GetAllObjectInfoAsync()
        {
            return _context.Set<T>().AsNoTracking();
        }
    }
}
