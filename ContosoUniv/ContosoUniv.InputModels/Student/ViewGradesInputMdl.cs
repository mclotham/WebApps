using ContosoUniv.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ContosoUniv.InputModels.Student
{
    public class ViewGradesInputMdl
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

        public void CreateGradeList( ContosoUnivContext dbContext )
        {
            using ( dbContext )
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

                    var grades =
                        from grade in dbContext.StudentGrade
                        join person in dbContext.Person on grade.StudentId equals person.PersonId
                        join course in dbContext.Course on grade.CourseId equals course.CourseId
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
