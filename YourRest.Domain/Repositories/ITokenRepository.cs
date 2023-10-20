using YourRest.Domain.Models;

namespace YourRest.Domain.Repositories;
public interface ITokenRepository
{
    Task<Token> GetTokenAsync(string username, string password);
}