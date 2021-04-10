using ContosoUniv.WebApp.Authorization;
using HtmlTags;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ContosoUniv.WebApp.Models
{
    public class MenuMdl
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly string _username;

        public class MenuItem
        {
            public List<MenuItem> SubMenuItems = new();
            public string DisplayText { get; set; }
            public string Route { get; set; }
            public string Permit { get; set; }
            public bool AlwaysAllow { get; set; }
        }

        private MenuItem TopMenu = new();
        private HtmlTag _htmlList;

        public string HtmlListItems { get; set; }

        public MenuMdl( UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, string username )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _username = username;
        }

        public void CreateHtmlMenu()
        {
            LoadAllMenuItems();
            RemoveUnauthorizedItems();
            CreateHtmlList();
        }

        public void LoadAllMenuItems()
        {
            MenuItem menuItemL1;
            MenuItem menuItemL2;
            TopMenu.SubMenuItems = new();

            menuItemL1 = new MenuItem { DisplayText = "Admin" };
            TopMenu.SubMenuItems.Add( menuItemL1 );
            menuItemL1.SubMenuItems.Add( new MenuItem { DisplayText = "Create User", Route = "/User/Create", Permit = Permits.Admin.UserCreate } );
            menuItemL2 = new MenuItem { DisplayText = "Roles" };
            menuItemL1.SubMenuItems.Add( menuItemL2 );
            menuItemL2.SubMenuItems.Add( new MenuItem { DisplayText = "Manage Roles", Route = "/Role/Index", Permit = Permits.Admin.RoleManage } );
            menuItemL2.SubMenuItems.Add( new MenuItem { DisplayText = "Create Role", Route = "/Role/Create", Permit = Permits.Admin.RoleCreate } );

            menuItemL1 = new MenuItem { DisplayText = "Instructor" };
            TopMenu.SubMenuItems.Add( menuItemL1 );
            menuItemL1.SubMenuItems.Add( new MenuItem { DisplayText = "View Courses", Route = "/Instructor/ViewCourses", Permit = Permits.Instructor.ViewCourses } );

            menuItemL1 = new MenuItem { DisplayText = "Student" };
            TopMenu.SubMenuItems.Add( menuItemL1 );
            menuItemL1.SubMenuItems.Add( new MenuItem { DisplayText = "View Grades", Route = "/Student/ViewGrades", Permit = Permits.Student.ViewGrades } );
        }

        public void RemoveUnauthorizedItems()
        {
            var permitList = new List<string>();
            var idUser = Task.Run( async () => await _userManager.FindByNameAsync( _username ) ).Result;
            IList<string> roles = Task.Run( async () => await _userManager.GetRolesAsync( idUser ) ).Result;
            foreach ( var roleName in roles )
            {
                IdentityRole role = Task.Run( async () => await _roleManager.FindByNameAsync( roleName ) ).Result;
                IList<Claim> roleClaims = Task.Run( async () => await _roleManager.GetClaimsAsync( role ) ).Result;
                foreach ( Claim roleClaim in roleClaims )
                    if ( !permitList.Contains( roleClaim.Type ) )
                        permitList.Add( roleClaim.Type );
            }

            RemoveUnauthorizedItems( TopMenu, permitList );
        }

        public void RemoveUnauthorizedItems( MenuItem menuNode, List<string> permitList )
        {
            var removeList = new List<MenuItem>();

            foreach ( var menuItem in menuNode.SubMenuItems )
            {
                if ( menuItem.SubMenuItems.Any() )
                {
                    RemoveUnauthorizedItems( menuItem, permitList );
                    if ( !menuItem.SubMenuItems.Any() )
                        removeList.Add( menuItem );
                }
                else if ( !permitList.Contains( menuItem.Permit ) && !menuItem.AlwaysAllow )
                {
                    removeList.Add( menuItem );
                }
            }

            foreach ( var menuItem in removeList )
                menuNode.SubMenuItems.Remove( menuItem );
        }

        public void CreateHtmlList()
        {
            HtmlListItems = string.Empty;

            foreach ( var menuItem in TopMenu.SubMenuItems )
            {
                var listItem = new HtmlTag( "li" );
                if ( menuItem.SubMenuItems.Any() )
                {
                    // Add top level list item for dropdown
//                    var listItem = new HtmlTag( "li" );
                    listItem.AddClasses( new string[] { "nav-item", "dropdown" } );

                    // Add top level anchor tag
                    var dda = new HtmlTag( "a" );
                    dda.AddClasses( new string[] { "nav-link", "dropdown-item" } );
                    dda.Attr( "href", "#" );
                    dda.Attr( "id", "navbarDropdownMenuLink" );
                    dda.Attr( "data-toggle", "dropdown" );
                    dda.Attr( "aria-haspopup", "true" );
                    dda.Attr( "aria-expanded", "false" );
                    dda.Text( menuItem.DisplayText );
                    listItem.Children.Add( dda );

                    // Add dropdown menu list
                    var ddml = new HtmlTag( "ul" );
                    ddml.AddClass( "dropdown-menu" );
                    ddml.Attr( "aria-labelledby", "navbarDropdownMenuLink" );
                    listItem.Children.Add( ddml );

                    // Add submenu items
                    AddSubMenuItems( menuItem, ddml );
                }
                else
                {
                    // Add top level list item for link
//                    var listItem = new HtmlTag( "li" );
                    listItem.AddClass( "nav-item" );

                    // Add top level anchor tag
                    var dda = new HtmlTag( "a" );
                    dda.AddClass( "nav-link" );
                    dda.Attr( "href", menuItem.Route );
                    dda.Text( menuItem.DisplayText );
                    listItem.Children.Add( dda );
                }

                HtmlListItems += listItem.ToHtmlString();
            }
        }

        public void AddSubMenuItems( MenuItem subMenu, HtmlTag parentList )
        {
            foreach( var menuItem in subMenu.SubMenuItems )
            {
                // Add top level list item for link
                var listItem = new HtmlTag( "li" );

                if ( menuItem.SubMenuItems.Any() )
                {
                    // Add top level anchor tag
                    var dda = new HtmlTag( "a" );
                    dda.AddClasses( new string[] { "dropdown-item", "dropdown-toggle" } );
                    dda.Attr( "href", "#" );
                    dda.Text( menuItem.DisplayText );
                    listItem.Children.Add( dda );

                    // Add dropdown menu list
                    var ddml = new HtmlTag( "ul" );
                    ddml.AddClass( "dropdown-menu" );
                    listItem.Children.Add( ddml );

                    // Add submenu items
                    AddSubMenuItems( menuItem, ddml );
                }
                else
                {
                    // Add top level anchor tag
                    var dda = new HtmlTag( "a" );
                    dda.AddClass( "dropdown-item" );
                    dda.Attr( "href", menuItem.Route );
                    dda.Text( menuItem.DisplayText );
                    listItem.Children.Add( dda );
                }

                parentList.Children.Add( listItem );
            }
        }
    }
}
