using MvcAuth.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcAuth.Models
{
    public class RoleUser
    {
        public Role Role { get; set; }
        public User User { get; set; }
    }
}