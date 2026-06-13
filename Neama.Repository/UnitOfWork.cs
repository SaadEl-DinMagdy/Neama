using Neama.Core;
using Neama.Core.Entities;
using Neama.Core.Repositories.Contract;
using Neama.Repository.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Neama.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreContext _dbContext;


        private Hashtable _repositories;

        public UnitOfWork(StoreContext dbContext)
        {
            _dbContext = dbContext;
            _repositories = new Hashtable();
        }

        public IGenaricRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            var key = typeof(TEntity).Name;

            if (!_repositories.ContainsKey(key))
            {

                var repo = new GenaricRepository<TEntity>(_dbContext);


                _repositories.Add(key, repo);
            }


            return _repositories[key] as IGenaricRepository<TEntity>;
        }

        public async Task<int> CompleteAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public async ValueTask DisposeAsync()
        {
            await _dbContext.DisposeAsync();
        }
    }
}