using Microsoft.AspNetCore.SignalR;
using System.Linq;
using System.Security.Claims;

public class CustomerUserIdProvider : IUserIdProvider
{
    public string GetUserId(HubConnectionContext context)
    {
        // Extract the user ID from the connection context
        // var userIdClaim = context.User?.Claims.FirstOrDefault(c => c.Type == "sub");
        // return userIdClaim?.Value ?? string.Empty;

        return context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
    }
}