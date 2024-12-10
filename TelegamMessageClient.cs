using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace _10WPF_TelegramBot
{
    /// <summary>
    /// Класс для взаимодействия с встроенным TelegramBotClient-классом с применением его методов API и 
	/// формированием ObservableCollection списка сообщений и списка имен скачен. файлов (сохр. в систем. папку с документ.)
	/// Мой бот работает по ссылке t.me/print_current_currency_bot, 
	/// строка токена, сгенерированная https://telegram.me/BotFather, должна быть в "Token.txt" в bin-папке проекта)
	/// </summary>
    class TelegamMessageClient
	{
		public TelegramBotClient bot;
		public ObservableCollection<MessageLog> BotMessageLog { get; set; } = new();
        public ObservableCollection<string> ListFiles { get; set; } = new ();

        static readonly string path = Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments);
				
		static readonly Object SyncObject = new();


        /// <summary>
        /// В конструкторе по токену инициализируется бот и запускается (с передачей метода обработки обновлений и др.).
		/// С пом. Object SyncObject осуществл. синхронизация изм. в коллекциях (при неск. польз-лях бота)
        /// </summary>
        /// <param name="pathToken">Строка токена</param>
        public TelegamMessageClient(string pathToken = "Token.txt")
		{
			string token = System.IO.File.ReadAllText(pathToken);
			bot = new TelegramBotClient(token);

			using var cts = new CancellationTokenSource();
			var receiverOptions = new ReceiverOptions
			{
				AllowedUpdates = { }
			};

			System.Windows.Data.BindingOperations.EnableCollectionSynchronization(ListFiles, SyncObject);
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
                        DateTime.Now.ToString(), 
						update.Message.Text);
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
			ListFiles.Add(audioName);			
		}	

		// метод для скачивания приходящего документа
		async Task DownLoadDocument(ITelegramBotClient bot, Update update)
		{
			var documentName = update.Message.Document.FileName;
			var documentId = update.Message.Document.FileId;
			var fullPath = Path.Combine(path, documentName);
			
			await DownLoad(bot, documentId, fullPath);
			ListFiles.Add(documentName);  	
		}

		// метод для скачивания приходящей картинки
		async Task DownLoadPhoto(ITelegramBotClient bot, Update update)
		{
			var photoId = update.Message.Photo[2].FileId;       // размер крупный			
			var photoName = $"picture {photoId}";
			var fullPath = Path.Combine(path, photoName);
			await DownLoad(bot, photoId, fullPath);
			ListFiles.Add(photoName);      
		}

		// метод для загрузки в указанный путь path приходящего файла по fileId
		static async Task DownLoad(ITelegramBotClient bot, string fileId, string path)
		{
			var file = await bot.GetFileAsync(fileId);
			using var fileStream = new FileStream(path, FileMode.Create);
			await bot.DownloadFileAsync(file.FilePath, fileStream);
		}

		// метод для отправки сообщения некоему пользователю userId
		public void SendMessage(string userId, string firstNameSelectedMessageLog, string messageText)
		{
                bot.SendTextMessageAsync(userId, messageText);
                BotMessageLog.Add(new MessageLog(
                    long.Parse(userId),
                    firstNameSelectedMessageLog,
                    DateTime.Now.ToString(),
                    messageText));
		}
	}
}
