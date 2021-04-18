using ContosoUniv.Data;
using ContosoUniv.Authorization;
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
        [Display( Name = "User Login" )]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        public List<SelectListItem> RoleList { get; set; }
        public List<SelectListItem> PersonList { get; set; }
        public string SelectedPerson { get; set; }
        public string SelectedRoles { get; set; }

        public void LoadPersonList( ContosoUnivContext dbContext, string selectedPerson = "" )
        {
            selectedPerson ??= "";

            var persons =
                from person in dbContext.Person
                join idPerson in dbContext.IdentityPerson on person.PersonId equals idPerson.PersonId
                into identityPersonTbl
                from idPerson2 in identityPersonTbl.DefaultIfEmpty()
                where idPerson2 == null
                select new
                {
                    Id = person.PersonId,
                    person.FirstName,
                    person.LastName,
                    person.Discriminator
                };

            PersonList = persons
                .Select( person => new SelectListItem
                    {
                        Value = person.Id.ToString(),
                        Text = person.LastName + ", " + person.FirstName + " ( " + person.Discriminator + " )",
                        Selected = person.Id.ToString() == selectedPerson
                    } )
                .OrderBy( o => o.Text )
                .ToList();
        }

        public void LoadRoleList( ContosoUnivContext dbContext, string selectedRoles )
        {
            selectedRoles ??= "";
            var selectedRoleList = selectedRoles.Split( new char[] { ';' } );

            var roles = dbContext.AspNetRoles.Select( role => role.Name ).ToList();

            RoleList = roles
                .Select( role => new SelectListItem
                    {
                        Value = role,
                        Text = role,
                        Selected = selectedRoleList.Contains( role )
                    } )
                .OrderBy( o => o.Text )
                .ToList();
        }
    }
}
