using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PrimusMessageBox.AppCore.Common.Interface;
using PrimusMessageBox.Domain.DTOs;
using StackExchange.Redis;

namespace PrimusMessageBox.AppCore.Common.Services
{
    public class CacheService : ICacheService
    {
        private readonly IDatabase _db;
        private readonly AppSettings _appSettings;
        public CacheService(IConnectionMultiplexer connectionMultiplxer, IOptions<AppSettings> appSettings)
        {
            _db = connectionMultiplxer.GetDatabase();
            _appSettings = appSettings.Value;
        }
        public async Task<T> GetData<T>(string key)
        {
            RedisValue result = new();
            try
            {
                result = await _db.StringGetAsync($"{_appSettings.AppKey}:{key}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return result.HasValue ? JsonConvert.DeserializeObject<T>(result) : default;
        }

        public async Task<bool> RemoveData(string key)
        {
            bool _isKeyExist = _db.KeyExists($"{_appSettings.AppKey}:{key}");
            if (_isKeyExist == true)
            {
                return await _db.KeyDeleteAsync($"{_appSettings.AppKey}:{key}");
            }
            return false;
        }

        public async Task<bool> SetData<T>(string key, T value, int ttl)
        {
            TimeSpan expiryTime = TimeSpan.FromSeconds(ttl); // ttl.DateTime.Subtract(DateTime.Now);
            bool isSet = false;
            try
            {
                isSet = await _db.StringSetAsync($"{_appSettings.AppKey}:{key}", JsonConvert.SerializeObject(value), expiryTime);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return isSet;
        }
    }
}
