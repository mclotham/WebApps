using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ContosoUniv.Authorization;
using ContosoUniv.InputModels.Admin.Role;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContosoUniv.WebAppRp.Areas.Admin.Pages.Role
{
    [Authorize( Policy = Permits.Admin.RoleEdit )]
    public class EditModel : PageModel
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        [BindProperty]
        public EditInputMdl Input { get; set; }

        public EditModel( RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task OnGetAsync( string id )
        {
            Input = new EditInputMdl { Id = id };

            var role = await _roleManager.FindByIdAsync( id );
            Input.Name = role.Name;
            var claimsTypes = ( await _roleManager.GetClaimsAsync( role ) ).Select( c => c.Type.Replace( '.', '_' ) ).ToArray();
            var permitItems = string.Join( ';', claimsTypes );
            Input.LoadPermitList( permitItems );
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var selectedPermits = Input.SelectedPermits.Split( new char[] { ';' } ).Select( p => p.Replace( '_', '.' ) );
            var role = await _roleManager.FindByIdAsync( Input.Id );
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
                return Page();
            }

            return RedirectToPage( "Index" );
        }
    }
}
