using Microsoft.AspNetCore.Mvc;
using Client.Models;
using Client.Services;
using System.Threading.Tasks;
using System;

namespace Client.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly ClientService _clientService;
        private readonly ILogger<ClientController> _logger;
        
        public ClientController(ClientService clientService, ILogger<ClientController> logger)
        {
            _clientService = clientService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetClients()
        {
            try
            {
                var clients = await _clientService.GetClientsAsync();
                return Ok(clients); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetClients method.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetClientById(string id)
        {
            try
            {
                var client = await _clientService.GetClientByIdAsync(id);
                if (client == null)
                {
                    return NotFound("Client not found.");
                }
                return Ok(client);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in GetClientById method with ID {id}.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClient(string id, [FromBody] ClientModel client)
        {
            if (id != client.Id)
            {
                return BadRequest("Mismatched ID in URL and body.");
            }

            try
            {
                var updated = await _clientService.UpdateClientAsync(client);
                if (!updated)
                {
                    return NotFound("Client not found or update failed.");
                }
                return Ok("Client updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in UpdateClient method with ID {id}.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateClient([FromBody] ClientModel client)
        {
            try
            {
                var created = await _clientService.CreateClientAsync(client);
                if (!created)
                {
                    return BadRequest("Failed to create client.");
                }
                return Ok("Client created successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in CreateClient method.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(string id)
        {
            try
            {
                var deleted = await _clientService.DeleteClientAsync(id);
                if (!deleted)
                {
                    return NotFound("Client not found or delete failed.");
                }
                return Ok("Client deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in DeleteClient method with ID {id}.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
