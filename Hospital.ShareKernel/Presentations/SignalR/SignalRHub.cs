using System.Collections.Concurrent;
using System.Security.Claims;
using Hospital.SharedKernel.Application.Consts;
using Hospital.SharedKernel.Application.Enums;
using Hospital.SharedKernel.Infrastructure.Caching.Models;
using Hospital.SharedKernel.Libraries.Utils;
using Hospital.SharedKernel.Presentations.SignalR.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Net.Http.Headers;

namespace Hospital.SharedKernel.Presentations.SignalR
{
    public class SignalRHub : Hub
    {
        /// <summary>
        /// Key: User Id
        /// Value: Connections
        /// </summary>
        public static ConcurrentDictionary<string, List<AuthenticatedSocketConnection>> EmployeeConnections = new();

        /// <summary>
        /// Key: User Id
        /// Value: Connections
        /// </summary>
        public static ConcurrentDictionary<string, List<AuthenticatedSocketConnection>> CustomerConnections = new();

        /// <summary>
        /// Key: Connection Id
        /// Value: Connections
        /// </summary>
        public static ConcurrentDictionary<string, AnonymousSocketConnection> AnonymousConnections = new();

        private readonly IHttpContextAccessor _accessor;

        public SignalRHub(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        [HubMethodName("OnlineUsers")]
        public async Task SendNumberOfOnlineUsers()
        {
            await Clients.All.SendAsync("ReceiveMessage", new SignalRMessage
            {
                Type = 0,
                Data = GetOnlineModel()
            });
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();

            var request = _accessor.HttpContext.Request;
            var uid = request.Headers[HeaderNamesExtension.Uid].ToString();

            if (!IsAuthenticatedConnection(out var claims))
            {
                var currentConnection = new AnonymousSocketConnection
                {
                    AccessDate = DateTime.Now,
                    Ip = AuthUtility.TryGetIP(request),
                    ConnectionId = Context.ConnectionId,
                    UserAgent = request.Headers[HeaderNames.UserAgent].ToString(),
                    Type = AccountType.Anonymous,
                    Uid = uid
                };

                AnonymousConnections.TryAdd(Context.ConnectionId, currentConnection);
            }
            else
            {
                var userId = claims.First(x => x.Type == ClaimConstant.USER_ID).Value;
                var email = claims.First(x => x.Type == ClaimValueTypes.Email).Value;
                var fullname = claims.First(x => x.Type == ClaimConstant.FULL_NAME).Value;
                var facilityId = claims.First(x => x.Type == ClaimConstant.FACILITY_ID).Value;
                var zoneId = claims.First(x => x.Type == ClaimConstant.ZONE_ID).Value;
                var type = (AccountType)int.Parse(claims.First(x => x.Type == ClaimConstant.ACCOUNT_TYPE).Value);
                var key = CacheManager.GetConnectionSocketKey(long.Parse(userId));

                var currentConnection = new AuthenticatedSocketConnection
                {
                    ConnectionId = Context.ConnectionId,
                    AccessDate = DateTime.Now,
                    UserId = userId,
                    Email = email,
                    Fullname = fullname,
                    Ip = AuthUtility.TryGetIP(request),
                    UserAgent = request.Headers[HeaderNames.UserAgent].ToString(),
                    Type = type,
                    Uid = uid,
                    FacilityId = long.Parse(facilityId),
                    ZoneId = long.Parse(zoneId)
                };
                switch (type)
                {
                    case AccountType.Customer:
                        AddConnection(CustomerConnections);
                        break;
                    case AccountType.Employee:
                        AddConnection(EmployeeConnections);
                        break;
                    default:
                        break;
                }

                void AddConnection(ConcurrentDictionary<string, List<AuthenticatedSocketConnection>> dict)
                {
                    if (dict.TryGetValue(key, out var connections))
                    {
                        connections.Add(currentConnection);
                        dict[key] = connections;
                    }
                    else
                    {
                        dict[key] = new List<AuthenticatedSocketConnection>
                        {
                            currentConnection
                        };
                    }
                }
            }

            await SendNumberOfOnlineUsers();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);

            if (!IsAuthenticatedConnection(out var claims))
            {
                AnonymousConnections.Remove(Context.ConnectionId, out var v);
            }
            else
            {
                var userId = claims.First(x => x.Type == ClaimConstant.USER_ID).Value;
                var type = (AccountType)int.Parse(claims.First(x => x.Type == ClaimConstant.ACCOUNT_TYPE).Value);
                var key = CacheManager.GetConnectionSocketKey(long.Parse(userId));

                switch (type)
                {
                    case AccountType.Customer:
                        RemoveConnection(CustomerConnections);
                        break;

                    case AccountType.Employee:
                        RemoveConnection(EmployeeConnections);
                        break;

                    default:
                        break;
                }


                void RemoveConnection(ConcurrentDictionary<string, List<AuthenticatedSocketConnection>> dict)
                {
                    var connections = dict[key];
                    var connection = connections.First(x => x.ConnectionId == Context.ConnectionId);

                    connections.Remove(connection);

                    if (!connections.Any())
                    {
                        dict.Remove(key, out var _);
                    }
                }
            }
            await SendNumberOfOnlineUsers();
        }

        private bool IsAuthenticatedConnection(out IEnumerable<Claim> claims)
        {
            claims = (Context.User.Identity as ClaimsIdentity).Claims;
            return claims != null && claims.Any();
        }

        private static OnlineModel GetOnlineModel() => new()
        {
            EmployeeCount = EmployeeConnections.Values.Select(list => list.GroupBy(x => x.Uid).Count()).Count(),
            CustomerCount = CustomerConnections.Values.Select(list => list.GroupBy(x => x.Uid).Count()).Count(),
            AnonymousCount = AnonymousConnections.Values.GroupBy(x => x.Uid).Count()
        };
    }
}
