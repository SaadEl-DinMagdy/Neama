using Neama.Core.Repositories.Contract;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Neama.Repository
{
    public class RedisRepository<T> : IRedisRepository<T>
    {
        private readonly IDatabase _database;

        public RedisRepository(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase(); 
        }
        public async Task<bool> DeleteAsync(string key)
        {
            return await _database.KeyDeleteAsync(key);
        }

        public async Task<T?> GetAsync(string key)
        {
            var result = await _database.StringGetAsync(key);

            return result.IsNullOrEmpty ? default(T) : JsonSerializer.Deserialize<T>(result.ToString());
        }

        public async Task<T?> SetAsync(string key, T valu, TimeSpan time)
        {
            var createdOrUpdated = await _database.StringSetAsync(key, JsonSerializer.Serialize(valu), time);

            if (!createdOrUpdated)
                return default(T);
            return await GetAsync(key);
        }
    }
}
