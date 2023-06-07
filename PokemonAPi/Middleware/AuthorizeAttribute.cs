using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using PokemonAPi.Context;
using PokemonAPi.Models;

namespace PokemonAPi.Middleware;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]

public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    private readonly PokemonsContext _context;
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var account = (User)context.HttpContext.Items["User"];
        if (account == null)
        {
            // not logged in
            context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
        }
    }
}