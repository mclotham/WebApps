using System;
using System.Collections.Generic;

#nullable disable

namespace ContosoUniv.Data
{
    public partial class OfficeAssignment
    {
        public int InstructorId { get; set; }
        public string Location { get; set; }
        public byte[] Timestamp { get; set; }

        public virtual Person Instructor { get; set; }
    }
}
