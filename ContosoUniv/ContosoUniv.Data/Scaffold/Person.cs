using System;
using System.Collections.Generic;

#nullable disable

namespace ContosoUniv.Data
{
    public partial class Person
    {
        public Person()
        {
            CourseInstructor = new HashSet<CourseInstructor>();
            IdentityPerson = new HashSet<IdentityPerson>();
            StudentGrade = new HashSet<StudentGrade>();
        }

        public int PersonId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public DateTime? HireDate { get; set; }
        public DateTime? EnrollmentDate { get; set; }
        public string Discriminator { get; set; }

        public virtual OfficeAssignment OfficeAssignment { get; set; }
        public virtual ICollection<CourseInstructor> CourseInstructor { get; set; }
        public virtual ICollection<IdentityPerson> IdentityPerson { get; set; }
        public virtual ICollection<StudentGrade> StudentGrade { get; set; }
    }
}
