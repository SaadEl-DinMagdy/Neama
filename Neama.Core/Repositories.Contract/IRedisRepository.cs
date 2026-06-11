using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neama.Core.Repositories.Contract
{
    public interface IRedisRepository<T>
    {
        Task<bool> DeleteAsync(string key);
        Task<T?> GetAsync(string key);
        Task<T?> SetAsync(string  key , T valu , TimeSpan time);
    }
}
