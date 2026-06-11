using Neama.Core.Entities;
using Neama.Core.Repositories.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neama.Core
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IGenaricRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;

        Task<int> CompleteAsync();
    }
}
