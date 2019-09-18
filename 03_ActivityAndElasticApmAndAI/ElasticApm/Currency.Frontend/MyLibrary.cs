using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Currency.Frontend
{
	class MyLibrary
	{
		private static readonly DiagnosticSource _diagnosticSource
			= new DiagnosticListener(typeof(MyLibrary).FullName);

		public double CalculateAverage(IEnumerable<double> items)
		{
			var activity = new Activity(nameof(CalculateAverage));

			if (_diagnosticSource.IsEnabled(typeof(MyLibrary).FullName))
			{
				_diagnosticSource.StartActivity(activity, new {Items = items});
			}

			activity.AddTag("NumberOfItemInTag", items.Count().ToString());
			activity.AddBaggage("NumberOfItemInTag", items.Count().ToString());
			var avgValue = items.Average();

			if (_diagnosticSource.IsEnabled(typeof(MyLibrary).FullName))
			{
				_diagnosticSource.StopActivity(activity, new {Items = items, Result = avgValue});
			}

			return avgValue;
		}
	}
}