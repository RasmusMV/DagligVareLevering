using DagligVareLevering.Models;

namespace DagligVareLevering.Service
{
    public interface IService<T>
    {
        Task<IEnumerable<T>> GetObjectsAsync();
        Task AddObjectAsync(T obj);
        Task DeleteObjectAsync(T obj);
        Task UpdateObjectAsync(T obj);
        Task<T> GetObjectByIdAsync(int id);
        IQueryable<T> GetAllObjectInfoAsync();
       
    }
}
