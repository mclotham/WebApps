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
    [Authorize( Policy = Permits.Admin.RoleManage )]
    public class IndexModel : PageModel
    {
        private readonly ContosoUnivContext _dbContext;

        [BindProperty]
        public ManageInputMdl Input { get; set; }

        public IndexModel( ContosoUnivContext dbContext )
        {
            _dbContext = dbContext;
        }

        public void OnGet()
        {
            Input = new ManageInputMdl();
            Input.LoadRoleInfo( _dbContext );
        }
    }
}
