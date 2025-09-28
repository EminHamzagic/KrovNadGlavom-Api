using System.Linq.Expressions;

namespace krov_nad_glavom_api.Application.Interfaces
{
    public interface IRepository<T> where T : class
    {
        // get methods
        Task<T> GetByIdAsync(string id);
        Task<IReadOnlyList<T>> GetAllAsync();

        // find methods
        Task<T> FindFirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
        Task<IReadOnlyList<T>> FindAllAsync(Expression<Func<T, bool>> predicate);

        // add methods
        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);

        // update methods
        void Update(T entity);

        // remove methods
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
        public Task<int> CountAsync();
    }
}