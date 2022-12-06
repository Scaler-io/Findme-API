using API.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.DataAccess.Specifications
{
    public class SpecificationEvaluator<TEntity> where TEntity : class
    {
        public static IQueryable<TEntity> GetQuery(
            IQueryable<TEntity> inputQuery,
            ISpecification<TEntity> spec
        )
        {
            var query = inputQuery;
            if(spec.Criteria != null) query = query.Where(spec.Criteria);
            if(spec.OrderBy != null) query = query.OrderBy(spec.OrderBy);
            if(spec.OrderByDescending != null) query = query.OrderByDescending(spec.OrderByDescending);
            if (spec.isPageingEnabled) query = query.Skip(spec.Skip).Take(spec.Take);
            if (spec.IncludeStrings != null) query = spec.IncludeStrings.Aggregate(query, (current, include) => current.Include(include));
            return query;
        }
    }
}
