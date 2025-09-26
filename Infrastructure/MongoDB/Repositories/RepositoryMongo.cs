using System.Linq.Expressions;
using krov_nad_glavom_api.Application.Interfaces;
using MongoDB.Driver;

namespace krov_nad_glavom_api.Infrastructure.MongoDB.Repositories
{
    public class RepositoryMongo<T> : IRepository<T> where T : class
    {
        private readonly IMongoCollection<T> _collection;

        public RepositoryMongo(krovNadGlavomMongoDbContext context)
        {
            _collection = context.GetCollection<T>();
        }

         public async Task<T> GetByIdAsync(string id)
        {
            // Assumes your entities have a string Id field
            return await _collection.Find(Builders<T>.Filter.Eq("Id", id)).FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        // find methods
        public async Task<T> FindFirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await _collection.Find(predicate).FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<T>> FindAllAsync(Expression<Func<T, bool>> predicate)
        {
            return await _collection.Find(predicate).ToListAsync();
        }

        // add methods
        public async void AddAsync(T entity)
        {
            await _collection.InsertOneAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _collection.InsertManyAsync(entities);
        }

        // remove methods
        public void Remove(T entity)
        {
            var idProp = entity.GetType().GetProperty("Id")?.GetValue(entity)?.ToString();
            if (idProp != null)
            {
                _collection.DeleteOne(Builders<T>.Filter.Eq("Id", idProp));
            }
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            var ids = entities
                .Select(e => e.GetType().GetProperty("Id")?.GetValue(e)?.ToString())
                .Where(id => id != null)
                .ToList();

            _collection.DeleteMany(Builders<T>.Filter.In("Id", ids));
        }

        public void Update(T entity)
        {
            var idProp = entity.GetType().GetProperty("Id")?.GetValue(entity)?.ToString();
            if (idProp != null)
            {
                _collection.ReplaceOne(Builders<T>.Filter.Eq("Id", idProp), entity);
            }
        }

        public async Task<int> CountAsync()
        {
            return (int)await _collection.CountDocumentsAsync(_ => true);
        }
    }
}