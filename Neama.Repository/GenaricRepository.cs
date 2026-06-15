using Microsoft.EntityFrameworkCore;
using Neama.Core.Entities;
using Neama.Core.Repositories.Contract;
using Neama.Core.Specifications;
using Neama.Repository.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neama.Repository
{
    public class GenaricRepository<T> : IGenaricRepository<T> where T : BaseEntity
    {
        private readonly StoreContext _DbContext;

        public GenaricRepository(StoreContext dbContext)
        {
            _DbContext = dbContext;
        }
        public async Task<IReadOnlyList<T>> GetAllAsync()
        {

            return await _DbContext.Set<T>().ToListAsync();
        }


        public async Task<T?> GetAsync(int id)
        {
            return await _DbContext.Set<T>().FindAsync(id);
        }


        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> spec)
        {
            return await ApplySpecifications(spec).AsNoTracking().ToListAsync();
        }

        public async Task<T?> GetWithSpecAsync(ISpecifications<T> spec)
        {
            return await ApplySpecifications(spec).FirstOrDefaultAsync();
        }
        public async Task<int> GetCountAsync(ISpecifications<T> spec)
        {
            return await ApplySpecifications(spec).CountAsync();
        }

        private IQueryable<T> ApplySpecifications(ISpecifications<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_DbContext.Set<T>(), spec);
        }

        public async Task AddAsync(T entity)
        {
            await _DbContext.Set<T>().AddAsync(entity);
        }

        public void Delete(T entity)
        {
            _DbContext.Remove(entity);
        }

        public void Update(T entity)
        {
            _DbContext.Set<T>().Update(entity);
        }
    }
}
