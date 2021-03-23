using System;
using System.Collections.Generic;

#nullable disable

namespace ContosoUniv.Data
{
    public partial class IdentityPerson
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public string IdentityId { get; set; }

        public virtual Person Person { get; set; }
    }
}
