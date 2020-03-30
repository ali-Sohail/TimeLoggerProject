using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TimeLogger.Web.Models;
using Xamarin.Forms;

namespace TimeLogger.ViewModels
{
  public class DailyLogsPageViewModel : BaseViewModel
  {
    public ICommand UpdateCommand { get; private set; }
    public DailyLogsPageViewModel()
    {
      UpdateCommand = new Command(async () => await Update());
    }

    private async Task Update()
    {
      try
      {
        IEnumerable<DayLog> result = await DataStore.GetItemsAsync(true);

        StringBuilder stringBuilder = new StringBuilder(Title);

        foreach (DayLog item in result)
        {
          System.Diagnostics.Debug.WriteLine($" {item.Description}");

          stringBuilder.Append($"1. {item.Description}\t\n");
        }
      }
      catch (Exception ex)
      {
        System.Diagnostics.Debug.WriteLine(ex.Message);
      }
    }
  }
}
