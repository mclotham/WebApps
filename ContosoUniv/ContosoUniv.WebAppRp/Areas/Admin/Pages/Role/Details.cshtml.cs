using ContosoUniv.Data;
using ContosoUniv.Authorization;
using ContosoUniv.InputModels.Admin.Role;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContosoUniv.WebAppRp.Areas.Admin.Pages.Role
{
    [Authorize( Policy = Permits.Admin.RoleDetails )]
    public class DetailsModel : PageModel
    {
        private readonly ContosoUnivContext _dbContext;

        [BindProperty]
        public DetailsInputMdl Input { get; set; }

        public DetailsModel( ContosoUnivContext dbContext )
        {
            _dbContext = dbContext;
        }

        public void OnGet( string id )
        {
            Input = new DetailsInputMdl { Id = id };
            Input.LoadDetails( _dbContext );
        }
    }
}
