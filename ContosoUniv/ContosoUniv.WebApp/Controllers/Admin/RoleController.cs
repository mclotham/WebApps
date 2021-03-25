using ContosoUniv.Data;
using ContosoUniv.WebApp.Authorization;
using ContosoUniv.WebApp.Models.Role;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ContosoUniv.WebApp.Controllers
{
    public class RoleController : Controller
    {
        private ContosoUnivContext _dbContext;
        private RoleManager<IdentityRole> _roleManager;

        public RoleController( ContosoUnivContext dbContext, RoleManager<IdentityRole> roleManager )
        {
            _dbContext = dbContext;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize( Policy = Permits.Admin.RoleCreate )]
        public IActionResult Create()
        {
            var model = new RoleCreateMdl();
            model.LoadPermitList();

            return View( model );
        }

        [HttpPost]
        [Authorize( Policy = Permits.Admin.RoleCreate )]
        public async Task<IActionResult> Create( RoleCreateMdl model )
        {
            var role = new IdentityRole { Name = model.RoleName };
            var result = await _roleManager.CreateAsync( role );
            if ( result.Succeeded )
            {
                if ( !string.IsNullOrEmpty( model.SelectedPermits ) )
                {
                    foreach ( var permit in model.SelectedPermits.Split( new char[] { ';' } ) )
                    {
                        result = await _roleManager.AddClaimAsync( role, new Claim( permit.Replace( '_', '.' ), "" ) );
                    }
                }
                return RedirectToAction( "Index", "Home" );
            }
            else
            {
                foreach ( var error in result.Errors )
                    ModelState.AddModelError( string.Empty, error.Description );
                model.LoadPermitList( model.SelectedPermits );
                return View( model );
            }

        }
    }
}
