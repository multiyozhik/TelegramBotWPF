using System.Collections.Generic;

namespace _10WPF_TelegramBot
{
	class CurrenciesJson
	{
		// строка Json в виде {"table": "latest", "rates": {"AED": 3.6731, "AFN": 87.023835...}
		public Dictionary<string, decimal> rates { get; set; }	
	}
}
