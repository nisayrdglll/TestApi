using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using static DataAccess.Handlers.RoleAuthorizationHandler;

namespace DataAccess.Handlers
{
    public class RoleAuthorizationHandler : AuthorizationHandler<RolesRequirement>
    {
        private string? ActionName;
        private string? ControllerName;

        public RoleAuthorizationHandler(IHttpContextAccessor actionContextAccessor)
        {
            //_roleAuthService = roleAuthService;
            var routeValues = ((dynamic)actionContextAccessor.HttpContext.Request).RouteValues as IReadOnlyDictionary<string, object>;
            this.ControllerName = routeValues["controller"].ToString();
            this.ActionName = routeValues["action"].ToString();
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RolesRequirement requirement)
        {
            //if (_roleAuthService.Find(p => p.RoleId == roleId))
            //{

            //}

            if (!string.IsNullOrWhiteSpace(requirement.Groups))
            {
                var groups = requirement.Groups.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                //we could check for group membership here.... maybe???
                foreach (var group in groups)
                {
                    if (context.User.IsInRole(group))
                    {
                        context.Succeed(requirement);
                        return Task.FromResult(0);
                    }
                }
            }
            else
            {
                //context.Succeed(requirement);
                //var mvcContext = context.Resource as AuthorizationFilterContext;
                //mvcContext.Result = new RedirectToActionResult("Index", "Home", null);
                //context.Succeed(requirement);
            }
            return Task.FromResult(0);
            //context.Fail();
            //context.Result = new RedirectToActionResult("Index", "Home", null);
        }

        public class RolesRequirement : IAuthorizationRequirement
        {
            public RolesRequirement(string groups)
            {
                Groups = groups;
            }

            public string Groups { get; private set; }
        }
    }
}
