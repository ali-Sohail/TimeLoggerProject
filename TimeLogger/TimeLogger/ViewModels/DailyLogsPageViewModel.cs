using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TimeLogger.Helpers;
using TimeLogger.Web.Models;
using Xamarin.Forms;
using SkiaSharp.Views.Forms;
using SkiaSharp;

namespace TimeLogger.ViewModels
{
	public class DailyLogsPageViewModel : BaseViewModel
	{
		public ICommand UpdateCommand { get; private set; }

		private SKCanvasView sKCanvasView;
		public SKCanvasView SKCanvasView
		{
			get => sKCanvasView;
			set => SetProperty(ref sKCanvasView, value);
		}
		private IEnumerable<DayLog> dayLogs;
		public IEnumerable<DayLog> DayLogs
		{
			get => dayLogs;
			set => SetProperty(ref dayLogs, value);
		}

		private DayLog dayLog;
		public DayLog DayLog
		{
			get => dayLog;
			set => SetProperty(ref dayLog, value);
		}


		public DailyLogsPageViewModel()
		{
			GetMockData();
			GetDayLogsAsync().ConfigureAwait(true);
		}

		private async Task GetDayLogsAsync()
		{
			try
			{
				DayLogs = await DataStore.GetItemsAsync(true);
				DayLog = DayLogs?.FirstOrDefault();
			}
			catch (Exception ex)
			{
				ExceptionLogger.LogException(ex);
			}
		}

		private void GetMockData()
		{
			SKCanvasView = new SKCanvasView()
			{
				EnableTouchEvents = true,
			};

			this.SKCanvasView.Touch += CanvasViewTouch;
			this.SKCanvasView.PaintSurface += CanvasViewPaintSurface;

			DayLog = new DayLog
			{
				Id = 0,
				InTime = DateTime.Now,
				UserId = "1",
				Description = $"Log for {DateTime.Now.ToLongDateString()}",
			};
		}

		private void CanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs e)
		{
			SKImageInfo info = e.Info;
			SKSurface surface = e.Surface;
			SKCanvas canvas = surface.Canvas;

			canvas.Clear();

			SKColor[] colors = { SKColors.Red, SKColors.Green, SKColors.Blue };
			//SKShaderTileMode tileMode =
			//	(SKShaderTileMode)(tileModePicker.SelectedIndex == -1 ?
			//								0 : tileModePicker.SelectedItem);


			SKPoint startPoint = new SKPoint(0, 0);
			SKPoint endPoint = new SKPoint(0, 0);

			SKMatrix sKMatrix = SKMatrix.MakeIdentity();


			//SKShaderTileMode sKShaderTileMode = 
			using (SKPaint paint = new SKPaint())
			{
				paint.Shader = SKShader.CreateLinearGradient(startPoint,
															 endPoint,
															 colors,
															 null,
															 SKShaderTileMode.Repeat);
				canvas.DrawRect(info.Rect, paint);
			}

			// Display the touch points here rather than by TouchPoint
			//using (SKPaint paint = new SKPaint())
			//{
			//	paint.Style = SKPaintStyle.Stroke;
			//	paint.Color = SKColors.Black;
			//	paint.StrokeWidth = 3;

			//	foreach (TouchPoint touchPoint in touchPoints)
			//	{
			//		canvas.DrawCircle(touchPoint.Center, touchPoint.Radius, paint);
			//	}

			//	// Draw gradient line connecting touchpoints
			//	canvas.DrawLine(touchPoints[0].Center, touchPoints[1].Center, paint);

			//	// Draw lines perpendicular to the gradient line
			//	SKPoint vector = touchPoints[1].Center - touchPoints[0].Center;
			//	float length = (float)Math.Sqrt(Math.Pow(vector.X, 2) +
			//									Math.Pow(vector.Y, 2));
			//	vector.X /= length;
			//	vector.Y /= length;
			//	SKPoint rotate90 = new SKPoint(-vector.Y, vector.X);
			//	rotate90.X *= 200;
			//	rotate90.Y *= 200;

			//	canvas.DrawLine(touchPoints[0].Center,
			//					touchPoints[0].Center + rotate90,
			//					paint);

			//	canvas.DrawLine(touchPoints[0].Center,
			//					touchPoints[0].Center - rotate90,
			//					paint);

			//	canvas.DrawLine(touchPoints[1].Center,
			//					touchPoints[1].Center + rotate90,
			//					paint);

			//	canvas.DrawLine(touchPoints[1].Center,
			//					touchPoints[1].Center - rotate90,
			//					paint);
		}


		private void CanvasViewTouch(object sender, SKTouchEventArgs e)
		{
			//throw new NotImplementedException();
		}
	}
}
