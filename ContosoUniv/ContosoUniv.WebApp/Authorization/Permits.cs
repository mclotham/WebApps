using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ContosoUniv.WebApp.Authorization
{
    public static class Permits
    {
        public class Admin
        {
            public const string RoleCreate = "Admin.RoleCreate";
            public const string RoleManage = "Admin.RoleManage";
            public const string RoleDetails = "Admin.RoleDetails";
            public const string RoleEdit = "Admin.RoleEdit";
            public const string RoleDelete = "Admin.RoleDelete";

            public const string UserCreate = "Admin.UserCreate";
            public const string UserManage = "Admin.UserManage";
            public const string UserDetails = "Admin.UserDetails";
            public const string UserEdit = "Admin.UserEdit";
            public const string UserDelete = "Admin.UserDelete";

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

        public static readonly string[] AllPermits =
        {
                Student.ViewGrades,

                Instructor.ViewCourses,
                Instructor.ViewClassRoster,

                Admin.RoleCreate,
                Admin.RoleManage,
                Admin.RoleDetails,
                Admin.RoleEdit,
                Admin.RoleDelete,

                Admin.UserCreate,
                Admin.UserManage,
                Admin.UserDetails,
                Admin.UserEdit,
                Admin.UserDelete,
        };

        public static List<string> GetAllPermits()
        {
            List<string> allPermits = new();
            var permitClassType = typeof( Permits );
            var subClasses = permitClassType.GetMembers().Where( pc => pc.MemberType == MemberTypes.NestedType );
            foreach ( var subClass in subClasses )
            {
                var subClassType = Type.GetType( permitClassType.FullName + "+" + subClass.Name );
                var permitFields = subClassType.GetFields();
                foreach ( var permitField in permitFields )
                    allPermits.Add( permitField.GetValue( permitField ) as string);
            }

            allPermits.Sort();
            return allPermits;
        }
    }
}
