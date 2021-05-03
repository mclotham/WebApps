using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContosoUniv.Authorization;
using ContosoUniv.Data;
using ContosoUniv.InputModels.Student;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContosoUniv.WebAppRp.Areas.Student.Pages
{
    [Authorize( Policy = Permits.Student.ViewGrades )]
    public class ViewGradesModel : PageModel
    {
        private readonly ContosoUnivContext _dbContext;

        [BindProperty]
        public ViewGradesInputMdl Input { get; set; }

        public ViewGradesModel( ContosoUnivContext dbContext )
        {
            _dbContext = dbContext;
        }

        public void OnGet()
        {
            Input = new ViewGradesInputMdl { Username = User.Identity.Name };
            Input.CreateGradeList( _dbContext );
        }
    }
}
