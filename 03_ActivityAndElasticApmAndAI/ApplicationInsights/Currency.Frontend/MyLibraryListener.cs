using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Currency.Frontend
{
	class MyLibraryObserver : IObserver<KeyValuePair<string, object>>
	{
		private TelemetryClient _telemetryClient;

		public MyLibraryObserver(TelemetryClient telemetryClient) => _telemetryClient = telemetryClient;

		public void OnCompleted() { }

		public void OnError(Exception error) { }

		public void OnNext(KeyValuePair<string, object> kv)
		{
			switch (kv.Key)
			{
				case "CalculateAverage.Start":
					_telemetryClient.TrackEvent("CalculateAverageAsync.Start");
					break;

				case "CalculateAverage.Stop":
					var properties = new Dictionary<string, string>();
					if (Activity.Current != null)
					{
						Console.WriteLine("Tags:");
						foreach (var tag in Activity.Current.Tags)
						{
							properties.Add(tag.Key, tag.Value);
						}
					}

					if (kv.Value.GetType().GetTypeInfo().GetDeclaredProperty("Result").GetValue(kv.Value)
						is double result)
					{
						properties.Add("Result", result.ToString());
					}

					_telemetryClient.TrackEvent("CalculateAverageAsync.Stop", properties);
					break;
				default:
					break;
			}
		}
	}
}
