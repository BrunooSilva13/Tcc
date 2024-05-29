using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Client.Models;

namespace Client.src.Repositories.Interface
{
    public interface IClientRepository
    {
        Task<List<ClientModel>> GetClientsAsync();
        Task<ClientModel> GetClientByIdAsync(string id);
        Task<bool> CreateClientAsync(ClientModel client);
        Task<bool> UpdateClientAsync(ClientModel client);
        Task<bool> DeleteClientAsync(string id);
        Task<bool> ClientExistsAsync(ClientModel client);
        
        
    }
}