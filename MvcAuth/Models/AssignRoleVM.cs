using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcAuth.Models
{
    public class AssignRoleVM
    {
        [Required(ErrorMessage = " Select Role Name")]
        public string RoleName { get; set; }

        [Required(ErrorMessage = "Select UserName")]
        public string UserName { get; set; }

        public List<SelectListItem> UserList { get; set; }

        public List<SelectListItem> RolesList { get; set; }
    }
}