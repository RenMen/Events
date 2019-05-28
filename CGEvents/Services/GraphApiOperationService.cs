using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Newtonsoft.Json;
using WebApp_OpenIDConnect_DotNet.Infrastructure;

namespace WebApp_OpenIDConnect_DotNet.Services.GraphOperations
{
    public class GraphApiOperationService : IGraphApiOperations
    {
        private readonly GraphServiceClient _client;
       


        public GraphApiOperationService(GraphServiceClient Client)
        {
            _client = Client;
           
        }

        public async Task<dynamic> GetUserInformation()
        {

            var ccemailAddress = new EmailAddress
            {
                Address = "ren.men.in@gmail.com",
            };

            var ccRecipients = new Recipient
            {
                EmailAddress = ccemailAddress,
            };

            var ccRecipientsList = new List<Recipient>
            {
                ccRecipients
            };

            var emailAddress = new EmailAddress
            {
                Address = "rkumar.mr@choueirigroup.com",
            };

            var toRecipients = new Recipient
            {
                EmailAddress = emailAddress,
            };

            var toRecipientsList = new List<Recipient>
            {
                toRecipients
            };

            var body = new ItemBody
            {
                ContentType = BodyType.Text,
                Content = "The new cafeteria is open.",
            };

            var message = new Message
            {
                Subject = "Meet for lunch?",
                Body = body,
                ToRecipients = toRecipientsList,
                CcRecipients = ccRecipientsList,
            };

            Boolean saveToSentItems = true;

            await _client.Me
                .SendMail(message, saveToSentItems)
                .Request()
                .PostAsync();

            return null;
        }
    }
}
