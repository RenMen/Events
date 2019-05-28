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
    public class ProfilePhoto : ViewComponent
    {
        readonly ITokenAcquisition tokenAcquisition;
        readonly WebOptions webOptions;
        readonly IHostingEnvironment env = null;

        public ProfilePhoto(ITokenAcquisition tokenAcquisition,
                      IOptions<WebOptions> webOptionValue, IHostingEnvironment env)
        {
            this.tokenAcquisition = tokenAcquisition;
            this.webOptions = webOptionValue.Value;
            this.env = env;
        }

        [MsalUiRequiredExceptionFilter(Scopes = new[] { Constants.ScopeUserRead })]
        public async Task<IViewComponentResult> InvokeAsync()
        {
            Graph::GraphServiceClient graphClient = GetGraphServiceClient(new[] { Constants.ScopeUserRead });
            try
            {
                // Get user photo
                var photoStream = await graphClient.Me.Photo.Content.Request().GetAsync();
                byte[] photoByte = ((MemoryStream)photoStream).ToArray();
                ViewData["Photo"]= Convert.ToBase64String(photoByte);
            }
            catch(System.Exception ex)
            {

            }

            return View();
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
