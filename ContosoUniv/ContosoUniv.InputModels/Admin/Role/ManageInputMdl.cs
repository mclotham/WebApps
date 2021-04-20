using ContosoUniv.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoUniv.InputModels.Admin.Models.Role
{
    public class ManageInputMdl
    {
        public List<RoleInfo> RoleInfoList { get; set; }

        public class RoleInfo
        {
            public string Id { get; set; }
            public string RoleName { get; set; }
            public int NumUsers { get; set; }
        }

        public void LoadRoleInfo( ContosoUnivContext dbContext )
        {
            RoleInfoList = dbContext.AspNetRoles
                .Select( role => new RoleInfo
                {
                    Id = role.Id,
                    RoleName = role.Name,
                    NumUsers = role.AspNetUserRoles.Count
                } )
                .OrderBy( o => o.RoleName )
                .ToList();
        }
    }
}
