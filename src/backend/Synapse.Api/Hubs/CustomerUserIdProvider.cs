using Microsoft.AspNetCore.SignalR;
using System.Linq;
using System.Security.Claims;

namespace Synapse.Api.Hubs;
public class CustomerUserIdProvider : IUserIdProvider
{
    public string? GetUserId(HubConnectionContext connection)
    {
        return connection.User?
            .FindFirst(ClaimTypes.NameIdentifier)?
            .Value;
    }
}