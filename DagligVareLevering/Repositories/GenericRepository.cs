using DagligVareLevering.EFDbContext;
using DagligVareLevering.Models;
using DagligVareLevering.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DagligVareLevering.Repositories
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _context;

        public GenericRepository(AppDbContext context)
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

        public IQueryable<T> Query()
        {
            return _context.Set<T>().AsNoTracking();
        }
    }
}
