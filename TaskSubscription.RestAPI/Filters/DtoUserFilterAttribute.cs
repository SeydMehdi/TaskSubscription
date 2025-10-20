using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Payment.Infrastructure.Utilities.Mapping;
using System.IdentityModel.Tokens.Jwt;

namespace Payment.API.Filters;
/// <summary>
/// Get dto set username and userid for dto set them to propeprties for checking user specially
/// in permission checking nad logging for logging events.
/// </summary>
public class DtoUserFilterAttribute : ActionFilterAttribute
{
    private ILogger<DtoUserFilterAttribute> logger;
    public DtoUserFilterAttribute(ILogger<DtoUserFilterAttribute> logger)
    {
        this.logger = logger;
    }
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        base.OnActionExecuting(context);
        string currentIp = context.HttpContext.Request.Host.Value;
        // Retrieve the token from the request headers
        checkAuthorization(context, currentIp);
    }

    private bool checkAuthorization(ActionExecutingContext context, string currentIp)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue("Authorization", out var token))
        {
            setResultError(context, $"IP: {currentIp} - token is not set in Authorization headers");
            return true;
        }
        var tokenValue = token.FirstOrDefault()?.Split(" ").Last(); // Extract the token value
        if (string.IsNullOrEmpty(tokenValue))
        {
            setResultError(context, $"IP: {currentIp} -Authorization is exist bu token is empty");
            return true;
        }
        string loginedUserId = getUserId(tokenValue);
        if (string.IsNullOrEmpty(loginedUserId))
        {
            setResultError(context, $"IP: {currentIp} - token exist but does  not contains Id");
            return true;
        }
        if (!Guid.TryParse(loginedUserId, out var userId))
        {
            setResultError(context, $"IP: {currentIp} - token contains id=\"{loginedUserId}\" is exist but not guid. login failed");
            return true;
        }
        // You can now use the user ID as needed
        setUserIdInDto(context, userId, currentIp);
        return false;
    }

    private void setResultError(ActionExecutingContext context, string message)
    {
        logger.LogError($"{nameof(DtoUserFilterAttribute)} {message}");
        context.Result = new UnauthorizedResult(); // Return unauthorized if user ID is not found
    }

    private static string getUserId(string tokenValue)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(tokenValue); // Read the JWT token
                                                         // Extract the user ID from claims
        var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "id"); // "id" is the claim type for user ID
        var loginedUserId = userIdClaim?.Value;
        return loginedUserId;
    }

    private static void setUserIdInDto(ActionExecutingContext context, Guid userId, string ipAddress)
    {
        foreach (var item in context.ActionArguments)
        {
            if (item.Value is IBaseDto dto)
            {
                var identity = context.HttpContext.User.Identity;
                if (identity != null)
                    dto.Username = identity.Name;
            }
        }
    }
}