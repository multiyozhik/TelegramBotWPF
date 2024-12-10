namespace _10WPF_TelegramBot
{
	/// <summary>
	/// Класс валюты со свойствами (название и значение курса, руб.)
	/// </summary>
	class Currency
	{
		public string Name { get; }		
		public decimal Value { get; }

		public Currency (string name, decimal value)
		{
			Name = name;			
			Value = value;			
		}
	}
}
