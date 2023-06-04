using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TradeEasy.Global.Contracts.Models.Authentication;
using TradeEasy.TokenManagement.Logic.Authentication;
using Ardalis.GuardClauses;

namespace TokenManagement
{
    public class GetEbayTokenFunction
    {
        private readonly BaseAuthenticator<EbayOptions> _authenticator;

        public GetEbayTokenFunction(BaseAuthenticator<EbayOptions> baseAuthenticator)
        {
                _authenticator = baseAuthenticator;
        }

        [FunctionName("GetEbayTokenFunction")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "tokens/{userId}/get-token")] HttpRequest req,
            string userId,
            ILogger log)
        {
            Guard.Against.NullOrEmpty(userId);

            var token = await _authenticator.GenerateAccessToken();

            return new OkObjectResult(token);
        }
    }
}
