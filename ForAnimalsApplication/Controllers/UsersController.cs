using ForAnimalsApplication.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ForAnimalsApplication.Controllers
{
    public class UsersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Users
        public ActionResult Index()
        {
            ViewBag.UsersList = db.Users.OrderBy(u => u.UserName).ToList();

            return View();
        }

        [HttpGet]
        public ActionResult Details(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return HttpNotFound("Lipseste id-ul contului!");
            }
            ApplicationUser user = db.Users
            .Include("Roles")
            .FirstOrDefault(u => u.Id.Equals(id));
            if (user != null)
            {
                ViewBag.UserRole = db.Roles
              .Find(user.Roles.First().RoleId).Name;
                return View(user);
            }

            return HttpNotFound("Nu se poate gasi contul cu id-ul " + id.ToString() + " !");
        }



        [HttpGet]

        public ActionResult DisplayRoles()
        {
            var roles = db.Roles.OrderBy(r => r.Name).ToList();
            return View(roles);
        }

        [HttpGet]
        public ActionResult AddRole()
        {
            IdentityRole role = new IdentityRole();
            return View(role);
        }


        [HttpPost]
        public ActionResult AddRole(IdentityRole roleReq)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Roles.Add(roleReq);
                    db.SaveChanges();
                    return RedirectToAction("DisplayRoles");
                }
                return View(roleReq);
            }
            catch (Exception e)
            {
                return View(roleReq);
            }
        }

        public ActionResult AddUserToRole()
        {
            //get all users
            //get all roles
            //create selectlist and pass using viewBag
            var users = db.Users.OrderBy(u => u.UserName).ToList();
            var roles = db.Roles.OrderBy(u => u.Name).ToList();

            ViewBag.Users = new SelectList(users, "Id", "UserName");
            ViewBag.Roles = new SelectList(roles, "Name", "Name");
            return View();
        }

        [HttpPost]
        public ActionResult AddUserToRole(UserViewModel userRole)
        {
            //find user from userRole.UserId
            //assign role to user
            //redirect to index

            ApplicationUser user = db.Users.Find(userRole.UserId);

            try
            {
                if (TryUpdateModel(user))
                {
                    var um = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                    foreach (var r in db.Roles.ToList())
                    {
                        um.RemoveFromRole(user.Id, r.Name);
                    }
                    um.AddToRole(user.Id, userRole.RoleName);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return View(userRole);
            }
        }
    }
}