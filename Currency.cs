namespace _10WPF_TelegramBot
{
	class Currency
	{
		public string Name { get; }		// название валюты		
		public decimal Value { get; }	// значение курса валют

		public Currency (string name, decimal value)
		{
			Name = name;			
			Value = value;			
		}
	}
}
