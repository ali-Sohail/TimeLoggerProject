using System.ComponentModel;
using TimeLogger.Models;
using TimeLogger.ViewModels;
using Xamarin.Forms;

namespace TimeLogger.Views
{
  // Learn more about making custom code visible in the Xamarin.Forms previewer
  // by visiting https://aka.ms/xamarinforms-previewer
  [DesignTimeVisible(false)]
  public partial class ItemDetailPage : ContentPage
  {
    private readonly ItemDetailViewModel viewModel;

    public ItemDetailPage(ItemDetailViewModel viewModel)
    {
      InitializeComponent();

      BindingContext = this.viewModel = viewModel;
    }

    public ItemDetailPage()
    {
      InitializeComponent();

      Item item = new Item
      {
        Text = "Item 1",
        Description = "This is an item description."
      };

      viewModel = new ItemDetailViewModel(item);
      BindingContext = viewModel;
    }
  }
}