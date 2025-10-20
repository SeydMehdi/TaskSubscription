using CleanArchitecture.Common.Infra.Utilities.Mapping;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;

namespace CleanArchitecture.Common.Infra.Common
{
    public static class InfraExtensions
    {

        public static void RegisterEntityMaps(this ModelBuilder modelBuilder, params Assembly[] assemblies)
        {
            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes();
                var runtimeType = typeof(IEntityConfigured);//.MakeGenericType(t);
                var eneitityMapTypes = types.Where(x => x.GetInterfaces().Contains(runtimeType) && !x.IsInterface).ToList();

                foreach (var type in eneitityMapTypes)
                {
                    if (type != null)
                    {
                        var enitity = Activator.CreateInstance(type) as IEntityConfigured;
                        enitity.Configure(modelBuilder);
                    }
                }

            }
        }


        /// <summary>
        /// if dto is allowed to paging (that checked by UsePaging==true)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        public static IQueryable<T> AddPaging<T>(this IQueryable<T> query, IQueryPaging dto)
        {
            if (dto.UsePaging)
                return query.Skip(dto.PageSkip).Take(dto.RowsPerPage);
            return query;
        }

        public static IQueryable<T> DateBetween<T>(this IQueryable<T> query,
            Expression<Func<T, DateTime>> dateSelector, IDateFilterDto dto)
        {
            if (dto.IsSearchByDate)
                return query.Where(m =>
                    dateSelector.Compile().Invoke(m) >= dto.FromDate &&
                    dateSelector.Compile().Invoke(m) <= dto.ToDate);
            return query;
        }


        public static IQueryable<T> QueryIf<T>(this IQueryable<T> query,
            Expression<Func<T, int>> dateSelector, object dto)
        {
            var valueProp = dto.GetType().GetProperty(nameof(dateSelector));
            var isSearch = true;
            var isSearchProp = dto.GetType().GetProperty($"Is{nameof(dateSelector)}");
            if (isSearchProp != null && isSearchProp.PropertyType == typeof(bool))
            {
                isSearch = (bool)isSearchProp.GetValue(dto);
            }
            var value = (int)valueProp.GetValue(dto);
            if (isSearch)
                return query.Where(m =>
                    dateSelector.Compile().Invoke(m) == value);
            return query;
        }

        /// <summary>
        /// Check if IsSearchBy{PropertyName} is exist and and check property is not null or whitespace then add condition
        /// to query otherwise return sourcequery
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="dateSelector"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        public static IQueryable<T> QueryIf<T>(this IQueryable<T> query,
           Expression<Func<T, string>> dateSelector, object dto)
        {
            var valueProp = dto.GetType().GetProperty(nameof(dateSelector));
            if (valueProp == null) return query;
            var isSearch = true;
            var isSearchProp = dto.GetType().GetProperty($"Is{nameof(dateSelector)}");
            if (isSearchProp != null && isSearchProp.PropertyType == typeof(bool))
            {
                isSearch = (bool)isSearchProp.GetValue(dto);
            }

            
            var value = valueProp.GetValue(dto).ToString();
            if (isSearch && !string.IsNullOrWhiteSpace(value))
                return query.Where(m =>
                    dateSelector.Compile().Invoke(m).Contains(value));
            return query;
        }
    }
}
