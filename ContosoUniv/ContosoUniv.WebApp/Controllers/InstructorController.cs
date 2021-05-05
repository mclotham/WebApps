using ContosoUniv.Data;
using ContosoUniv.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContosoUniv.InputModels.Instructor;

namespace ContosoUniv.WebApp.Controllers
{
    public class InstructorController : Controller
    {
        private readonly ContosoUnivContext _dbContext;
        public InstructorController( ContosoUnivContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize( Policy = Permits.Instructor.ViewCourses )]
        public IActionResult ViewCourses()
        {
            var model = new ViewCoursesInputMdl { Username = User.Identity.Name };
            model.LoadCourseInfo( _dbContext );
            return View( model );
        }
    }
}
