using ContosoUniv.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoUniv.InputModels.Admin.Models.Role
{
    public class DetailsInputMdl
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> UserList { get; set; }
        public List<string> PermitList { get; set; }

        public void LoadDetails( ContosoUnivContext dbContext )
        {
            Name = dbContext.AspNetRoles.Find( Id ).Name;
            LoadUserList( dbContext );
            LoadPermitList( dbContext );
        }

        public void LoadUserList( ContosoUnivContext dbContext )
        {
            var userList =
                from userRole in dbContext.AspNetUserRoles
                join user in dbContext.AspNetUsers on userRole.UserId equals user.Id
                where userRole.RoleId == Id
                select user.UserName;

            UserList = userList.OrderBy( o => o ).ToList();
        }

        public void LoadPermitList( ContosoUnivContext dbContext )
        {
            var permitList = dbContext.AspNetRoleClaims.Where( rc => rc.RoleId == Id ).Select( rc2 => rc2.ClaimType );
            PermitList = permitList.OrderBy( o => o ).ToList();
        }
    }
}
