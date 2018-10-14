using MvcAuth.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace MvcAuth.CustomAuthentication
{
    public class CustomRole : RoleProvider
    {
        /// <summary>  
        ///   
        /// </summary>  
        /// <param name="username"></param>  
        /// <param name="roleName"></param>  
        /// <returns></returns>  
        public override bool IsUserInRole(string username, string roleName)
        {
            var userRoles = GetRolesForUser(username);
            return userRoles.Contains(roleName);
        }

        /// <summary>  
        ///   
        /// </summary>  
        /// <param name="username"></param>  
        /// <returns></returns>  
        public override string[] GetRolesForUser(string username)
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return null;
            }

            var userRoles = new string[] { };

            using (AuthenticationDB dbContext = new AuthenticationDB())
            {
                var selectedUser = (from us in dbContext.Users.Include("Roles")
                                    where string.Compare(us.Username, username, StringComparison.OrdinalIgnoreCase) == 0
                                    select us).FirstOrDefault();


                if (selectedUser != null)
                {
                    userRoles = new[] { selectedUser.Roles.Select(r => r.RoleName).ToString() };
                }

                return userRoles.ToArray();
            }


        }



        #region Overrides of Role Provider  

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public override void AddUsersToRoles(string[] userNames, string[] roleNames)
        {
          //throw new NotImplementedException();
          for(int i=0; i<userNames.Length; i++)
          {
                for (int j = 0; j < roleNames.Length; j++)
                {
                    using (AuthenticationDB db = new AuthenticationDB())
                    {
                        string rr = roleNames[j];
                        string uu = userNames[i];
                        Role selectedRole = db.Roles.FirstOrDefault(r => r.RoleName == rr);
                        User selectedUser = db.Users.FirstOrDefault(u => u.Username == uu);
                        selectedRole.Users.Add(selectedUser);
                        db.SaveChanges();
                    }
                }
          }
          
        }

        //public override void AddUserToRole(string username, string roleName)
        //{
        //    throw new NotImplementedException();
        //}


        public override void CreateRole(string roleName)
        {
            Role role = new Role();
            role.RoleName = roleName;
            // throw new NotImplementedException();
            using (AuthenticationDB db = new AuthenticationDB())
            {
                db.Roles.Add(role);
                db.SaveChanges();
            }
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            //using (AuthenticationDB db = new AuthenticationDB())
            //{
            //    string[] result = db.Roles.Select(r => r.RoleName).ToArray();
            //    return result;
            //}
            throw new NotImplementedException();
            //var userRoles = new string[] { };
            //using (AuthenticationDB db = new AuthenticationDB())
            //{
            //    int i;
            //    i = 0;
            //    foreach (var item in db.Roles.Select(r => r.RoleName).ToString())
            //    {
            //        userRoles[i] = db.Roles.Select(r => r.RoleName).ToString();
            //        i++;
            //    }
            //    return userRoles;
            //}
            
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }


        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            //throw new NotImplementedException();
            using (AuthenticationDB db = new AuthenticationDB())
            {
                return db.Roles.Any(r => r.RoleName == roleName);
                //var result = (from t in db.Roles
                //              where t.RoleName == roleName
                //              select roleName).Any();

            }
        }

        #endregion
    }
}
