using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TimeLogger.Helpers;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TimeLogger.Services
{
  public sealed class RestClient : IRestClient
  {
    private static RestClient _instance;
    public static RestClient Instance => _instance ?? (_instance = new RestClient());

    private async Task<string> GetAuthToken()
    {
      try
      {
        string token = await SecureStorage.GetAsync("oauth_token").ConfigureAwait(true);
        return await Task.FromResult(token);
      }
      catch (Exception ex)
      {
        Debug.WriteLine(ex.Message);
        return await Task.FromResult(string.Empty);
      }
    }

    private System.Net.Http.HttpClient GetClient(HttpClientHandler handler = null)
    {
      HttpClient client = handler == null ? new HttpClient() : new HttpClient(handler, true);
      client.Timeout = TimeSpan.FromSeconds(60);
      return client;
    }

    private async Task<System.Net.Http.HttpResponseMessage> RequestAsync(HttpMethod method, string url, object payload = null)
    {
      NetworkAccess current = Connectivity.NetworkAccess;
      if (current == NetworkAccess.Internet)
      {
        try
        {
          HttpRequestMessage request = PrepareRequest(method, url, payload);

          string authToken = await GetAuthToken().ConfigureAwait(true);
          
          request.Headers.Add("Authorization", authToken);
          
          Debug.WriteLine($"Used Token : {authToken}");

          return GetClient().SendAsync(request, System.Net.Http.HttpCompletionOption.ResponseContentRead).Result;
        }
        catch (Exception ex)
        {
          Debug.WriteLine("Error while Sending Request : " + ex.Message);
          Crashes.TrackError(ex, new Dictionary<string, string> { { "Class Name : ", GetType().Name }, { "Method Name : ", MethodBase.GetCurrentMethod().Name } });
          throw new RestClientException(ex.Message, ex);
        }
      }
      else
      {
        //DependencyService.Get<IToastService>().ShortAlert("Internet connection error");
      }

      return Task.FromResult<HttpResponseMessage>(null).Result;
    }

    private System.Net.Http.HttpRequestMessage PrepareRequest(HttpMethod method, string url, object payload)
    {
      Uri uri = PrepareUri(url);
      HttpRequestMessage request = new HttpRequestMessage(method, uri);
      if (payload != null)
      {
        string json = JsonConvert.SerializeObject(payload);
        request.Content = new StringContent(json, Encoding.UTF8, "application/json");
        Debug.WriteLine($"API URL :{uri} \nAPI Request :{json}");
      }
      return request;
    }

    private Uri PrepareUri(string url)
    {
      return new Uri(url);
    }

    private readonly Action<HttpStatusCode, string> _defaultErrorHandler = (statusCode, body) =>
    {
      if (statusCode < HttpStatusCode.OK || statusCode >= HttpStatusCode.BadRequest)
      {
        Debug.WriteLine($"Request responded with status code={statusCode}, response={body}");

        throw new RestClientException(statusCode, body);
      }
    };

    private void HandleIfErrorResponse(HttpStatusCode statusCode, string content, Action<HttpStatusCode, string> errorHandler = null)
    {
      if (errorHandler != null)
      {
        errorHandler(statusCode, content);
      }
      else
      {
        _defaultErrorHandler(statusCode, content);
      }
    }

    private T GetValue<T>(string value)
    {
      return (T)Convert.ChangeType(value, typeof(T));
    }

    public async Task<T> GetAsync<T>(string url, bool useAuthToken = true)
    {
      try
      {
        HttpResponseMessage response = await RequestAsync(HttpMethod.Get, url).ConfigureAwait(false);
        string content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        Debug.WriteLine($"API URL :{url} \nAPI Response :{content}");
        HandleIfErrorResponse(response.StatusCode, content);
        if (typeof(T) == typeof(string))
        {
          return GetValue<T>(content);
        }

        return JsonConvert.DeserializeObject<T>(content);
      }
      catch (System.Net.WebException ex)
      {
        Debug.WriteLine("Error" + ex.StackTrace);
        Crashes.TrackError(ex, new Dictionary<string, string> { { "Class Name : ", GetType().Name }, { "Method Name : ", MethodBase.GetCurrentMethod().Name } });
      }
      catch (HttpRequestException ex)
      {
        Debug.WriteLine("Error in GET Request :" + ex.Message);
        Crashes.TrackError(ex, new Dictionary<string, string> { { "Class Name : ", GetType().Name }, { "Method Name : ", MethodBase.GetCurrentMethod().Name } });
      }
      return default(T);
    }

    public async Task<T> PostAsync<T>(string url, object payload, bool useAuthToken = true)
    {
      try
      {
        HttpResponseMessage response = await RequestAsync(HttpMethod.Post, url, payload).ConfigureAwait(false);
        string content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        Debug.WriteLine($"API URL :{url} \nAPI Response :{content}");
        HandleIfErrorResponse(response.StatusCode, content);
        if (typeof(T) == typeof(string))
        {
          return GetValue<T>(content);
        }
        return JsonConvert.DeserializeObject<T>(content);
      }
      catch (JsonException ex)
      {
        Debug.WriteLine("Deserialize Error in POST Request :" + ex.Message);
        Crashes.TrackError(ex, new Dictionary<string, string> { { "Class Name : ", GetType().Name }, { "Method Name : ", MethodBase.GetCurrentMethod().Name } });
      }
      catch (HttpRequestException ex)
      {
        Debug.WriteLine("Error in POST Request :" + ex.Message);
        Crashes.TrackError(ex, new Dictionary<string, string> { { "Class Name : ", GetType().Name }, { "Method Name : ", MethodBase.GetCurrentMethod().Name } });
      }
      catch (Exception ex)
      {
        Debug.WriteLine("Error in POST Request :" + ex.Message);
        Crashes.TrackError(ex, new Dictionary<string, string> { { "Class Name : ", GetType().Name }, { "Method Name : ", MethodBase.GetCurrentMethod().Name } });
      }
      return default(T);
    }

    public async Task<T> PutAsync<T>(string url, object payload, bool useAuthToken = true)
    {
      try
      {
        HttpResponseMessage response = await RequestAsync(HttpMethod.Put, url, payload).ConfigureAwait(false);
        string content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        HandleIfErrorResponse(response.StatusCode, content);
        if (typeof(T) == typeof(string))
        {
          return GetValue<T>(content);
        }

        return JsonConvert.DeserializeObject<T>(content);
      }
      catch (HttpRequestException ex)
      {
        Debug.WriteLine("Error in PUT Request :" + ex.Message);
        Crashes.TrackError(ex, new Dictionary<string, string> { { "Class Name : ", GetType().Name }, { "Method Name : ", MethodBase.GetCurrentMethod().Name } });
      }
      return default(T);
    }

    public async Task<T> PostAsyncWithoutReturnContent<T>(string url, object payload, bool useAuthToken = true)
    {
      try
      {
        HttpResponseMessage response = await RequestAsync(HttpMethod.Post, url, payload).ConfigureAwait(false);
        string content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        HandleIfErrorResponse(response.StatusCode, content);
        if (typeof(T) == typeof(string))
        {
          return GetValue<T>(content);
        }
      }
      catch (HttpRequestException ex)
      {
        Debug.WriteLine("Error in POST Request :" + ex.Message);
        Crashes.TrackError(ex, new Dictionary<string, string> { { "Class Name : ", GetType().Name }, { "Method Name : ", MethodBase.GetCurrentMethod().Name } });
      }
      return default(T);
    }

    public async Task<T> PutAsyncWithoutReturnContent<T>(string url, object payload, bool useAuthToken = true)
    {
      try
      {
        HttpResponseMessage response = await RequestAsync(HttpMethod.Put, url, payload).ConfigureAwait(false);
        string content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        HandleIfErrorResponse(response.StatusCode, content);
        if (typeof(T) == typeof(string))
        {
          return GetValue<T>(content);
        }
      }
      catch (HttpRequestException ex)
      {
        Debug.WriteLine("Error in POST Request :" + ex.Message);
        Crashes.TrackError(ex, new Dictionary<string, string> { { "Class Name : ", GetType().Name }, { "Method Name : ", MethodBase.GetCurrentMethod().Name } });
      }
      return default(T);
    }

    public async Task<T> DeleteAsync<T>(string url, bool useAuthToken = true)
    {
      try
      {
        HttpResponseMessage response = await RequestAsync(HttpMethod.Delete, url).ConfigureAwait(false);
        string content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        HandleIfErrorResponse(response.StatusCode, content);
        if (typeof(T) == typeof(string))
        {
          return GetValue<T>(content);
        }

        return JsonConvert.DeserializeObject<T>(content);
      }
      catch (HttpRequestException ex)
      {
        Debug.WriteLine("Error in DELETE Request :" + ex.Message);
        Crashes.TrackError(ex, new Dictionary<string, string> { { "Class Name : ", GetType().Name }, { "Method Name : ", MethodBase.GetCurrentMethod().Name } });
      }
      return default(T);
    }
  }
}