using ContosoUniv.Data;
using ContosoUniv.WebApp.Authorization;
using ContosoUniv.WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoUniv.WebApp.Controllers
{
    [Authorize]
    public class StudentController : Controller
    {
        private ContosoUnivContext _dbContext;

        public StudentController( ContosoUnivContext dbContext )
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        [Authorize( Policy = Permits.Student.ViewGrades )]
        public IActionResult ViewGrades()
        {
            var model = new StudentViewGradesMdl();
            model.Username = HttpContext.User.Identity.Name;
            model.CreateGradeList( _dbContext );
            return View( model );
        }
    }
}
