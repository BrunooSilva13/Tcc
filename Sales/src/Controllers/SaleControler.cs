using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class SalesController : ControllerBase
{
    private readonly ISaleRepository _saleRepository;
    private readonly IHttpClientFactory _httpClientFactory;

    public SalesController(ISaleRepository saleRepository, IHttpClientFactory httpClientFactory)
    {
        _saleRepository = saleRepository;
        _httpClientFactory = httpClientFactory;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Sale>>> GetSales()
    {
        return Ok(await _saleRepository.GetAllSales());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Sale>> GetSale(int id)
    {
        var sale = await _saleRepository.GetSaleById(id);
        if (sale == null)
        {
            return NotFound();
        }
        return Ok(sale);
    }

    [HttpPost]
    public async Task<ActionResult<Sales>> PostSales(Sales sales)
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

        await _saleRepository.AddSale(sale);
        return CreatedAtAction(nameof(GetSale), new { id = sale.Id }, sale);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutSale(int id, Sale sale)
    {
        if (id != sale.Id)
        {
            return BadRequest();
        }
        await _saleRepository.UpdateSale(sale);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSale(int id)
    {
        await _saleRepository.DeleteSale(id);
        return NoContent();
    }
}
