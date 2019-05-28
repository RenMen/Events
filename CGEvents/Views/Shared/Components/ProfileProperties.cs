using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Graph = Microsoft.Graph;
using Microsoft.Identity.Web.Client;
using WebApp_OpenIDConnect_DotNet.Infrastructure;
using WebApp_OpenIDConnect_DotNet.Services;
using Microsoft.AspNetCore.Hosting;

namespace CGEvents.Views.Shared.Components
{
    [Authorize]
    public class ProfileProperties:ViewComponent
    {
        readonly ITokenAcquisition tokenAcquisition;
        readonly WebOptions webOptions;
        readonly IHostingEnvironment env = null;
        public ProfileProperties(ITokenAcquisition tokenAcquisition,
              IOptions<WebOptions> webOptionValue, IHostingEnvironment env)
        {
            this.tokenAcquisition = tokenAcquisition;
            this.webOptions = webOptionValue.Value;
            this.env = env;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var prop = await GetProfileProperties();

            if (prop != null)
            {
                ViewData["Me"] = prop;
            }
            else
            {
                ViewData["Me"] = null;
            }

            return View();
        }
        public class Usr
        {
            public string Name { get; set; }
            public string Email { get; set; }
        }

        [MsalUiRequiredExceptionFilter(Scopes = new[] { Constants.ScopeUserRead })]
        private async Task<Usr> GetProfileProperties()
        {
            Graph::GraphServiceClient graphClient = GetGraphServiceClient(new[] { Constants.ScopeUserRead });
            try
            {
                // Get users properties
             var me= await graphClient.Me.Request().Select(e => new  {
                   e.DisplayName,
                    e.Mail
                })
                .GetAsync();

                return new Usr { Email = me.Mail, Name = me.DisplayName };
              
            }
            catch (System.Exception ex)
            {
                return null;
            }
        }

        private Graph::GraphServiceClient GetGraphServiceClient(string[] scopes)
        {

            return GraphServiceClientFactory.GetAuthenticatedGraphClient(async () =>
            {
                string result = await tokenAcquisition.GetAccessTokenOnBehalfOfUser(
                       HttpContext, scopes);
                return result;
            }, webOptions.GraphApiUrl);
        }
    }
}

