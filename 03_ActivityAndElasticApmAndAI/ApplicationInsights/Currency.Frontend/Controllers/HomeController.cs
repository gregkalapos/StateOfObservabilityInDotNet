using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Currency.Frontend.Models;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Currency.Frontend.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}

		private static double? CurrencyRate;


		public async Task<IActionResult> Index()
		{
			var httpClient = new HttpClient();
			//var res = await httpClient.GetAsync("http://localhost:5001/api/values");

			//var strRes = await res.Content.ReadAsStringAsync();
			var longVal = 1;// long.Parse(strRes);

			var startStr = DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd");
			var endStr = DateTime.Now.ToString("yyyy-MM-dd");

			var currencyConv = await httpClient.GetAsync(
				$"https://api.exchangeratesapi.io/history?start_at={startStr}&end_at={endStr}&symbols=USD");

			var strCurrencyRes = await currencyConv.Content.ReadAsStringAsync();

			var historicalData = ParseHistoricalValues(strCurrencyRes);
			var avarageRate = new MyLibrary().CalculateAverage(historicalData);

			ViewData["retVal"] = $"{longVal} EUR is {avarageRate * longVal} USD";
			CurrencyRate = avarageRate;

			return View();
		}

		private IEnumerable<double> ParseHistoricalValues(string strCurrencyRes)
		{
			var jsObjRates = JsonConvert.DeserializeObject(strCurrencyRes) as JObject;

			var reader = (jsObjRates).First.CreateReader();

			bool wasLastUsd = false;
			var historicalValues = new List<Double>();
			while (reader.Read())
			{
				if (wasLastUsd)
				{
					var doubleVal = (double)reader.Value;
					historicalValues.Add(doubleVal);
					wasLastUsd = false;
				}
				if (reader.Value is string && (string)reader.Value == "USD")
				{
					wasLastUsd = true;
				}

			}

			return historicalValues;
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
