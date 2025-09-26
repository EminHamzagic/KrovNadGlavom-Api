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
                case IQueryable<ApartmentWithBuildingDto> apartments:
                    {
                        if (!string.IsNullOrEmpty(parameters.City))
                            apartments = apartments.Where(x => x.Building.City.ToLower().Contains(parameters.City.ToLower()));
                        if (!string.IsNullOrEmpty(parameters.Address))
                            apartments = apartments.Where(x => x.Building.Address.ToLower().Contains(parameters.Address.ToLower()));
                        if (!string.IsNullOrEmpty(parameters.Orientation))
                            apartments = apartments.Where(x => x.Apartment.Orientation == parameters.Orientation);
                        if (parameters.Area != null)
                            apartments = apartments.Where(x => x.Apartment.Area == parameters.Area);
                        if (parameters.RoomCount != null)
                            apartments = apartments.Where(x => x.Apartment.RoomCount == parameters.RoomCount);
                        if (parameters.BalconyCount != null)
                            apartments = apartments.Where(x => x.Apartment.BalconyCount == parameters.BalconyCount);
                        if (parameters.Floor != null)
                            apartments = apartments.Where(x => x.Apartment.Floor == parameters.Floor);

                        return (IQueryable<T>)apartments;
                    }
                case IQueryable<Agency> agencies:
                    {
                        if (!string.IsNullOrEmpty(parameters.SearchText))
                            agencies = agencies.Where(x => x.Name.ToLower().Contains(parameters.SearchText.ToLower()));

                        return (IQueryable<T>)agencies;
                    }
                case IQueryable<Building> buildings:
                    {
                        if (!string.IsNullOrEmpty(parameters.SearchText))
                            buildings = buildings.Where(x => x.ParcelNumber.ToLower().Contains(parameters.SearchText.ToLower())
                            || x.City.ToLower().Contains(parameters.SearchText.ToLower())
                            || x.Address.ToLower().Contains(parameters.SearchText.ToLower()));

                        return (IQueryable<T>)buildings;
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
                                    return (IQueryable<T>)apartments.OrderBy(parameters.SortProperty);
                                }
                                else
                                {
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
                case IQueryable<Agency> agencies:
                    {
                        try
                        {
                            if (!string.IsNullOrEmpty(parameters.SortProperty))
                            {
                                if (parameters.SortType == "asc")
                                {
                                    return (IQueryable<T>)agencies.OrderBy(parameters.SortProperty);
                                }
                                else
                                {
                                    return (IQueryable<T>)agencies.OrderByDescending(parameters.SortProperty);
                                }
                            }
                        }
                        catch
                        {
                            Log.Information("You tried to order by property that does not exist");
                        }
                        return (IQueryable<T>)agencies.OrderByDescending(a => a.Id);
                    }
                case IQueryable<Building> buildings:
                    {
                        try
                        {
                            if (!string.IsNullOrEmpty(parameters.SortProperty))
                            {
                                if (parameters.SortType == "asc")
                                {
                                    return (IQueryable<T>)buildings.OrderBy(parameters.SortProperty);
                                }
                                else
                                {
                                    return (IQueryable<T>)buildings.OrderByDescending(parameters.SortProperty);
                                }
                            }
                        }
                        catch
                        {
                            Log.Information("You tried to order by property that does not exist");
                        }
                        return (IQueryable<T>)buildings.OrderByDescending(a => a.Id);
                    }
            }
            return source;
        }
    }
}