using System.Threading;
using System.Windows;
using System.Globalization;
using System.Windows.Markup;

namespace _10WPF_TelegramBot
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		private void Application_Startup(object sender, StartupEventArgs e)
		{
			Thread.CurrentThread.CurrentCulture = new CultureInfo("ru-ru");
			Thread.CurrentThread.CurrentUICulture = new CultureInfo("ru-ru");
			FrameworkElement.LanguageProperty.OverrideMetadata(
				typeof(FrameworkElement),
				new FrameworkPropertyMetadata(
					XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
		}
	}
}
