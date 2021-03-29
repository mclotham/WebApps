using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoUniv.WebApp.Models.Role
{
    public class DeleteMdl
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool AllowDelete { get; set; }
    }
}
