using System;
using System.Collections.Generic;
using System.Threading;
using OpenTelemetry.Exporter.Jaeger;
using OpenTelemetry.Trace;
using OpenTelemetry.Trace.Config;
using OpenTelemetry.Trace.Sampler;

namespace OpenTelemetryBasicSample
{
	class Program
	{
		static void Main(string[] args)
		{
			// 1 and 2. - configure the exporeter
			ConsfigExporter();

			// 3. Tracer is global singleton. You can register it via dependency injection if it exists
			// but if not - you can use it as follows:
			var tracer = Tracing.Tracer;

			// 4. Create a scoped span. It will end automatically when using statement ends
			using (tracer.WithSpan(tracer.SpanBuilder("Main").StartSpan()))
			{
				tracer.CurrentSpan.SetAttribute("custom-attribute", 55);
				Console.WriteLine("About to do a busy work");
				for (int i = 0; i < 10; i++)
				{
					DoWork(i);
				}
			}

			// 5. Gracefully shutdown the exporter so it'll flush queued traces to Jaeger.
			Tracing.SpanExporter.Dispose();
			
			Console.WriteLine("Press any key to exit!");
			Console.ReadKey();
		}
		
		private static void DoWork(int i)
		{
			// 6. Get the global singleton Tracer object
			var tracer = Tracing.Tracer;

			// 7. Start another span. If another span was already started, it'll use that span as the parent span.
			// In this example, the main method already started a span, so that'll be the parent span, and this will be
			// a child span.
			using (tracer.WithSpan(tracer.SpanBuilder("DoWork").StartSpan()))
			{
				// Simulate some work.
				var span = tracer.CurrentSpan;

				try
				{
					Console.WriteLine("Doing busy work");
					Thread.Sleep(1000);
				}
				catch (ArgumentOutOfRangeException e)
				{
					// 6. Set status upon error
					span.Status = Status.Internal.WithDescription(e.ToString());
				}

				// 7. Annotate our span to capture metadata about our operation
				var attributes = new Dictionary<string, object>();
				attributes.Add("use", "demo");
				span.AddEvent("Invoking DoWork", attributes);
			}
		}

		private static void ConsfigExporter()
		{
			// 1. Configure exporter to export traces to Jaeger
			var exporter = new JaegerExporter(
				new JaegerExporterOptions
				{
					ServiceName = "OpenTelemetrySample",
					AgentHost = "localhost",
					AgentPort = 5775,
				},
				Tracing.SpanExporter);
                
			exporter.Start();

			// 2. Configure 100% sample rate for the purposes of the demo
			ITraceConfig traceConfig = Tracing.TraceConfig;
			ITraceParams currentConfig = traceConfig.ActiveTraceParams;
			var newConfig = currentConfig.ToBuilder()
				.SetSampler(Samplers.AlwaysSample)
				.Build();
			traceConfig.UpdateActiveTraceParams(newConfig);
		}

	}
}