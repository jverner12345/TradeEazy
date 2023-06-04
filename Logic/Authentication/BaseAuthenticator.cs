using Ardalis.GuardClauses;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TradeEasy.Global.Contracts.Models.Authentication;

namespace TradeEasy.TokenManagement.Logic.Authentication;

public class BaseAuthenticator<T> : IAuthenticator where T : BaseOptions
{
    protected readonly IHttpClientFactory _httpClientFactory;
    protected readonly IOptions<T> _options;

    public BaseAuthenticator(
        IOptions<T> options,
        IHttpClientFactory httpClientFactory)
    {
        _options = Guard.Against.Null(options);
        _httpClientFactory = Guard.Against.Null(httpClientFactory);
    }

    public virtual async Task<AccessToken?> GenerateAccessToken()
    {
        throw new NotImplementedException();
    }

    public virtual Task<AccessToken> RefreshToken()
    {
        throw new NotImplementedException();
    }
}
