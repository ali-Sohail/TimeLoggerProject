using Microsoft.AppCenter.Crashes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization;
using Xamarin.Forms;

namespace TimeLogger.Helpers
{
  [Serializable]
  public sealed class RestClientException : Exception, ISerializable
  {
    [NonSerialized]
    private readonly HttpStatusCode statusCode;

    [NonSerialized]
    private readonly string responseBody;

    public RestClientException(string message)
        : base(message)
    {
      Device.BeginInvokeOnMainThread(() =>
      {
        Debug.WriteLine($"Serialization error {message}");
        //DependencyService.Get<IToastService>().ShortAlert("Serialization error");
      });
    }

    public RestClientException(string message, Exception innerException)
        : base(message, innerException)
    {
      Device.BeginInvokeOnMainThread(() =>
      {
        Debug.WriteLine($"RequestAsync Failed : {message}");
        Crashes.TrackError(innerException, new Dictionary<string, string> { { "Class Name : ", GetType().Name }, { "Method Name : ", MethodBase.GetCurrentMethod().Name } });
        //DependencyService.Get<IToastService>().ShortAlert($"API error : {innerException.Message}");
      });
    }

    public RestClientException(Exception innerException)
        : base(innerException.Message, innerException)
    {
      Debug.WriteLine($"API error {innerException.Message}");
    }

    public RestClientException(HttpStatusCode statusCode, string responseBody)
        : base($"Request responded with status code={statusCode}, response={responseBody}")
    {
      this.statusCode = statusCode;
      this.responseBody = responseBody;

      if (statusCode == HttpStatusCode.Unauthorized || responseBody.Contains("Provided token is expired"))
      {
        // Force Logout
        Device.BeginInvokeOnMainThread(() =>
        {
          //Settings.ClearAllData();
          //Task.Delay(100);
          //App.Current.MainPage = new NavigationPage(new LoginPage());
          //Task.Delay(1500);
          //DependencyService.Get<IToastService>().ShortAlert("Session Expires: Please login");
          //SecureStorage.Remove("oauth_token");
        });
      }
    }

    private RestClientException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
      Debug.WriteLine("Serialization error");
    }
  }
}