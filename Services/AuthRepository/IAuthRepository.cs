using dotnet_rpg.Models;

namespace dotnet_rpg.Services.AuthRepository;

public interface IAuthRepository
{
    Task<ServiceResponse<int>> Register(User user, string password);
    Task<ServiceResponse<string>> Login(string username, string password);
    Task<bool> UserExists(string username);

    int GetUserId();
}