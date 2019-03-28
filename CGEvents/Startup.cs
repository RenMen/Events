using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CGEvents.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;
using Microsoft.Extensions.FileProviders;
using System.Reflection;

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
                services.AddAuthentication(AzureADDefaults.AuthenticationScheme)
                .AddAzureAD(options => Configuration.Bind("AzureAd", options));
                //https://docs.telerik.com/aspnet-core/getting-started/getting-started


            //}


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
            app.UsePathBase("/CgEvents");
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
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
