using ContosoUniv.Data;
using ContosoUniv.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContosoUniv.InputModels.Student;

namespace ContosoUniv.WebApp.Controllers
{
    [Authorize]
    public class StudentController : Controller
    {
        private readonly ContosoUnivContext _dbContext;

        public StudentController( ContosoUnivContext dbContext )
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        [Authorize( Policy = Permits.Student.ViewGrades )]
        public IActionResult ViewGrades()
        {
            var model = new ViewGradesInputMdl { Username = User.Identity.Name };
            model.CreateGradeList( _dbContext );
            return View( model );
        }
    }
}
