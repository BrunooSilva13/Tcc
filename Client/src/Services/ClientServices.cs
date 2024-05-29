using Client.Models;
using Client.src.Repositories.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace Client.Services
{
    public class ClientService
    {
        private readonly IClientRepository _clientRepository;
       

        public ClientService(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
            
        }

        public Task<List<ClientModel>> GetClientsAsync() => _clientRepository.GetClientsAsync();

        public Task<ClientModel> GetClientByIdAsync(string id) => _clientRepository.GetClientByIdAsync(id);

        public async Task<bool> CreateClientAsync(ClientModel client)
        {

            if (client.Birthdate.HasValue && client.Birthdate > DateTime.Today)
            {
                throw new InvalidOperationException("The date of birth cannot be greater than the current date.");
            }


            if (await _clientRepository.ClientExistsAsync(client))
            {
                throw new InvalidOperationException("A customer with the same identifier already exists.");
            }

            return await _clientRepository.CreateClientAsync(client);
        }

        public async Task<bool> UpdateClientAsync(ClientModel client)
        {

            var existingClient = await _clientRepository.GetClientByIdAsync(client.Id);
            if (existingClient == null)
            {
                throw new InvalidOperationException("Customer not found.");
            }


            existingClient.Name = client.Name ?? existingClient.Name;
            existingClient.Surname = client.Surname ?? existingClient.Surname;
            existingClient.Email = client.Email ?? existingClient.Email;
            existingClient.Birthdate = client.Birthdate ?? existingClient.Birthdate;

            return await _clientRepository.UpdateClientAsync(existingClient);
        }

        public async Task<bool> DeleteClientAsync(string id)
        {

            var client = await _clientRepository.GetClientByIdAsync(id);
            if (client == null)
            {
                throw new InvalidOperationException("Client not found to delete.");
            }

            return await _clientRepository.DeleteClientAsync(id);
        }
    }
}
