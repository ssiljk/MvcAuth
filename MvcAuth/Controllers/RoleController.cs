using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MvcAuth.DataAccess;
using MvcAuth.Models;
using MvcAuth.CustomAuthentication;
using System.Web.Security;

namespace MvcAuth.Controllers
{
    //[CustomAuthorize(Roles = "Administrador")]
    public class RoleController : Controller
    {
        private AuthenticationDB db = new AuthenticationDB();
        private CustomRole roleProv = new CustomRole();

        // GET: Roles
        public ActionResult Index()
        {
            return View(db.Roles.ToList());
        }

        // GET: Roles/Create
        public ActionResult Create()
        {
            return View();
        }
        [HttpGet]
        public ActionResult DisplayAllRoles()
        {
            return View(db.Roles.ToList());
            //IEnumerable<string> roleNameList;
            //roleNameList = roleProv.GetAllRoles();
            //return View(roleNameList);

        }
        // POST: Roles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RoleId,RoleName")] Role role)
        {
            if (ModelState.IsValid)
            {
                if (roleProv.RoleExists(role.RoleName))
                {
                   // ModelState.AddModelError("Error", "Rolename ya existe");
                    ModelState.AddModelError("", "Rolename ya existe");
                    return View(role);
                }
                else
                {
                    //db.Roles.Add(role);
                    //db.SaveChanges();
                    roleProv.CreateRole(role.RoleName);
                }
                //return RedirectToAction("RoleIndex", "Account");
            }
            return View(role);
        }

        // GET: Roles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Role role = db.Roles.Find(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }

        // POST: Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Role role = db.Roles.Find(id);
            db.Roles.Remove(role);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Roles/RoleUsers/5
        public ActionResult RoleUsers(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Role role = db.Roles.Find(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }

        //// GET: Roles/RoleUsersAdd/5
        //public ActionResult RoleUsersAdd(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    RoleUser roleUser = new RoleUser();
        //    roleUser.Role = db.Roles.Find(id);
        //    roleUser.User = new User();
        //    ViewBag.UserId = new SelectList(db.Users, "UserId", "Username");
        //    return View(roleUser);
        //}

        //// POST: Roles/RoleUsersAdd/5
        //[HttpPost, ActionName("RoleUsersAdd")]
        ////[ValidateAntiForgeryToken]
        //public ActionResult RoleUsersAdd([Bind(Include = "UserId")]RoleUser roleUser)
        //{
        //    Role role = db.Roles.Find(roleUser.Role.RoleId);
        //    role.Users.Add(roleUser.User);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        [HttpGet]
        public ActionResult RoleAddToUser()
        {
            AssignRoleVM objvm = new AssignRoleVM();
            //objvm.RolesList = GetAll_Roles();
            objvm.RolesList = GetAll_Roles();
            objvm.UserList = GetAll_Users();
            return View(objvm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RoleAddToUser(AssignRoleVM objvm)
        {
            bool statusRoleAddToUser = false;
            string messageRoleAddToUser = string.Empty;

            if (objvm.RoleName == "0")
            {
                ModelState.AddModelError("RoleName", "Seleccione un Perfil");
            }
            if (objvm.UserName == "0")
            {
                ModelState.AddModelError("UserName", "Seleccione un Usuario");
            }
            if (ModelState.IsValid)
            {
                string[] userList = new string[] { objvm.UserName };
                string[] roleList = new string[] { objvm.RoleName };
                roleProv.AddUsersToRoles(userList, roleList);
                messageRoleAddToUser = "Se asocio el Usuario al Rol";
                statusRoleAddToUser = true;
            }
            else
            {
                messageRoleAddToUser = "Error asociando el usuario al Rol";
            }
            ViewBag.Message = messageRoleAddToUser;
            ViewBag.Status = statusRoleAddToUser;
            return View(objvm);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [NonAction]
        public List<SelectListItem> GetAll_Roles()
        {
            //string[] listRoles = new string[] { };
            //List<SelectListItem> listrole = new List<SelectListItem>();
            //listrole.Add(new SelectListItem { Text = "select", Value = "0" });
            //listRoles = roleProv.GetAllRoles();

            //    foreach (var item in listRoles)
            //    {
            //        listrole.Add(new SelectListItem { Text = item, Value = item });
            //    }

            //return listrole;
            //*
            //List<SelectListItem> listrole = new List<SelectListItem>();
            //listrole.Add(new SelectListItem { Text = "select", Value = "0" });
            //foreach (var item in db.Roles)
            //{
            //    listrole.Add(new SelectListItem { Text = item.RoleName, Value = item.RoleName });
            //}
            //return listrole;
            //*
            List<SelectListItem> listrole = new List<SelectListItem>();
            listrole.Add(new SelectListItem { Text = "select", Value = "0" });
            string[] result = db.Roles.Select(r => r.RoleName).ToArray();
            for (int i = 0; i < result.Length; i++)
                listrole.Add(new SelectListItem { Text = result[i], Value = result[i] });
            return listrole;
        }

        [NonAction]
        public List<SelectListItem> GetAll_Users()
        {
            List<SelectListItem> listuser = new List<SelectListItem>();
            listuser.Add(new SelectListItem { Text = "Select", Value = "0" });
                foreach (var item in db.Users)
                {
                    listuser.Add(new SelectListItem { Text = item.Username, Value = item.Username });
                }
            return listuser;
        }


    }
}