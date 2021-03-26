﻿using System;
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
            public const string UserCreate = "Admin.UserCreate";
        }

        public class Student
        {
            public const string ViewGrades = "Student.ViewGrades";
        }

        public class Instructor
        {
            public const string ViewCourses = "Instructor.ViewCourses";
            public const string ViewClassRoster = "Instructor.ViewClassRoster";
        }

        public readonly static string[] AllPermits =
        {
                Student.ViewGrades,

                Instructor.ViewCourses,
                Instructor.ViewClassRoster,

                Admin.RoleCreate,
                Admin.UserCreate,
        };
    }
}
