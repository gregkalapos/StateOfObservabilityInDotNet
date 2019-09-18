using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Elastic.Apm;
using Elastic.Apm.Api;

namespace Currency.Frontend
{
	class MyLibraryObserver : IObserver<KeyValuePair<string, object>>
	{
		readonly ConcurrentDictionary<string, ISpan> _concurrentDictionary = new ConcurrentDictionary<string, ISpan>();

		public void OnError(Exception error)
		{
		}
		
		public void OnCompleted()
		{
		}

		public void OnNext(KeyValuePair<string, object> kv)
		{
			switch (kv.Key)
			{
				case "CalculateAverage.Start":
					var transaction = Agent.Tracer.CurrentTransaction;
					if (transaction != null && Activity.Current.Id != null)
					{
						var span = transaction.StartSpan("CalculateAverage", "MyLib");

						_concurrentDictionary.TryAdd(Activity.Current.Id, span);
					}

					break;

				case "CalculateAverage.Stop":

					if (Activity.Current != null &&
					    _concurrentDictionary.Remove(Activity.Current.Id, out ISpan currentSpan))
					{
						if (kv.Value.GetType().GetTypeInfo().GetDeclaredProperty("Result").GetValue(kv.Value)
							is double result)
						{
							currentSpan.Labels["Result"] = result.ToString();
						}

						foreach (var tag in Activity.Current.Tags)
						{
							currentSpan.Labels[$"tag - {tag.Key}"] = tag.Value;
						}

						currentSpan.End();
					}

					break;
				default:
					break;
			}
		}
	}
}