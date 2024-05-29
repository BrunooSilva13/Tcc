using System.Globalization;
using coin.src.Models;
using coin.src.Services.Refit;
using coin.src.Services.Interfaces;
using StackExchange.Redis;
using Newtonsoft.Json;

namespace coin.src.Services
{
    public class QuotationService : IQuotationService
    {
        private readonly IEconomy _economyApi;
        private readonly IDatabase _redisDatabase;

        public QuotationService(IEconomy economyApi, IConnectionMultiplexer redisConnection)
        {
            _economyApi = economyApi;
            _redisDatabase = redisConnection.GetDatabase();
        }

        public async Task<Quotation> GetCurrencyInfo()
        {
            try
            {
                var cacheKey = "CurrencyInfo";
                var cachedData = await _redisDatabase.StringGetAsync(cacheKey);

                if (!cachedData.IsNullOrEmpty)
                {
                    return JsonConvert.DeserializeObject<Quotation>(cachedData);
                }

                var currencyInfo = await _economyApi.GetCurrencyInfo();
                var quotation = new Quotation
                {
                    CODE = currencyInfo["USDBRL"].Code,
                    Value = Convert.ToDecimal(currencyInfo["USDBRL"].High, CultureInfo.InvariantCulture),
                    CreatedAt = DateTime.Now
                };

                var json = JsonConvert.SerializeObject(quotation);
                // Armazenar a cotação no cache do Redis para uso futuro
                await _redisDatabase.StringSetAsync(cacheKey, json);
                await _redisDatabase.KeyExpireAsync(cacheKey, TimeSpan.FromHours(24));

                return quotation;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting currency information: {ex.Message}");
            }
        }
    }
}