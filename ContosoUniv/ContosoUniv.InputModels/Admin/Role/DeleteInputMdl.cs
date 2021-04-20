using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoUniv.InputModels.Admin.Models.Role
{
    public class DeleteInputMdl
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool AllowDelete { get; set; }
    }
}
