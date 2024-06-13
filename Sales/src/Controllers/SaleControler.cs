using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sales;
using MySql.Data.MySqlClient;
// Adicione outros namespaces necessários conforme apropriado





[ApiController]
[Route("api/[controller]")]
public class SalesController : ControllerBase
{
    private readonly ISaleService _saleService;
    private readonly IHttpClientFactory _httpClientFactory;

    public SalesController(ISaleService saleService, IHttpClientFactory httpClientFactory)
    {
        _saleService = saleService;
        _httpClientFactory = httpClientFactory;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Sale>>> GetSales()
    {
        var sales = await _saleService.GetAllSalesAsync();
        return Ok(sales);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Sale>> GetSale(int id)
    {
        var sale = await _saleService.GetSaleByIdAsync(id);
        if (sale == null)
        {
            return NotFound();
        }
        return Ok(sale);
    }

    [HttpPost]
    public async Task<ActionResult<Sale>> PostSale(Sale sale)
    {
        var client = _httpClientFactory.CreateClient();

        // Obter informações do cliente
        var clientResponse = await client.GetAsync($"https://localhost:5001/api/clients/{sale.ClientId}");
        if (!clientResponse.IsSuccessStatusCode)
        {
            return BadRequest("Cliente não encontrado.");
        }

        // Obter informações do produto
        var productResponse = await client.GetAsync($"https://localhost:5002/api/products/{sale.ProductId}");
        if (!productResponse.IsSuccessStatusCode)
        {
            return BadRequest("Produto não encontrado.");
        }

        // Obter informações de moeda
        var currencyResponse = await client.GetAsync($"https://localhost:5003/api/coins/{sale.Currency}");
        if (!currencyResponse.IsSuccessStatusCode)
        {
            return BadRequest("Moeda não encontrada.");
        }

        // Simulação de cálculo do preço total
        sale.TotalPrice = sale.Quantity * 100; // Exemplo de cálculo

        await _saleService.AddSaleAsync(sale);
        return CreatedAtAction(nameof(GetSale), new { id = sale.SaleId }, sale);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutSale(int id, Sale sale)
    {
        if (id != sale.SaleId)
        {
            return BadRequest();
        }
        await _saleService.UpdateSaleAsync(sale);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSale(int id)
    {
        await _saleService.DeleteSaleAsync(id);
        return NoContent();
    }
}
