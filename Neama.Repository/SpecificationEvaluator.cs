using Microsoft.EntityFrameworkCore;
using Neama.Core.Entities;
using Neama.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neama.Repository
{
    public static class SpecificationEvaluator<T> where T : BaseEntity
    {
        public static IQueryable<T> GetQuery(IQueryable<T> InputQuery, ISpecifications<T> Spec)
        {
            var query = InputQuery;

            if (Spec.Criteria != null)
            {
                query = query.Where(Spec.Criteria);
            }

            if (Spec.OrderByAsc != null)
            {
                query = query.OrderBy(Spec.OrderByAsc);
            }
            else if (Spec.OrderByDesc != null)
            {
                query = query.OrderByDescending(Spec.OrderByDesc);
            }

            if (Spec.IsPaginationEnabled)
            {
                query = query.Skip(Spec.Skip).Take(Spec.Take);
            }

            query = Spec.Includes.Aggregate(query, (currentQuery, IncludeExperation) => currentQuery.Include(IncludeExperation));

            return query;
        }
    }
}
