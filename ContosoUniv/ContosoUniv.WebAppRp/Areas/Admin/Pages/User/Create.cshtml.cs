using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContosoUniv.Authorization;
using ContosoUniv.Data;
using ContosoUniv.InputModels.Admin.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContosoUniv.WebAppRp.Areas.Admin.Pages.User
{
    [Authorize( Policy = Permits.Admin.UserCreate )]
    public class CreateModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ContosoUnivContext _dbContext;

        [BindProperty]
        public CreateInputMdl Input { get; set; }

        public CreateModel( UserManager<IdentityUser> userManager, ContosoUnivContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        public void OnGet()
        {
            Input = new CreateInputMdl();
            Input.LoadPersonList( _dbContext );
            Input.LoadRoleList( _dbContext, null );
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if ( Input.SelectedPerson == "0" )
            {
                ModelState.AddModelError( "", "A person must be selected." );
            }

            if ( string.IsNullOrEmpty( Input.SelectedRoles ) )
            {
                ModelState.AddModelError( "", "At least one role must be selected." );
            }

            if ( !ModelState.IsValid )
            {
                Input.LoadPersonList( _dbContext, Input.SelectedPerson );
                Input.LoadRoleList( _dbContext, Input.SelectedRoles );
                return Page();
            }

            var user = new IdentityUser { UserName = Input.Username };
            var result = await _userManager.CreateAsync( user, Input.Password );

            if ( result.Succeeded )
            {
                var roleList = Input.SelectedRoles.Split( new char[] { ';' } );
                result = await _userManager.AddToRolesAsync( user, roleList );

                _dbContext.IdentityPerson.Add( new IdentityPerson { IdentityId = user.Id, PersonId = Int32.Parse( Input.SelectedPerson ) } );
                _dbContext.SaveChanges();

                return LocalRedirect( "~/" );
            }
            else
            {
                foreach ( var error in result.Errors )
                    ModelState.AddModelError( string.Empty, error.Description );
                Input.LoadPersonList( _dbContext, Input.SelectedPerson );
                Input.LoadRoleList( _dbContext, Input.SelectedRoles );
                return Page();
            }

        }
    }
}
