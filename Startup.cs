using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RestaurantWebApp.Models;
using Stripe;

namespace RestaurantWebApp
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		
		public void ConfigureServices(IServiceCollection services)
		{
			// Other service configurations...

			services.Configure<StripeSettings>(Configuration.GetSection("Stripe"));
			services.AddSingleton(Configuration); // Add this line to make Configuration accessible
			services.AddRazorPages(); // Add this line for Razor Pages support
			StripeConfiguration.ApiKey = Configuration["Stripe:StripeSecretKey"];

		}


		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				app.UseHsts();
			}

			// Other app configurations...

			
			app.UseStaticFiles(); // Ensure this line is present to serve static files

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				// Other endpoint configurations...

				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");

				endpoints.MapRazorPages(); // Add this line for Razor Pages support
			});

			// Other app configurations...
		}
	}
}
