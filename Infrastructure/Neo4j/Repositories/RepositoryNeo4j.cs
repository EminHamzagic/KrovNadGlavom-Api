using System.Linq.Expressions;
using System.Text.Json;
using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Application.Utils.Extensions;
using Neo4j.Driver;

namespace krov_nad_glavom_api.Infrastructure.Neo4j.Repositories
{
    public class RepositoryNeo4j<T> : IRepository<T> where T : class, new()
    {
        private readonly krovNadGlavomNeo4jDbContext _context;
        private readonly string _label;

        public RepositoryNeo4j(krovNadGlavomNeo4jDbContext context)
        {
            _context = context;
            _label = typeof(T).Name;
        }
        
        public async Task<T> GetByIdAsync(string id)
        {
            var query = $"MATCH (n:{_label} {{ Id: $id }}) RETURN n LIMIT 1";
            await using var session = _context.Driver.AsyncSession();
            var cursor = await session.RunAsync(query, new { id });

            if (await cursor.FetchAsync())
            {
                return MapToEntity(cursor.Current["n"].As<INode>());
            }

            return null;
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            var query = $"MATCH (n:{_label}) RETURN n";
            await using var session = _context.Driver.AsyncSession();
            var cursor = await session.RunAsync(query);

            var list = new List<T>();
            await foreach (var record in cursor)
            {
                list.Add(MapToEntity(record["n"].As<INode>()));
            }
            return list;
        }

        public async Task<T> FindFirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            var all = await GetAllAsync();
            return all.AsQueryable().FirstOrDefault(predicate.Compile());
        }

        public async Task<IReadOnlyList<T>> FindAllAsync(Expression<Func<T, bool>> predicate)
        {
            var all = await GetAllAsync();
            return all.AsQueryable().Where(predicate.Compile()).ToList();
        }

        public async Task AddAsync(T entity)
        {
            var props = entity.ToDictionary();
            var query = $"CREATE (n:{_label} $props)";
            await using var session = _context.Driver.AsyncSession();
            await session.RunAsync(query, new { props });
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            foreach (var e in entities)
            {
                await AddAsync(e);
            }
        }

        public async void Remove(T entity)
        {
            var id = entity.GetId();
            var query = $"MATCH (n:{_label} {{ Id: $id }}) DETACH DELETE n";
            await using var session = _context.Driver.AsyncSession();
            await session.RunAsync(query, new { id });
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            foreach (var e in entities)
            {
                Remove(e);
            }
        }

        public async void Update(T entity)
        {
            var id = entity.GetId();
            var props = entity.ToDictionary();
            var query = $"MATCH (n:{_label} {{ Id: $id }}) SET n = $props";
            await using var session = _context.Driver.AsyncSession();
            await session.RunAsync(query, new { id, props });
        }

        public async Task<int> CountAsync()
        {
            var query = $"MATCH (n:{_label}) RETURN count(n) AS cnt";
            await using var session = _context.Driver.AsyncSession();
            var cursor = await session.RunAsync(query);
            var record = await cursor.SingleAsync();
            return record["cnt"].As<int>();
        }

        private static T MapToEntity(INode node)
        {
            return node.ToEntity<T>();
        }
    }
}