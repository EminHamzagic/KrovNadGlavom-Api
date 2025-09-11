using System.Linq.Expressions;
using krov_nad_glavom_api.Data.DTO.Apartment;
using krov_nad_glavom_api.Domain.Entities;
using Serilog;

namespace krov_nad_glavom_api.Application.Utils
{
    public static class IQueriableExtensions
    {
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName)
        {
            return source.OrderBy(ToLambda<T>(propertyName));
        }

        public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string propertyName)
        {
            return source.OrderByDescending(ToLambda<T>(propertyName));
        }

        private static Expression<Func<T, object>> ToLambda<T>(string propertyName)
        {
            var parameter = Expression.Parameter(typeof(T));
            var property = Expression.Property(parameter, propertyName);
            var propAsObject = Expression.Convert(property, typeof(object));

            return Expression.Lambda<Func<T, object>>(propAsObject, parameter);
        }

        public static IQueryable<T> Filter<T>(this IQueryable<T> source, QueryStringParameters parameters)
        {
            switch (source)
            {
                case IQueryable<ApartmentToReturnDto> apartments:
                    {
                        if (!string.IsNullOrEmpty(parameters.City))
                            apartments = apartments.Where(a => a.Building.City.ToLower().Contains(parameters.City.ToLower()));
                        if (!string.IsNullOrEmpty(parameters.Address))
                            apartments = apartments.Where(a => a.Building.Address.ToLower().Contains(parameters.Address.ToLower()));
                        if (!string.IsNullOrEmpty(parameters.Orientation))
                            apartments = apartments.Where(a => a.Orientation == parameters.Orientation);
                        if (parameters.Area != null)
                            apartments = apartments.Where(a => a.Area == parameters.Area);
                        if (parameters.RoomCount != null)
                            apartments = apartments.Where(a => a.RoomCount == parameters.RoomCount);
                        if (parameters.BalconyCount != null)
                            apartments = apartments.Where(a => a.BalconyCount == parameters.BalconyCount);
                        if (parameters.Floor != null)
                            apartments = apartments.Where(a => a.Floor == parameters.Floor);

                        return (IQueryable<T>)apartments;
                    }
            }
            return source;
        }

        public static IQueryable<T> Sort<T>(this IQueryable<T> source, QueryStringParameters parameters)
        {
            switch (source)
            {
                case IQueryable<ApartmentToReturnDto> apartments:
                    {
                        try
                        {
                            if (!string.IsNullOrEmpty(parameters.SortProperty))
                            {
                                if (parameters.SortType == "asc")
                                {
                                    Log.Information($"Ordered in ascending order by {parameters.SortProperty}");
                                    return (IQueryable<T>)apartments.OrderBy(parameters.SortProperty);
                                }
                                else
                                {
                                    Log.Information($"Ordered in descending order by {parameters.SortProperty}");
                                    return (IQueryable<T>)apartments.OrderByDescending(parameters.SortProperty);
                                }
                            }
                        }
                        catch
                        {
                            Log.Information("You tried to order by property that does not exist");
                        }
                        return (IQueryable<T>)apartments.OrderByDescending(a => a.Id);
                    }
            }
            return source;
        }
    }
}