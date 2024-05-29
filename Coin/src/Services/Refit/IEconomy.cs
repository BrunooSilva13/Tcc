using System;
using System.Collections.Generic;
using Refit;

namespace coin.src.Services.Refit
{
    public interface IEconomy
    {
        [Get("/last/USD-BRL")]
        Task<Dictionary<string, CurrencyInfo>> GetCurrencyInfo();
    }

    public class CurrencyInfo
    {
        public string? Code { get; set; }
        public string? High { get; set; }
    }
}