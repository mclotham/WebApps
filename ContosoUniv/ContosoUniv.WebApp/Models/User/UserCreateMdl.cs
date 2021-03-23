using ContosoUniv.Data;
using ContosoUniv.WebApp.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoUniv.WebApp.Models.User
{
    public class UserCreateMdl
    {
        [Required]
        [Display( Name = "Role Name" )]
        public string RoleName { get; set; }

        public List<SelectListItem> RoleList { get; set; }
        public List<SelectListItem> PersonList { get; set; }
        public string SelectedPerson { get; set; }
        public string SelectedRoles { get; set; }

        public void LoadPersonList( ContosoUnivContext dbContext, string selectedPerson = "" )
        {
            selectedPerson = selectedPerson ?? "";
            using ( dbContext )
            {
                var persons =
                    from person in dbContext.Person
                    join idPerson in dbContext.IdentityPerson on person.PersonId equals idPerson.PersonId
                    into identityPersonTbl
                    from idPerson2 in identityPersonTbl.DefaultIfEmpty()
                    where idPerson2 == null
                    select new { person, idPerson2 };
            }
        }

        public void LoadRoleList( ContosoUnivContext dbContext, string selectedRole )
        {

        }
    }
}
