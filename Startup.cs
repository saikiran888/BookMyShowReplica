using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using BookMyShowReplica4.Data;
using BookMyShowReplica4.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BookMyShowReplica4.Services;
using System.Data.SqlClient;
using PetaPoco;
using SimpleInjector;
using SimpleInjector.Lifestyles;

namespace BookMyShowReplica4
{
	public class Startup
	{
		private Container container;
		public IHttpContextAccessor HttpContextAccessor { get; }
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
			container = new Container();
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddDbContext<ApplicationDbContext>(options =>
			    options.UseSqlServer(
				Configuration.GetConnectionString("ApplicationDbContextConnection")));

			services.AddDatabaseDeveloperPageExceptionFilter();

			services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
			    .AddEntityFrameworkStores<ApplicationDbContext>();

			services.AddIdentityServer()
			    .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();

			services.AddAuthentication()
			    .AddIdentityServerJwt();
			services.AddControllersWithViews();

			services.AddLogging();
			services.AddLocalization(options => options.ResourcesPath = "Resources");
			services.AddSimpleInjector(container, options =>
			{
				// AddAspNetCore() wraps web requests in a Simple Injector scope and
				// allows request-scoped framework services to be resolved.
				options.AddAspNetCore()

				    // Ensure activation of a specific framework type to be created by
				    // Simple Injector instead of the built-in configuration system.
				    // All calls are optional. You can enable what you need. For instance,
				    // ViewComponents, PageModels, and TagHelpers are not needed when you
				    // build a Web API.
				    .AddControllerActivation()
				    .AddViewComponentActivation()
				    .AddPageModelActivation()
				    .AddTagHelperActivation();

				// Optionally, allow application components to depend on the non-generic
				// ILogger (Microsoft.Extensions.Logging) or IStringLocalizer
				// (Microsoft.Extensions.Localization) abstractions.
				options.AddLogging();
				options.AddLocalization();
			});
			services.AddControllersWithViews();
			services.AddRazorPages();
			// In production, the Angular files will be served from this directory
			services.AddSpaStaticFiles(configuration =>
			{
				configuration.RootPath = "ClientApp/dist";
			});
			InitializeContainer();
		}
		private void InitializeContainer()
		{
			var connectionString = Configuration.GetConnectionString("ApplicationDbContextConnection");
			var connection = new SqlConnection(connectionString);
			connection.Open();
			container.Register<IHttpContextAccessor, HttpContextAccessor>();
			container.Register<SqlConnection>(() => new SqlConnection(connectionString), Lifestyle.Singleton);
			container.Register<Database>(() => new Database(connection), Lifestyle.Singleton);
			container.Register<IUserServices, UserServices>();
			container.Register<IMovieServices, MovieServices>();

		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			app.UseSimpleInjector(this.container);
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseMigrationsEndPoint();
			}
			else
			{
				app.UseExceptionHandler("/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();
			if (!env.IsDevelopment())
			{
				app.UseSpaStaticFiles();
			}


			app.UseRouting();

			app.UseAuthentication();
			app.UseIdentityServer();
			app.UseAuthorization();
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
			name: "default",
			pattern: "{controller}/{action=Index}/{id?}");
				endpoints.MapRazorPages();
			});

			app.UseSpa(spa =>
			{
				// To learn more about options for serving an Angular SPA from ASP.NET Core,
				// see https://go.microsoft.com/fwlink/?linkid=864501

				spa.Options.SourcePath = "ClientApp";

				if (env.IsDevelopment())
				{
					spa.UseAngularCliServer(npmScript: "start");
				}
			});

			container.Verify();
		}
	}
}
