using CGEvents.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json.Serialization;
using System.IO.Compression;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.Client.TokenCacheProviders;
using WebApp_OpenIDConnect_DotNet.Infrastructure;
using WebApp_OpenIDConnect_DotNet.Services;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Server.IIS;

namespace CGEvents
{
    public class Startup
    {
        public IHostingEnvironment Environment { get; }
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            Environment = env;

        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //if (!Environment.IsDevelopment())
            //{

            #region snippet1
            var physicalProvider = Environment.WebRootFileProvider;

            var compositeProvider =
                new CompositeFileProvider(physicalProvider);

            services.AddSingleton<IFileProvider>(compositeProvider);
            #endregion

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddOptions();

            //services.AddAuthentication(AzureADDefaults.AuthenticationScheme)
            //.AddAzureAD(options => Configuration.Bind("AzureAd", options));



            // Token acquisition service based on MSAL.NET
            // and chosen token cache implementation
            //services.AddAzureAdV2Authentication(Configuration)
            //        .AddMsal(new string[] { Constants.ScopeUserRead })
            //        .AddSqlAppTokenCache(new MSALSqlTokenCacheOptions(Configuration.GetConnectionString("TokenCacheDbConnStr")))
            //        .AddSqlPerUserTokenCache(new MSALSqlTokenCacheOptions(Configuration.GetConnectionString("TokenCacheDbConnStr")));

            services.AddAzureAdV2Authentication(Configuration)
                    .AddMsal(new string[] { Constants.ScopeUserRead })
                    .AddInMemoryTokenCaches();


            //services.AddTransient<IClaimsTransformation, ClaimsTransformer>();
            //services.AddAuthentication(IISServerDefaults.AuthenticationScheme);

            // Add Graph
            services.AddGraphService(Configuration);
            //
            var connection = Configuration.GetConnectionString("EventsDB");// @"Server=Marketing2016;Database=MiscForms;Trusted_Connection=True;ConnectRetryCount=0";
            services.AddDbContext<MiscFormsContext>(options => options.UseSqlServer(connection, b => b.UseRowNumberForPaging()));
            services.AddKendo();

            //Refer https://stackoverflow.com/questions/41732254/gzip-in-net-core-not-working
            // Configure Compression level
            services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Fastest);

            // Add Response compression services
            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
                options.EnableForHttps = true;
            });


            services
               .AddMvc(options =>
            {
                if (!Environment.IsDevelopment())
                {

                }
                //options.EnableEndpointRouting = false;
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
            .AddJsonOptions(options =>
            options.SerializerSettings.ContractResolver = new DefaultContractResolver()); ;
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            //AutomaticMigrationsEnabled = true;
            //AutomaticMigrationDataLossAllowed = true;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {

                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //DefaultFilesOptions defaultFilesOptions = new DefaultFilesOptions();

            //defaultFilesOptions.DefaultFileNames.Clear();
            app.UsePathBase("/CGEvent");
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            //   app.UseCookiePolicy();  need to remove 

            //if (!env.IsDevelopment())
            //{
            app.UseAuthentication();
            // Register external authentication middleware
            //}

            app.UseResponseCompression();
            app.UseResponseCompression();
            //https://stackoverflow.com/questions/51107638/asp-net-core-mvc-routing-not-working-in-visual-studio-2017-after-changing-appl

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: null,
                    template: "{controller}/{action}");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
