using ContosoUniv.Data;
using ContosoUniv.WebApp.Authorization;
using ContosoUniv.WebApp.Models.Instructor;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoUniv.WebApp.Controllers
{
    public class InstructorController : Controller
    {
        private ContosoUnivContext _dbContext;
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
            var model = new InstructorViewCoursesMdl();
            model.Username = User.Identity.Name;
            model.LoadCourseInfo( _dbContext );
            return View( model );
        }
    }
}
