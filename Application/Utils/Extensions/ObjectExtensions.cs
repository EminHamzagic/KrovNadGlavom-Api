using System.Reflection;

namespace krov_nad_glavom_api.Application.Utils.Extensions
{
    public static class ObjectExtensions
    {
        public static Dictionary<string, object> ToDictionary(this object obj)
        {
            if (obj == null)
                return new Dictionary<string, object>();

            return obj.GetType()
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => p.CanRead && p.GetValue(obj) != null)
                    .ToDictionary(p => p.Name, p => p.GetValue(obj));
        }
    }
}