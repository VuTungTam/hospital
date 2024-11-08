using Hospital.SharedKernel.Application.Consts;
using Hospital.SharedKernel.Application.Enums;
using Hospital.SharedKernel.Libraries.Utils;
using Hospital.SharedKernel.SignalR.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Net.Http.Headers;
using System.Collections.Concurrent;
using System.Security.Claims;

namespace Hospital.SharedKernel.SignalR
{
    public class SignalRHub : Hub
    {
        public static ConcurrentDictionary<string, List<AuthenticatedAccessInfo>> EmployeeConnections = new();

        public static ConcurrentDictionary<string, List<AuthenticatedAccessInfo>> CustomerConnections = new();

        public static ConcurrentDictionary<string, AnonymousAccessInfo> AnonymousConnections = new();

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
                Data = new OnlineModel
                {
                    EmployeeCount = EmployeeConnections.SelectMany(x => x.Value).Count(),
                    CustomerCount = CustomerConnections.SelectMany(x => x.Value).Count(),
                    AnonymousCount = AnonymousConnections.Values.Count()
                }
            });
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();

            if (!IsAuthenticatedConnection(out var claims))
            {
                var value = new AnonymousAccessInfo
                {
                    AccessDate = DateTime.Now,
                    Ip = AuthUtility.TryGetIP(_accessor.HttpContext.Request),
                    ConnectionId = Context.ConnectionId,
                    UserAgent = _accessor.HttpContext.Request.Headers[HeaderNames.UserAgent].ToString(),
                    Type = AccountType.Anonymous
                };

                AnonymousConnections.TryAdd(Context.ConnectionId, value);
            }
            else
            {
                var userId = claims.First(x => x.Type == ClaimConstant.USER_ID).Value;
                var username = claims.First(x => x.Type == ClaimConstant.USERNAME).Value;
                var fullname = claims.First(x => x.Type == ClaimConstant.FULL_NAME).Value;
                var type = (AccountType)int.Parse(claims.First(x => x.Type == ClaimConstant.ACCOUNT_TYPE).Value);
                var key = BaseCacheKeys.GetSocketKey(long.Parse(userId));

                var value = new AuthenticatedAccessInfo
                {
                    ConnectionId = Context.ConnectionId,
                    AccessDate = DateTime.Now,
                    UserId = userId,
                    Username = username,
                    Fullname = fullname,
                    Ip = AuthUtility.TryGetIP(_accessor.HttpContext.Request),
                    UserAgent = _accessor.HttpContext.Request.Headers[HeaderNames.UserAgent].ToString(),
                    Type = type
                };
                switch (type)
                {
                    case AccountType.Customer:
                        if (CustomerConnections.TryGetValue(key, out var values))
                        {
                            values.Add(value);
                            CustomerConnections[key] = values;
                        }
                        else
                        {
                            CustomerConnections[key] = new List<AuthenticatedAccessInfo>
                            {
                               value
                            };
                        }
                        break;
                    case AccountType.Employee:
                        if (EmployeeConnections.TryGetValue(key, out var values_2))
                        {
                            values_2.Add(value);
                            EmployeeConnections[key] = values_2;
                        }
                        else
                        {
                            EmployeeConnections[key] = new List<AuthenticatedAccessInfo>
                            {
                               value
                            };
                        }
                        break;
                    default:
                        break;
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
                var key = BaseCacheKeys.GetSocketKey(long.Parse(userId));

                switch (type)
                {
                    case AccountType.Customer:
                        if (CustomerConnections.ContainsKey(key))
                        {
                            if (CustomerConnections[key].Count > 1)
                            {
                                var connection = CustomerConnections[key].First(x => x.ConnectionId == Context.ConnectionId);
                                var values = CustomerConnections[key].Remove(connection);
                            }
                            else
                            {
                                CustomerConnections.Remove(key, out var v);
                            }
                        }
                        break;
                    case AccountType.Employee:
                        if (EmployeeConnections.ContainsKey(key))
                        {
                            if (EmployeeConnections[key].Count > 1)
                            {
                                var connection = EmployeeConnections[key].First(x => x.ConnectionId == Context.ConnectionId);
                                var values = EmployeeConnections[key].Remove(connection);
                            }
                            else
                            {
                                EmployeeConnections.Remove(key, out var v);
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            await SendNumberOfOnlineUsers();
        }

        private bool IsAuthenticatedConnection(out IEnumerable<Claim> claims)
        {
            claims = (Context.User.Identity as ClaimsIdentity).Claims;
            return claims != null && claims.Any();
        }
    }
}
