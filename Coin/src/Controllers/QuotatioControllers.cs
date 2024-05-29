using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using coin.src.Services.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using coin.src.Models;

namespace coin.src.Controller
{
    [Route("api")]
    public class QuotationController : ControllerBase
    {
        private readonly IDistributedCache _cache;
        private readonly IQuotationService _quotationService;

        

        [HttpGet]
        public async Task<ActionResult<Quotation>> Get()
        {
            try
            {
                var res = await _quotationService.GetCurrencyInfo();
                return StatusCode(201, res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}