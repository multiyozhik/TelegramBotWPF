using System.Collections.Generic;
using System.Net;
using System.Text.Json;

namespace _10WPF_TelegramBot
{
	static class GettingCurrencyValues
	{
		record CurrencyInfo(string Code, string Name);
		static readonly CurrencyInfo[] currencies = new CurrencyInfo[] 
		{
			new ("EUR","Евро"), 
			new ("USD","Доллар США"),
			new ("GBP","Фунт стерлингов Великобритании"),
			new ("JPY", "Японская йена"),
			new ("BYN", "Белорусский рубль"),
			new ("CNY", "Юань"),
			new ("HRK", "Хорватская куна")
		};
		
		static public List<Currency> GetCurrencyValues()
		{			
			var urlRecourceCurrencies = $@"https://cdn.cur.su/api/latest.json";
			var webClient = new WebClient();
			var jsonString = webClient.DownloadString(urlRecourceCurrencies);
			var currenciesJson = JsonSerializer.Deserialize<CurrenciesJson>(jsonString);
			var ratesDictionary = currenciesJson.rates; 														// 			
			var rateRUB = ratesDictionary["RUB"];
			var currenciesDataGrid = new List<Currency>();
			foreach (var item in currencies)
			{			
				// выполняется конвертация для вывода курса валюты в рублях
				var newCurrencyItem = new Currency(item.Name, rateRUB / ratesDictionary[item.Code]);
				currenciesDataGrid.Add(newCurrencyItem);

			}			
			return currenciesDataGrid;
		}
	}
}
