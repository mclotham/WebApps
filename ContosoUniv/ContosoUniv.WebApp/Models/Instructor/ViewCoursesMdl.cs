using ContosoUniv.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoUniv.WebApp.Models.Instructor
{
    public class ViewCoursesMdl
    {
        public string Username { get; set; }
        public string IdentityId { get; set; }
        public string FullName { get; set; }
        public List<CourseInfo> CourseList { get; set; }

        public class CourseInfo
        {
            public string CourseName { get; set; }
            public string Department { get; set; }
            public string NumCredits { get; set; }
        }

        public void LoadCourseInfo( ContosoUnivContext dbContext )
        {
            var personInfoList =
                from person in dbContext.Person
                join idperson in dbContext.IdentityPerson on person.PersonId equals idperson.PersonId
                join aspuser in dbContext.AspNetUsers on idperson.IdentityId equals aspuser.Id
                where aspuser.UserName == this.Username
                select new { person.PersonId, person.FirstName, person.LastName };

            var personInfo = personInfoList.Any() ? personInfoList.First() : null;

            if ( personInfo != null )
            {
                FullName = $"{personInfo.FirstName} {personInfo.LastName}";

                var courseInfoList =
                    from crsInst in dbContext.CourseInstructor
                    join crs in dbContext.Course on crsInst.CourseId equals crs.CourseId
                    join dept in dbContext.Department on crs.DepartmentId equals dept.DepartmentId
                    where crsInst.PersonId == personInfo.PersonId
                    select new CourseInfo
                    {
                        CourseName = crs.Title,
                        Department = dept.Name,
                        NumCredits = crs.Credits.ToString()
                    };

                CourseList = courseInfoList.OrderBy( o => o.CourseName ).ToList();
            }
        }
    }
}
