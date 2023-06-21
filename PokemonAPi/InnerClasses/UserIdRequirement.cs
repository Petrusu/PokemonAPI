using Microsoft.AspNetCore.Authorization;

namespace PokemonAPi;

public class UserIdRequirement :IAuthorizationRequirement
{
    public int UserId { get; }

    public UserIdRequirement(int userId)
    {
        UserId = userId;
    }
}