using System;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Http;

namespace TodoList.Api.Services {
    public interface IUserIdProvider {
        string GetUserId();
    }

    public class UserIdProvider : IUserIdProvider {
        private readonly IHttpContextAccessor _accessor;

        public UserIdProvider(IHttpContextAccessor accessor) {
            this._accessor = accessor;
        }

        public string GetUserId() {
            IIdentity identity = this._accessor.HttpContext.User.Identity;
            if (!identity.IsAuthenticated) {
                return Guid.Empty.ToString();
            }

            return this._accessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
    }
}