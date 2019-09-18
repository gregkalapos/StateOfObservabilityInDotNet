using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Currency.Frontend
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddApplicationInsightsTelemetry();

			services.AddControllersWithViews();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, TelemetryClient telemetryClient)
		{
			DiagnosticListener.AllListeners.Subscribe(new Subscriber(telemetryClient));

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
			}
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");
			});
		}

		class Subscriber : IObserver<DiagnosticListener>
		{
			TelemetryClient _telemetryClient;
			public Subscriber(TelemetryClient telemetryClient) => _telemetryClient = telemetryClient;

			public void OnCompleted() { }

			public void OnError(Exception error) { }

			public void OnNext(DiagnosticListener listener)
			{
				if (listener.Name == typeof(MyLibrary).FullName)
				{
					listener.Subscribe(new MyLibraryObserver(_telemetryClient));
				}
			}
		}
	}
}
