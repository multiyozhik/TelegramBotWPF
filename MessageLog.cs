namespace _10WPF_TelegramBot
{
	class MessageLog
	{
		public long Id { get; set; }
		public string FirstName { get; set; }
		public string Date { get; set; }
		public string MessageText { get; set; }
		public MessageLog(long id, string firstName, string Date, string messageText)
		{
			this.Id = id;
			this.FirstName = firstName;
			this.Date = Date;
			this.MessageText = messageText;
		}
	}
}
