
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TimeLogger.Views
{
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class DailyLogsPage : ContentPage
  {
    public DailyLogsPage()
    {
      InitializeComponent();
    }
    protected override void OnAppearing()
    {
      base.OnAppearing();

    }

    private void canvas_PaintSurface(object sender, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs e)
    {
      ViewModels.DailyLogsPageViewModel viewModel = this.BindingContext as ViewModels.DailyLogsPageViewModel;
      viewModel.CanvasViewPaintSurface(sender, e);
    }

    private void canvas_Touch(object sender, SkiaSharp.Views.Forms.SKTouchEventArgs e)
    {
      ViewModels.DailyLogsPageViewModel viewModel = this.BindingContext as ViewModels.DailyLogsPageViewModel;
      viewModel.CanvasViewTouch(sender, e);
    }
  }
}