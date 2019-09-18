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
			var res = await httpClient.GetAsync("http://dbservice:5001/api/values");

			var strRes = await res.Content.ReadAsStringAsync();
			var longVal = long.Parse(strRes);

			if (!CurrencyRate.HasValue)
			{
				//this could be done in parallel with the other call and await with Task.WhenAll
				//but that's not the case and you can see it in Kibana.
				var currencyConv = await httpClient.GetAsync(
					"http://data.fixer.io/api/latest?access_key=16f5ceaa4a034ef513991519e51bde03&base=EUR&symbols=USD");

				var strCurrencyRes = await currencyConv.Content.ReadAsStringAsync();

				dynamic jsObj = JsonConvert.DeserializeObject(strCurrencyRes);

				var rate = (double)jsObj.rates.USD;
				CurrencyRate = rate;

				ViewData["retVal"] = $"{longVal} EUR is {rate * longVal} USD";
				CurrencyRate = rate;
			}
			else
			{
				//a primitive caching to create custom spans with the agent API
				//Agent.Tracer.CurrentTransaction.CaptureSpan("ReadCurrencyRate", "Cache", (s) =>
				//{
					//s.Tags["CachedCurrencyRate"] = CurrencyRate.ToString();
					ViewData["retVal"] = $"{longVal} EUR is {CurrencyRate * longVal} USD";
				//});

			}

			return View();
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
