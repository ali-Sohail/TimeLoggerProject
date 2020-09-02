using System.Threading.Tasks;

namespace TimeLogger.Services
{
    public interface IRestClient
    {
        Task<T> GetAsync<T>(string url, bool useAuthToken = true);

        Task<T> PostAsync<T>(string url, object payload, bool useAuthToken = true);

        Task<T> PutAsync<T>(string url, object payload, bool useAuthToken = true);

        Task<T> PutAsyncWithoutReturnContent<T>(string url, object payload, bool useAuthToken = true);

        Task<T> DeleteAsync<T>(string url, bool useAuthToken = true);

        Task<T> PostAsyncWithoutReturnContent<T>(string url, object payload, bool useAuthToken = true);
    }
}