using Neo4j.Driver;

namespace krov_nad_glavom_api.Application.Utils.Extensions
{
    public static class Neo4jMappingExtensions
    {
        public static T ToEntity<T>(this INode node) where T : new()
        {
            var obj = new T();
            var type = typeof(T);

            foreach (var prop in node.Properties)
            {
                var propertyInfo = type.GetProperty(prop.Key);
                if (propertyInfo == null) continue;

                object value = prop.Value;

                if (value is LocalDateTime ldt)
                    value = ldt.ToDateTime();
                else if (value is ZonedDateTime zdt)
                    value = zdt.ToDateTimeOffset().DateTime;
                else if (value is long l)
                {
                    if (propertyInfo.PropertyType == typeof(int))
                        value = (int)l;
                    else if (propertyInfo.PropertyType == typeof(decimal))
                        value = (decimal)l;
                    else if (propertyInfo.PropertyType == typeof(double))
                        value = (double)l;
                }
                else if (value is double d)
                {
                    if (propertyInfo.PropertyType == typeof(decimal))
                        value = (decimal)d;
                    else if (propertyInfo.PropertyType == typeof(int))
                        value = (int)d;
                }

                if (propertyInfo.PropertyType.IsAssignableFrom(value.GetType()))
                {
                    propertyInfo.SetValue(obj, value);
                }
                else if (propertyInfo.PropertyType == typeof(DateTime) && value is string s)
                {
                    propertyInfo.SetValue(obj, DateTime.Parse(s));
                }
            }

            return obj;
        }
    }
}