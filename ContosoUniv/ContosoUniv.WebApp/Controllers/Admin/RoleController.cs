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
        private readonly ContosoUnivContext _dbContext;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController( ContosoUnivContext dbContext, RoleManager<IdentityRole> roleManager )
        {
            _dbContext = dbContext;
            _roleManager = roleManager;
        }

        [HttpGet]
        [Authorize( Policy = Permits.Admin.RoleManage )]
        public IActionResult Index()
        {
            var model = new ManageMdl();
            model.LoadRoleInfo( _dbContext );
            return View( model );
        }

        [HttpGet]
        [Authorize( Policy = Permits.Admin.RoleDetails )]
        public IActionResult Details( string id )
        {
            var model = new DetailsMdl { Id = id };
            model.LoadDetails( _dbContext );
            return View( model );
        }

        [HttpGet]
        [Authorize( Policy = Permits.Admin.RoleCreate )]
        public IActionResult Create()
        {
            var model = new CreateMdl();
            model.LoadPermitList();

            return View( model );
        }

        [HttpPost]
        [Authorize( Policy = Permits.Admin.RoleCreate )]
        public async Task<IActionResult> Create( CreateMdl model )
        {
            var role = new IdentityRole { Name = model.Name };
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
                return RedirectToAction( "Index" );
            }
            else
            {
                foreach ( var error in result.Errors )
                    ModelState.AddModelError( string.Empty, error.Description );
                model.LoadPermitList( model.SelectedPermits );
                return View( model );
            }

        }
        [HttpGet]
        [Authorize( Policy = Permits.Admin.RoleEdit )]
        public async Task<IActionResult> Edit( string id)
        {
            var model = new EditMdl { Id = id };

            var role = await _roleManager.FindByIdAsync( id );
            model.Name = role.Name;
            var claimsTypes = ( await _roleManager.GetClaimsAsync( role ) ).Select( c => c.Type.Replace( '.', '_' )).ToArray();
            var permitItems = string.Join( ';', claimsTypes );
            model.LoadPermitList( permitItems );

            return View( model );
        }

        [HttpPost]
        [Authorize( Policy = Permits.Admin.RoleEdit )]
        public async Task<IActionResult> Edit( EditMdl model )
        {
            var selectedPermits = model.SelectedPermits.Split( new char[] { ';' } ).Select( p => p.Replace( '_', '.' ) );
            var role = await _roleManager.FindByIdAsync( model.Id );
            var currentClaims = await _roleManager.GetClaimsAsync( role );

            var removeClaims = currentClaims.Where( c => !selectedPermits.Contains( c.Type ) );
            foreach ( var claim in removeClaims )
            {
                var result = await _roleManager.RemoveClaimAsync( role, claim );
                if ( !result.Succeeded )
                    foreach ( var error in result.Errors )
                        ModelState.AddModelError( "", error.Description );
            }

            var currentPermits = currentClaims.Select( c => c.Type );
            var addPermits = selectedPermits.Except( currentPermits );
            foreach ( var permit in addPermits )
            {
                var result = await _roleManager.AddClaimAsync( role, new Claim( permit, "" ) );
                if ( !result.Succeeded )
                    foreach ( var error in result.Errors )
                        ModelState.AddModelError( "", error.Description );
            }

            if ( !ModelState.IsValid )
            {
                return View();
            }

            return RedirectToAction( "Index" );
        }

        [HttpGet]
        [Authorize( Policy = Permits.Admin.RoleDelete )]
        public async Task<IActionResult> Delete( string id )
        {
            var model = new DeleteMdl { Id = id };

            var role = await _roleManager.FindByIdAsync( id );
            model.Name = role.Name;

            var numUsers = _dbContext.AspNetUserRoles.Where( r => r.RoleId == id ).Count();
            model.AllowDelete = numUsers == 0;

            return View( model );
        }

        [HttpPost]
        [Authorize( Policy = Permits.Admin.RoleDelete )]
        public async Task<IActionResult> Delete( DeleteMdl model)
        {
            var role = await _roleManager.FindByIdAsync( model.Id );
            var result = await _roleManager.DeleteAsync( role );

            if ( !result.Succeeded )
            {
                foreach ( var error in result.Errors )
                    ModelState.AddModelError( "", error.Description );
                return View( model );
            }

            return RedirectToAction( "Index" );
        }
    }
}
