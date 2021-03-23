using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoUniv.WebApp.Authorization
{
    public static class Permits
    {
        public class Admin
        {
            public const string RoleCreate = "Admin.RoleCreate";
        }

        public class Student
        {
            public const string ViewGrades = "Student.ViewGrades";
        }

        public class Instructor
        {
            public const string ViewClasses = "Instructor.ViewClasses";
            public const string ViewClassRoster = "Instructor.ViewClassRoster";
        }

        public readonly static string[] AllPermits =
        {
                Student.ViewGrades,

                Instructor.ViewClasses,
                Instructor.ViewClassRoster,

                Admin.RoleCreate,
        };
    }
}
