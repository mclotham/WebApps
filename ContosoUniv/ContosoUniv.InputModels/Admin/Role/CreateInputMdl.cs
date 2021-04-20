using ContosoUniv.Authorization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ContosoUniv.InputModels.Admin.Role
{
    public class CreateInputMdl
    {
        [Required]
        [Display( Name = "Role Name" )]
        public string Name { get; set; }

        public List<SelectListItem> PermitList { get; set; }
        public string SelectedPermits { get; set; }

        public void LoadPermitList( string selectedItems = "" )
        {
            selectedItems ??= "";
            var selectedList = selectedItems.Split( new char[] { ';' } );
            PermitList = Permits.GetAllPermits().Select( p => new SelectListItem
            {
                Text = p,
                Value = p.Replace( '.', '_' ),
                Selected = selectedList.Contains( p.Replace( '.', '_' ) )
            } ).ToList();
        }
    }
}
