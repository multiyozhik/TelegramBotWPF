using System.Collections.Generic;

namespace _10WPF_TelegramBot
{
    /// <summary>
    /// Классы для десериализации в объект Valute строки данных с сайта ЦБ РФ https://www.cbr-xml-daily.ru/daily_json.js
    /// Класс ValuteDTO для таблицы окна курса валют (наименование и значение курса, руб.)
    /// </summary>

    class CurrenciesDTO       //DTO = data transfer object
    {
		public Dictionary<string, ValuteDTO> Valute { get; set; }	
	}

    class ValuteDTO
    {
        public string Name { get; set; }
        public decimal Value { get; set; }
    }
}

/* структура данных на сайте
  {
    "Date": "2024-12-07T11:30:00+03:00",
      "PreviousDate": "2024-12-06T11:30:00+03:00",
      "PreviousURL": "\/\/www.cbr-xml-daily.ru\/archive\/2024\/12\/06\/daily_json.js",
      "Timestamp": "2024-12-09T14:00:00+03:00",
      "Valute": {
        "EUR": {
            "ID": "R01239",
          "NumCode": "978",
          "CharCode": "EUR",
          "Nominal": 1,
          "Name": "Евро",
          "Value": 106.304,
          "Previous": 109.7802
        },..}
*/