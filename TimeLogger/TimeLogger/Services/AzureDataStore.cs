using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TimeLogger.Models;
using Xamarin.Essentials;

namespace TimeLogger.Services
{
    public class AzureDataStore : IDataStore<Item>
    {
        private readonly HttpClient client;
        private IEnumerable<Item> items;

        public AzureDataStore()
        {
            client = new HttpClient
            {
                BaseAddress = new Uri($"{App.AzureBackendUrl}/")
            };

            items = new List<Item>();
        }

        private bool IsConnected => Connectivity.NetworkAccess == NetworkAccess.Internet;
        public async Task<IEnumerable<Item>> GetItemsAsync(bool forceRefresh = false)
        {
            if (forceRefresh && IsConnected)
            {
                string json = await client.GetStringAsync($"api/item");
                items = await Task.Run(() => JsonConvert.DeserializeObject<IEnumerable<Item>>(json));
            }

            return items;
        }

        public async Task<Item> GetItemAsync(string id)
        {
            if (id != null && IsConnected)
            {
                string json = await client.GetStringAsync($"api/item/{id}");
                return await Task.Run(() => JsonConvert.DeserializeObject<Item>(json));
            }

            return null;
        }

        public async Task<bool> AddItemAsync(Item item)
        {
            if (item == null || !IsConnected)
            {
                return false;
            }

            string serializedItem = JsonConvert.SerializeObject(item);

            HttpResponseMessage response = await client.PostAsync($"api/item", new StringContent(serializedItem, Encoding.UTF8, "application/json"));

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateItemAsync(Item item)
        {
            if (item == null || item.Id == null || !IsConnected)
            {
                return false;
            }

            string serializedItem = JsonConvert.SerializeObject(item);
            byte[] buffer = Encoding.UTF8.GetBytes(serializedItem);
            ByteArrayContent byteContent = new ByteArrayContent(buffer);

            HttpResponseMessage response = await client.PutAsync(new Uri($"api/item/{item.Id}"), byteContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            if (string.IsNullOrEmpty(id) && !IsConnected)
            {
                return false;
            }

            HttpResponseMessage response = await client.DeleteAsync($"api/item/{id}");

            return response.IsSuccessStatusCode;
        }
    }
}
