using ContosoUniv.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoUniv.InputModels.Admin.Models.Role
{
    public class EditInputMdl
    {
        [Display( Name = "Role Name" )]
        public string Name { get; set; }
        public string Id { get; set; }
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
