using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContosoUniv.Authorization;
using ContosoUniv.InputModels.Instructor;
using ContosoUniv.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContosoUniv.WebAppRp.Areas.Instructor.Pages
{
    [Authorize( Policy = Permits.Instructor.ViewCourses )]
    public class ViewCoursesModel : PageModel
    {
        private readonly ContosoUnivContext _dbContext;

        [BindProperty]
        public ViewCoursesInputMdl Input { get; set; }

        public ViewCoursesModel( ContosoUnivContext dbContext )
        {
            _dbContext = dbContext;
        }

        public void OnGet()
        {
            Input = new ViewCoursesInputMdl { Username = User.Identity.Name };
            Input.LoadCourseInfo( _dbContext );
        }
    }
}
