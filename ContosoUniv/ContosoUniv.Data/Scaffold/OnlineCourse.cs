﻿using System;
using System.Collections.Generic;

#nullable disable

namespace ContosoUniv.Data
{
    public partial class OnlineCourse
    {
        public int CourseId { get; set; }
        public string Url { get; set; }

        public virtual Course Course { get; set; }
    }
}
