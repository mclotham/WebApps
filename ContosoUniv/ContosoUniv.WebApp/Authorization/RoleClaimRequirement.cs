using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoUniv.WebApp.Authorization
{
    public class RoleClaimRequirement : IAuthorizationRequirement
    {
        public string RoleClaim { get; }

        public RoleClaimRequirement( string roleClaim )
        {
            RoleClaim = roleClaim;
        }
    }
}
