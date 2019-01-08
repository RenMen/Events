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
            if (!Environment.IsDevelopment())
            {

                services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            }
            //https://docs.telerik.com/aspnet-core/getting-started/getting-started
            if (!Environment.IsDevelopment())
            { 
                services.AddAuthentication(AzureADDefaults.AuthenticationScheme)
                .AddAzureAD(options => Configuration.Bind("AzureAd", options));
            }
            //
            var connection = @"Server=Marketing2016;Database=MiscForms;Trusted_Connection=True;ConnectRetryCount=0";
            services.AddDbContext<MiscFormsContext>(options => options.UseSqlServer(connection, b => b.UseRowNumberForPaging()));
            services.AddKendo();

            services                
               .AddMvc(options =>
            {
                if (!Environment.IsDevelopment())
                {
                    //options.EnableEndpointRouting = false;
                    var policy = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .Build();
                    options.Filters.Add(new AuthorizeFilter(policy));
                }
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
            .AddJsonOptions(options =>
            options.SerializerSettings.ContractResolver = new DefaultContractResolver()); ;
        }

        
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
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

            app.UseHttpsRedirection();
            app.UseStaticFiles();
         //   app.UseCookiePolicy();  need to remove 

            if (!env.IsDevelopment())
            {
                app.UseAuthentication();
                // Register external authentication middleware
            }
           
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
