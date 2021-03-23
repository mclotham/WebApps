using ContosoUniv.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoUniv.WebApp.Models
{
    public class StudentViewGradesMdl
    {
        public class GradeInfo
        {
            public string CourseName { get; set; }
            public string Grade { get; set; }
        }

        public string Username { get; set; }
        public string IdentityId { get; set; }
        public string FullName { get; set; }
        public List<GradeInfo> GradeList { get; set; }

        private ContosoUnivContext _dbContext;

        public StudentViewGradesMdl( ContosoUnivContext dbContext )
        {
            _dbContext = dbContext;
        }

        public void CreateGradeList()
        {
            using ( _dbContext )
            {
                var personInfoList =
                    from person in _dbContext.Person
                    join idperson in _dbContext.IdentityPerson on person.PersonId equals idperson.PersonId
                    join aspuser in _dbContext.AspNetUsers on idperson.IdentityId equals aspuser.Id
                    where aspuser.UserName == this.Username
                    select new { person.PersonId, person.FirstName, person.LastName };

                var personInfo = personInfoList.Count() > 0 ? personInfoList.First() : null;

                if ( personInfo != null )
                {
                    FullName = $"{personInfo.FirstName} {personInfo.LastName}";

                    var grades =
                        from grade in _dbContext.StudentGrade
                        join person in _dbContext.Person on grade.StudentId equals person.PersonId
                        join course in _dbContext.Course on grade.CourseId equals course.CourseId
                        where person.PersonId == personInfo.PersonId
                        select new GradeInfo
                        {
                            CourseName = course.Title,
                            Grade = grade.Grade.ToString()
                        };

                    GradeList = grades.OrderBy( o => o.CourseName ).ToList();
                }
            }
        }
    }
}
