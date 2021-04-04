using Microsoft.AspNetCore.Authentication;
using System;
using System.Security.Claims;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ContosoUniv.WebApp.Authorization
{
    public class RoleClaimTransformer : IClaimsTransformation
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleClaimTransformer(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager )
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public Task<ClaimsPrincipal> TransformAsync( ClaimsPrincipal principal )
        {
            ClaimsIdentity ci = (ClaimsIdentity)principal.Identity;

            IdentityUser user = Task.Run( async () => await _userManager.FindByNameAsync( ci.Name ) ).Result;
            IList<string> roles = Task.Run( async () => await _userManager.GetRolesAsync( user ) ).Result;

            foreach ( string roleName in roles )
            {
                IdentityRole role = Task.Run( async () => await _roleManager.FindByNameAsync( roleName ) ).Result;
                IList<Claim> roleClaims = Task.Run( async () => await _roleManager.GetClaimsAsync( role ) ).Result;
                foreach ( Claim roleClaim in roleClaims )
                    if ( !ci.HasClaim( roleClaim.Type, "" ) )
                        ci.AddClaim( new Claim( roleClaim.Type, "" ) );
            }

            return Task.FromResult( principal );
        }
    }
}