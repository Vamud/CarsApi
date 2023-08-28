using CarsApi.Services;
using CarsApi.Services.Interfaces;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace CarsApi
{
	public class Startup
	{
		private readonly IWebHostEnvironment _env;
		private readonly IConfiguration _config;

		public Startup(IWebHostEnvironment webHostEnvironment, IConfiguration config)
		{
			_env = webHostEnvironment ?? throw new ArgumentNullException(nameof(webHostEnvironment));
			_config = config ?? throw new ArgumentNullException(nameof(config));
		}

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllers();

            services.AddCors(options =>
            {

                options.AddPolicy("AllowDashboardOrigin",
                             builder => builder.SetIsOriginAllowed((host) => true).AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            });

            services.AddTransient<IFakeDataService, FakeDataService>();
			services.AddTransient<IFilterService, FilterService>();

            services.AddUmbraco(_env, _config)
				.AddBackOffice()
				.AddWebsite()
				.AddDeliveryApi()
				.AddComposers()
				.Build();
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

            app.UseCors("AllowDashboardOrigin");

            app.UseUmbraco()
				.WithMiddleware(u =>
				{
					u.UseBackOffice();
					u.UseWebsite();
				})
				.WithEndpoints(u =>
				{
					u.UseInstallerEndpoints();
					u.UseBackOfficeEndpoints();
					u.UseWebsiteEndpoints();
				});

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});

        }
	}
}
