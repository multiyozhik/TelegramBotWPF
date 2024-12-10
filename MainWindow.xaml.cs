using System.Windows;
using System.Text.Json;
using System.IO;
using Path = System.IO.Path;
using System;
using Telegram.Bot.Types;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace _10WPF_TelegramBot
{
    /// <summary>
    /// Класс глав. окна (точка входа StartupUri="MainWindow.xaml").
	/// В конструкторе инициализ. объект типа TelegamMessageClient для взаимод. с API бота, и 
	/// инициализ. ItemsSource у эл-та управл. MessageListBox глав окна
    /// </summary>
    public partial class MainWindow : Window
	{
        readonly TelegamMessageClient telegamMessageClient = new();
        readonly string messagesHistoryPath = "MessagesHistory.txt";
		public MainWindow()
		{
			InitializeComponent();
            MessageListBox.ItemsSource = telegamMessageClient.BotMessageLog;
		}

        //Метод для отправки пользователю (скрытый TextBlock с именем UserIDforSendingMessage)
		//текста сообщения (TextBox с именем SentMessageText)
        void SentButton_Click(object sender, RoutedEventArgs e)
		{
			var selectedMessageLog = MessageListBox.SelectedItem;

			if (selectedMessageLog is not null)
			{
                var firstNameSelectedMessageLog = ((MessageLog)selectedMessageLog).FirstName;
                telegamMessageClient.SendMessage(UserIDforSendingMessage.Text, firstNameSelectedMessageLog, SentMessageText.Text);
                SentMessageText.Clear();
            }
            else
            {
                MessageBox.Show(
                    "Сообщение не отправлено. Не выбран пользователь, которому отправлять сообщение",
                    "Предупреждение",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
        }

		//Метод для открытия диалог. окна "Файлы" из меню (список скаченных файлов в виде текста)
		void FilesMenu_Click(object sender, RoutedEventArgs e)
		{
			var filesWindow = new FilesWindow
			{
				DataContext = telegamMessageClient.ListFiles
			};
			filesWindow.Show();			
		}

		//Метод для открытия диалог. окна "Курс валют" из меню (в виде таблицы наименование - значение, руб.)
		void CurrenciesMenu_Click(object sender, RoutedEventArgs e)
		{
			var currenciesWindow = new CurrenciesWindow
			{
				DataContext = GettingCurrencyValues.GetCurrencyValues()
			};
			currenciesWindow.ShowDialog();
		}

        //Метод для экспорта истории сообщений в JSON в "MessagesHistory.txt" (в папке проекта)
        async void MessagesHistory_Click(object sender, RoutedEventArgs e)
		{
			if (telegamMessageClient.BotMessageLog.Count > 0)
			{
                var options = new JsonSerializerOptions { WriteIndented = true };
				var jsonMessageHistoryString = JsonSerializer.Serialize(telegamMessageClient.BotMessageLog, options);
				await System.IO.File.WriteAllTextAsync(messagesHistoryPath, jsonMessageHistoryString, encoding: Encoding.UTF8);
			}
			else
				MessageBox.Show(
					"Новые сообщения отсутствуют",
					"Предупреждение",
					MessageBoxButton.OK,
					MessageBoxImage.Warning);

		}

        //Метод для открытия файла истории сообщения "MessagesHistory.txt" по клику из меню
        void OpenHistory_Click(object sender, RoutedEventArgs e)
		{
			if (System.IO.File.Exists(messagesHistoryPath))
			{
				System.Diagnostics.Process txtFileOpening = new();
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
