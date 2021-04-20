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
    [Authorize( Policy = Permits.Admin.RoleCreate)]
    public class CreateModel : PageModel
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ContosoUnivContext _dbContext;

        [BindProperty]
        public CreateInputMdl Input { get; set; }

        public CreateModel( RoleManager<IdentityRole> roleManager, ContosoUnivContext dbContext )
        {
            _roleManager = roleManager;
            _dbContext = dbContext;
        }

        public void OnGet()
        {
            Input = new();
            Input.LoadPermitList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var role = new IdentityRole { Name = Input.Name };
            var result = await _roleManager.CreateAsync( role );
            if ( result.Succeeded )
            {
                if ( !string.IsNullOrEmpty( Input.SelectedPermits ) )
                {
                    foreach ( var permit in Input.SelectedPermits.Split( new char[] { ';' } ) )
                    {
                        result = await _roleManager.AddClaimAsync( role, new Claim( permit.Replace( '_', '.' ), "" ) );
                    }
                }
                return Redirect( Url.Content( "~/" ) );
            }
            else
            {
                foreach ( var error in result.Errors )
                    ModelState.AddModelError( string.Empty, error.Description );
                Input.LoadPermitList( Input.SelectedPermits );
                return Page();
            }

        }
    }
}
