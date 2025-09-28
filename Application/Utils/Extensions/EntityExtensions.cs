using System.Reflection;

namespace krov_nad_glavom_api.Application.Utils.Extensions
{
    public static class EntityExtensions
    {
        public static object GetId<T>(this T entity)
        {
            var prop = typeof(T).GetProperty("Id", BindingFlags.Public | BindingFlags.Instance);
            if (prop == null)
                throw new InvalidOperationException($"Type {typeof(T).Name} does not have an Id property.");

            return prop.GetValue(entity);
        }
    }
}