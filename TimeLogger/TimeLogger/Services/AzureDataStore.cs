using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TimeLogger.Helpers;
using TimeLogger.Web.Models;
using Xamarin.Essentials;

namespace TimeLogger.Services
{
	public class AzureDataStore : IDataStore<DayLog>
	{
		private readonly HttpClient client;
		private IEnumerable<DayLog> items;

		public AzureDataStore()
		{
			client = new HttpClient
			{
				BaseAddress = new Uri($"{App.AzureBackendUrl}/"),
				Timeout = new TimeSpan(0, 0, 10)
			};

			items = new List<DayLog>();
		}

		private bool IsConnected => Connectivity.NetworkAccess == NetworkAccess.Internet;
		public async Task<IEnumerable<DayLog>> GetItemsAsync(bool forceRefresh = false)
		{
			try
			{
				if (forceRefresh && IsConnected)
				{
					string json = await client.GetStringAsync($"api/DayLogs");
					items = await Task.Run(() => JsonConvert.DeserializeObject<IEnumerable<DayLog>>(json));
				}
				return items;
			}
			catch (Exception ex)
			{
				ExceptionLogger.LogException(ex);
				throw;
			}
		}

		public async Task<DayLog> GetItemAsync(string id)
		{
			if (id != null && IsConnected)
			{
				string json = await client.GetStringAsync($"api/DayLogs/{id}");
				return await Task.Run(() => JsonConvert.DeserializeObject<DayLog>(json));
			}
			return null;
		}

		public async Task<bool> AddItemAsync(DayLog item)
		{
			if (item == null || !IsConnected)
			{
				return false;
			}

			string serializedItem = JsonConvert.SerializeObject(item);

			HttpResponseMessage response = await client.PostAsync($"api/DayLogs", new StringContent(serializedItem, Encoding.UTF8, "application/json"));

			return response.IsSuccessStatusCode;
		}

		public async Task<bool> UpdateItemAsync(DayLog item)
		{
			if (item == null || !IsConnected)
			{
				return false;
			}

			string serializedItem = JsonConvert.SerializeObject(item);
			byte[] buffer = Encoding.UTF8.GetBytes(serializedItem);
			ByteArrayContent byteContent = new ByteArrayContent(buffer);

			HttpResponseMessage response = await client.PutAsync(new Uri($"api/DayLogs/{item.Id}"), byteContent);

			return response.IsSuccessStatusCode;
		}

		public async Task<bool> DeleteItemAsync(string id)
		{
			if (string.IsNullOrEmpty(id) && !IsConnected)
			{
				return false;
			}

			HttpResponseMessage response = await client.DeleteAsync($"api/DayLogs/{id}");

			return response.IsSuccessStatusCode;
		}
	}
}
