using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TodoList.Api.Framework.Extensions {
    public static class ModelStateExtensions {
        public static void AddIdentityErrors(this ModelStateDictionary modelState, IdentityResult identityResult) {
            foreach (IdentityError error in identityResult.Errors) {
                modelState.AddModelError(error.Code, error.Description);
            }
        }
    }
}