using Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DataModels.Identity
{
    public class AppClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, ApplicationRole>
    {
        public AppClaimsPrincipalFactory(
            UserManager<ApplicationUser> userManager
            , RoleManager<ApplicationRole> roleManager
            , IOptions<IdentityOptions> optionsAccessor)
        : base(userManager, roleManager, optionsAccessor)
        { }

        public async override Task<ClaimsPrincipal> CreateAsync(ApplicationUser user)
        {
            var principal = await base.CreateAsync(user);

            if (!string.IsNullOrWhiteSpace(user.Id))
            {
                ((ClaimsIdentity)principal.Identity).AddClaims(new[] {
                    new Claim(Enums.UserEnums.Id, user.Id),
                });
            }

            if (!string.IsNullOrWhiteSpace(user.Name))
            {
                ((ClaimsIdentity)principal.Identity).AddClaims(new[] {
                    new Claim(Enums.UserEnums.Name, user.Name)
                });
            }

            if (!string.IsNullOrWhiteSpace(user.FirstName))
            {
                ((ClaimsIdentity)principal.Identity).AddClaims(new[] {
                    new Claim(Enums.UserEnums.FirstName, user.FirstName)
                });
            }

            if (!string.IsNullOrWhiteSpace(user.LastName))
            {
                ((ClaimsIdentity)principal.Identity).AddClaims(new[] {
                    new Claim(Enums.UserEnums.LastName, user.LastName),
                });
            }

            if (!string.IsNullOrWhiteSpace(user.PhoneNumber))
            {
                ((ClaimsIdentity)principal.Identity).AddClaims(new[] {
                    new Claim(Enums.UserEnums.Phone, user.PhoneNumber),
                });
            }
            if (!string.IsNullOrWhiteSpace(user.PictureUrl))
            {
                ((ClaimsIdentity)principal.Identity).AddClaims(new[] {
                    new Claim(Enums.UserEnums.Picture, user.PictureUrl),
                });
            }

            //Department Set
            ((ClaimsIdentity)principal.Identity).AddClaims(new[] {
                new Claim(Enums.UserEnums.DepartmentId, user.DepartmentId.ToString()),
            });

            return principal;
        }
    }
}
