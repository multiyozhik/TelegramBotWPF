using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;

namespace _10WPF_TelegramBot
{
    /// <summary>
    /// Статический класс для получ. из сайта ЦБ РФ курса валют 
    /// Метод возвр. List<Currency> (наименование и значение, руб.)
    /// </summary>
    static class GettingCurrencyValues
	{		
		static public List<Currency> GetCurrencyValues()
		{
            var urlRecourceCurrencies = $@"https://www.cbr-xml-daily.ru/daily_json.js";
			var jsonString = new WebClient().DownloadString(urlRecourceCurrencies);
			var currenciesDTO = JsonSerializer.Deserialize<CurrenciesDTO>(jsonString);
            return currenciesDTO
				.Valute.Values
				.Select(item => new Currency(item.Name, item.Value))
				.ToList();
            //у Dictionary Valute берем значения словаря Values и по ним (объекты типа ValuteDTO) пробегаем
        }
    }
}
