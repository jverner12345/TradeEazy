using System.Threading.Tasks;
using TradeEasy.Global.Contracts.Models.Authentication;

namespace TradeEasy.TokenManagement.Logic.Authentication;

public interface IAuthenticator
{
    Task<AccessToken?> GenerateAccessToken();

    Task<AccessToken> RefreshToken();
}
