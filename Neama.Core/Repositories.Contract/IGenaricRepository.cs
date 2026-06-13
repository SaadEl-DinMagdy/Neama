using Neama.Core.Entities;
using Neama.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neama.Core.Repositories.Contract
{
    public interface IGenaricRepository<T> where T : BaseEntity
    {
        Task<T?> GetAsync(int id);
        Task<IReadOnlyList<T>> GetAllAsync();

        Task<T?> GetWithSpecAsync(ISpecifications<T> spec);
        Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> spec);

        Task<int> GetCountAsync(ISpecifications<T> spec);

        Task AddAsync(T entity);
        void Delete(T entity);
        void Update(T entity);
    }
}
