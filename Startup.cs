using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Refit;
using eBay.ApiClient.Auth.OAuth2;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using TradeEasy.TokenManagement.Logic.Authentication;
using TradeEasy.TokenManagement.Logic.Authentication.EbayAuthenticator;
using TradeEasy.Global.Contracts.Models.Authentication;

[assembly: FunctionsStartup(typeof(TradeEasy.Api.Functions.Startup))]


namespace TradeEasy.Api.Functions
{
    public class Startup : FunctionsStartup
    {
        public Startup()
        {

        }

        public override void Configure(IFunctionsHostBuilder builder)
        {
            var configuration = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("local.settings.json", true, true)
                 .Build();

            EbayOptions sec = configuration.GetSection("EbayOptions").Get<EbayOptions>();

            builder.Services.Configure<EbayOptions>(options => configuration.GetSection("EbayOptions").Bind(options));


            EbayOptions ebayOptions = configuration.Get<EbayOptions>();

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddSingleton<OAuth2Api>();
            builder.Services.AddSingleton<BaseAuthenticator<BaseOptions>>();
            builder.Services.AddSingleton<BaseAuthenticator<EbayOptions>, EbayAuthenticator>();




            builder.Services.AddHttpClient("Ebay", httpClient =>
            {
                httpClient.BaseAddress = new Uri(sec.EbayAuthUrl);

            });

            builder.Services.AddLogging();
        }
    }
}
