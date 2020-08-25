using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace TodoList.Api.Framework.Extensions {
    public static class ControllerBaseExtensions {
        public static string GetUserId(this ControllerBase controller) => GetClaimValue(controller, ClaimTypes.NameIdentifier);
        public static string GetUserName(this ControllerBase controller) => GetClaimValue(controller, ClaimTypes.Name);
        public static string GetUserEmail(this ControllerBase controller) => GetClaimValue(controller, ClaimTypes.Email);

        private static string GetClaimValue(ControllerBase controller, string claimType) {
            return controller.User.FindFirst(claimType).Value;
        }
    }
}