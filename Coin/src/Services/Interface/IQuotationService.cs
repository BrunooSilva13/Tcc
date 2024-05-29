using System;
using System.Collections.Generic;
using System.Linq;
using coin.src.Models;
using coin.src.Services.Refit;

namespace coin.src.Services.Interfaces
{
    public interface IQuotationService
    {
        Task<Quotation> GetCurrencyInfo();
    }
}