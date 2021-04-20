using ContosoUniv.Data;
using ContosoUniv.Authorization;
using ContosoUniv.InputModels.Admin.Role;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace ContosoUniv.WebAppRp.Areas.Admin.Pages.Role
{
    [Authorize( Policy = Permits.Admin.RoleDelete )]
    public class DeleteModel : PageModel
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ContosoUnivContext _dbContext;

        [BindProperty]
        public DeleteInputMdl Input { get; set; }

        public DeleteModel( RoleManager<IdentityRole> roleManager, ContosoUnivContext dbContext )
        {
            _roleManager = roleManager;
            _dbContext = dbContext;
        }

        public async Task OnGetAsync( string id )
        {
            Input = new DeleteInputMdl { Id = id };

            var role = await _roleManager.FindByIdAsync( id );
            Input.Name = role.Name;

            Input.AllowDelete = !_dbContext.AspNetUserRoles.Where( r => r.RoleId == id ).Any();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var role = await _roleManager.FindByIdAsync( Input.Id );
            var result = await _roleManager.DeleteAsync( role );

            if ( !result.Succeeded )
            {
                foreach ( var error in result.Errors )
                    ModelState.AddModelError( "", error.Description );
                return Page();
            }

            return RedirectToPage( "Index" );
        }
    }
}
