using Ardalis.GuardClauses;
using eBay.ApiClient.Auth.OAuth2;
using eBay.ApiClient.Auth.OAuth2.Model;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TradeEasy.Global.Contracts.Models.Authentication;

namespace TradeEasy.TokenManagement.Logic.Authentication.EbayAuthenticator;

public class EbayAuthenticator : BaseAuthenticator<EbayOptions>
{
    private readonly IOptions<EbayOptions> _ebayOptions;
    private readonly OAuth2Api _ebayAuthenticator;
    private readonly IHttpClientFactory _httpClientFactory;

    private AccessToken? _token;

    public EbayAuthenticator(
        IOptions<EbayOptions> ebayOptions,
        OAuth2Api ebayAthenticator,
        IHttpClientFactory httpClientFactory)
        : base(
            ebayOptions,
            httpClientFactory)
    {
        _ebayOptions = Guard.Against.Null(ebayOptions);
        _ebayAuthenticator = Guard.Against.Null(ebayAthenticator);
        _httpClientFactory = Guard.Against.Null(httpClientFactory);
    }

    public override async Task<AccessToken?> GenerateAccessToken()
    {
        var _client = _httpClientFactory.CreateClient("Ebay");

        var auth = System.Text.Encoding.UTF8.GetBytes($"{_ebayOptions.Value.AppId}:{_ebayOptions.Value.ClientSecret}");

        if (_token is null)
        {
            _client.DefaultRequestHeaders.Add(
                "Authorization",
                $"Basic {Convert.ToBase64String(auth)}");

            _client.DefaultRequestHeaders.Add(
                "ContentType",
                "application/x-www-form-urlencoded");

            var formContent = new FormUrlEncodedContent(
                new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("grant_type", "client_credentials"),
                   // new KeyValuePair<string, string>("scopes", "")
                });


            var response = await _client.PostAsync("identity/v1/oauth2/token", formContent);

            if (response.IsSuccessStatusCode)
            {
                var accessToken = await response.Content.ReadAsStringAsync();

                _token = JsonConvert.DeserializeObject<AccessToken>(accessToken);
            }
        }

        return _token;
    }
}
