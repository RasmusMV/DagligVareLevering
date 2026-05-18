using DagligVareLevering.Repositories.Interfaces;
using DagligVareLevering.Service.Interfaces;

namespace DagligVareLevering.Service
{
    public class GenericService<T> : IService<T> where T : class
    {
        private readonly IRepository<T> _repository;

        public GenericService(IRepository<T> repository)
        {
            _repository = repository;
        }

        public Task AddObjectAsync(T obj)
        {
            return _repository.AddObjectAsync(obj);
        }

        public Task UpdateObjectAsync(T obj)
        {
            return _repository.UpdateObjectAsync(obj);
        }

        public Task DeleteObjectAsync(T obj)
        {
            return _repository.DeleteObjectAsync(obj);
        }

        public Task<T> GetObjectByIdAsync(int id)
        {
            return _repository.GetObjectByIdAsync(id);
        }

        public Task<IEnumerable<T>> GetObjectsAsync()
        {
            return _repository.GetObjectsAsync();
        }
    }
}
