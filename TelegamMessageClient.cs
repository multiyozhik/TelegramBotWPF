using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using System.Collections.ObjectModel;
using System.Windows;
using System.Diagnostics;

namespace _10WPF_TelegramBot
{
	class TelegamMessageClient
	{
		public TelegramBotClient bot;
		public ObservableCollection<MessageLog> BotMessageLog { get; set; }

		// путь для скачивания файлов будет в системную папку с документами
		static readonly string path = Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments);
		// объявляем listFiles для списка имен скаченных файлов
		public ObservableCollection<string> listFiles = new ObservableCollection<string>();
		
		static readonly Object SyncObject = new object();

		// конструктор, будет передаваться окно WPF с токеном бота, при этом
		// в свойстве BotMessageLog типа ObservableCollection добавление элементов типа MessageLog
		public TelegamMessageClient(MainWindow window, string pathToken = "Token.txt")
		{
			string token = System.IO.File.ReadAllText(pathToken);
			bot = new TelegramBotClient(token);
			BotMessageLog = new ObservableCollection<MessageLog>();

			using var cts = new CancellationTokenSource();
			var receiverOptions = new ReceiverOptions
			{
				AllowedUpdates = { }
			};
			// для синхронизации изменений в листе файлов и синхронном отображении в окне списка файлов
			System.Windows.Data.BindingOperations.EnableCollectionSynchronization(listFiles, SyncObject);
			// для синхронизации изменений в истории сообщений и синхронном отображении в окне списка сообщений
			System.Windows.Data.BindingOperations.EnableCollectionSynchronization(BotMessageLog, SyncObject);

			bot.StartReceiving(HandleUpdateAsync, HandleErrorAsync, receiverOptions, cancellationToken: cts.Token);
		}

		// метод обработки обновлений
		async Task HandleUpdateAsync(
			ITelegramBotClient bot,
			Update update,
			CancellationToken cancellationToken)
		{
			// если пришло сообщение Message https://core.telegram.org/bots/api#message => обрабатываем его				
			if (update.Type == UpdateType.Message)
			{
				// если пришло текстовое сообщение 
				if (update.Message.Type == MessageType.Text && update?.Message?.Text != null)
				{
					var newMessageLog = new MessageLog(
						update.Message.Chat.Id,
						update.Message.Chat.FirstName,
						DateTime.Now.ToLongDateString(), update.Message.Text);
					BotMessageLog.Add(newMessageLog);
				}
				// если пришел аудиофайл https://core.telegram.org/bots/api/#audio => скачиваем его 
				if (update.Message.Type == MessageType.Audio)
					await DownLoadAudio(bot, update);

				// если пришел документ https://core.telegram.org/bots/api/#document => скачиваем его 
				if (update.Message.Type == MessageType.Document)
					await DownLoadDocument(bot, update);

				// если пришла картинка https://core.telegram.org/bots/api/#photosize => скачиваем
				if (update.Message.Type == MessageType.Photo)
					await DownLoadPhoto(bot, update);
			}
		}

		// обработка исключений
		static Task HandleErrorAsync(ITelegramBotClient bot, Exception exception, CancellationToken cancellationToken)
		{
			var ErrorMessage = exception switch
			{
				ApiRequestException apiRequestException
					=> $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
				_ => exception.ToString()
			};
			Debug.WriteLine(ErrorMessage);
			return Task.CompletedTask;
		}

		// метод для скачивания приходящего аудиофайла
		async Task DownLoadAudio(ITelegramBotClient bot, Update update)
		{
			var audioName = update.Message.Audio.FileName;
			var audioFileId = update.Message.Audio.FileId;
			var fullPath = Path.Combine(path, audioName);
			await DownLoad(bot, audioFileId, fullPath);
			listFiles.Add(audioName);			
		}	

		// метод для скачивания приходящего документа
		async Task DownLoadDocument(ITelegramBotClient bot, Update update)
		{
			var documentName = update.Message.Document.FileName;
			var documentId = update.Message.Document.FileId;
			var fullPath = Path.Combine(path, documentName);
			await DownLoad(bot, documentId, fullPath);
			listFiles.Add(documentName);  	
		}

		// метод для скачивания приходящей картинки
		async Task DownLoadPhoto(ITelegramBotClient bot, Update update)
		{
			var photoId = update.Message.Photo[3].FileId;       // размер 3 крупный			
			var photoName = $"picture {photoId}";
			var fullPath = Path.Combine(path, photoName);
			await DownLoad(bot, photoId, fullPath);
			listFiles.Add(photoName);      
		}

		// метод для загрузки в указанный путь path приходящего файла по fileId
		static async Task DownLoad(ITelegramBotClient bot, string fileId, string path)
		{
			var file = await bot.GetFileAsync(fileId);
			using var fileStream = new FileStream(path, FileMode.Create);
			await bot.DownloadFileAsync(file.FilePath, fileStream);
		}

		// метод для отправки сообщения некоему пользователю userId
		public void SendMessage(string userId, string messageText)
		{
			if (long.TryParse(userId, out long id))
				bot.SendTextMessageAsync(id, messageText);
			else
			{
				MessageBox.Show(
					"Сообщение не отправлено. Не выбран пользователь, которому отправлять сообщение",
					"Предупреждение",
					MessageBoxButton.OK,
					MessageBoxImage.Warning);
			}
		}
	}
}
