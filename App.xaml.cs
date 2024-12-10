using System.Threading;
using System.Windows;
using System.Globalization;
using System.Windows.Markup;
using System.Runtime.CompilerServices;

namespace _10WPF_TelegramBot
{
	/// <summary>
	/// Глобальная установка локали при запуске приложения 
	/// (для корректного получения числовых данных с сайта через точку)
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
