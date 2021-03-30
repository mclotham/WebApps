using ContosoUniv.Data;
using ContosoUniv.WebApp.Authorization;
using ContosoUniv.WebApp.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoUniv.WebApp.Controllers.Admin
{
    public class UserController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ContosoUnivContext _dbContext;

        public UserController( ContosoUnivContext dbContext, UserManager<IdentityUser> userManager )
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize( Policy = Permits.Admin.UserCreate )]
        public IActionResult Create()
        {
            var model = new UserCreateMdl();
            model.LoadPersonList( _dbContext );
            model.LoadRoleList( _dbContext, null );

            return View( model );
        }

        [HttpPost]
        [Authorize( Policy = Permits.Admin.UserCreate )]
        public async Task<IActionResult> Create( UserCreateMdl model )
        {
            if ( model.SelectedPerson == "0" )
            {
                ModelState.AddModelError( "", "A person must be selected." );
            }

            if ( string.IsNullOrEmpty( model.SelectedRoles ) )
            {
                ModelState.AddModelError( "", "At least one role must be selected." );
            }

            if ( !ModelState.IsValid )
            {
                model.LoadPersonList( _dbContext, model.SelectedPerson );
                model.LoadRoleList( _dbContext, model.SelectedRoles );
                return View( model );
            }

            var user = new IdentityUser { UserName = model.Username };
            var result = await _userManager.CreateAsync( user, model.Password );

            if ( result.Succeeded )
            {
                var roleList = model.SelectedRoles.Split( new char[] { ';' } );
                result = await _userManager.AddToRolesAsync( user, roleList );

                _dbContext.IdentityPerson.Add( new IdentityPerson { IdentityId = user.Id, PersonId = Int32.Parse( model.SelectedPerson ) } );
                _dbContext.SaveChanges();

                return RedirectToAction( "Index", "Home" );
            }
            else
            {
                foreach ( var error in result.Errors )
                    ModelState.AddModelError( string.Empty, error.Description );
                model.LoadPersonList( _dbContext, model.SelectedPerson );
                model.LoadRoleList( _dbContext, model.SelectedRoles );
                return View( model );
            }
        }
    }
}
