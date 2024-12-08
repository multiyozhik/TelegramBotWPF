using System.Windows;
using System.Text.Json;
using System.IO;
using Path = System.IO.Path;

namespace _10WPF_TelegramBot
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		TelegamMessageClient telegamMessageClient;
		string messagesHistoryPath = "MessagesHistory.txt";
		public MainWindow()
		{
			InitializeComponent();
			telegamMessageClient = new TelegamMessageClient(this);

			// у элемента управления MessageListBox окна MainWindow
			// по свойству ItemsSource идет ссылка на источник данных 
			// на свойство типа ObservableCollection<MessageLog>,
			// после чего в XAML можно привязать к конкретным свойствам MessageLog
			MessageListBox.ItemsSource = telegamMessageClient.BotMessageLog;
		}


		/// <summary>
		/// Метод позволяет отправить сообщение пользователю UserIDforSendingMessage
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void SentButton_Click(object sender, RoutedEventArgs e)
		{
			telegamMessageClient.SendMessage(UserIDforSendingMessage.Text, SentMessageText.Text);
			SentMessageText.Clear();
		}

		/// <summary>
		/// Метод позволяет открыть окно "Файлы" из меню приложения
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void FilesMenu_Click(object sender, RoutedEventArgs e)
		{
			var filesWindow = new FilesWindow
			{
				DataContext = telegamMessageClient.listFiles
			};
			filesWindow.Show();			
		}

		/// <summary>
		/// Метод позволяет открыть окно "Курс валют" из меню приложения
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CurrenciesMenu_Click(object sender, RoutedEventArgs e)
		{
			var currenciesWindow = new CurrenciesWindow
			{
				DataContext = GettingCurrencyValues.GetCurrencyValues()
			};
			currenciesWindow.ShowDialog();
		}

		/// <summary>
		/// Метод позволяет экспортировать историю сообщений в JSON в текстовый файл
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MessagesHistory_Click(object sender, RoutedEventArgs e)
		{
			if (telegamMessageClient.BotMessageLog.Count > 0)
			{
				var jsonMessageHistoryString = JsonSerializer.Serialize(telegamMessageClient.BotMessageLog);
				File.AppendAllText(messagesHistoryPath, jsonMessageHistoryString);
			}
			else
				MessageBox.Show(
					"Новые сообщения отсутствуют",
					"Предупреждение",
					MessageBoxButton.OK,
					MessageBoxImage.Warning);

		}
		/// <summary>
		/// Метод позволяет открыть текстовый файл истории сообщения по клику из меню приложения
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>		
		private void OpenHistory_Click(object sender, RoutedEventArgs e)
		{
			if (File.Exists(messagesHistoryPath))
			{
				System.Diagnostics.Process txtFileOpening = new System.Diagnostics.Process();
				txtFileOpening.StartInfo.FileName = "notepad.exe";
				string messagesHistoryTxtFilePath = Path.Combine(Directory.GetCurrentDirectory(), messagesHistoryPath);
				txtFileOpening.StartInfo.Arguments = messagesHistoryTxtFilePath;
				txtFileOpening.Start();
			}
			else
				MessageBox.Show(
					"Файла истории сообщений не существует",
					"Предупреждение",
					MessageBoxButton.OK,
					MessageBoxImage.Warning);
		}
	}
}
