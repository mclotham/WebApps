using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ContosoUniv.WebApp.Authorization
{
    public class RoleClaimHandler : AuthorizationHandler<RoleClaimRequirement>
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleClaimHandler( UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager ) : base()
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        protected override Task HandleRequirementAsync( AuthorizationHandlerContext context, RoleClaimRequirement requirement )
        {
            if ( context.User.Identity.Name == null )
                return Task.CompletedTask;

            IdentityUser user = Task.Run( async () => await _userManager.FindByNameAsync( context.User.Identity.Name ) ).Result;
            IList<string> roles = Task.Run( async () => await _userManager.GetRolesAsync( user ) ).Result;

            foreach ( var roleName in roles )
            {
                IdentityRole role = Task.Run( async () => await _roleManager.FindByNameAsync( roleName ) ).Result;
                if ( Task.Run( async () => await _roleManager.GetClaimsAsync( role ) ).Result.Where( claim => claim.Type == requirement.RoleClaim ).Any() )
                {
                    context.Succeed( requirement );
                    break;
                }
            }

            return Task.CompletedTask;
        }
    }
}
