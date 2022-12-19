using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UserManagement.Models;
using System.Data.Entity;

namespace UserManagement.Controllers
{
    public class HomeController : Controller
    {
        private USMDBEntities2 db = new USMDBEntities2();

        public ActionResult Index()
        {
            var userTables = db.UserTables.Include(u => u.RoleTable);
            return View(userTables.ToList());
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]

        public ActionResult LoginUser(String exampleInputEmail, string exampleInputPassword)
        {
            try
            {
                if (exampleInputEmail != null && exampleInputPassword != null)
                {
                    var finduser = db.UserTables.Where(u => u.Email == exampleInputEmail && u.Password == exampleInputPassword).ToList();
                    if (finduser.Count() == 1)
                    {
                        Session["UserID"] = finduser[0].UserID;
                        Session["Name"] = finduser[0].Name;
                        Session["RoleID"] = finduser[0].RoleID;
                        
                        Session["Password"] = finduser[0].Password;
                        Session["Phone"] = finduser[0].Phone;
                        Session["AddOn"] = finduser[0].AddOn;
                        Session["Signup"] = finduser[0].Signup;

                    }
                    string url = string.Empty;
                    if (finduser[0].RoleID == 2)
                    {
                        return RedirectToAction("About");
                    }
                    else if (finduser[0].RoleID == 1)
                    {
                        url = "About";
                    }
                    else
                    {
                        url = "About";
                    }
                    return RedirectToAction(url);
                }
            }
            catch (Exception ex)
            {
                Session["UserID"] = string.Empty;
                Session["RoleID"] = string.Empty;
                Session["Name"] = string.Empty;
                Session["Email"] = string.Empty;
                Session["Password"] = string.Empty;
                Session["Phone"] = string.Empty;
                Session["AddOn"] = string.Empty;
                Session["Signup"] = string.Empty;
                ViewBag.Message = "Email or Password is Incorrect";
            }
            return View("Login");
        }
        [HttpGet]
        public ActionResult Registration()
        {
            ViewBag.RoleID = new SelectList(db.RoleTables, "RoleID", "Role");
            return View();
        }
        [HttpPost]
        public ActionResult UserRegistration([Bind(Include = "UserID,Name,Email,Phone,IsActive,AddOn,Signup,RoleID,Password")] UserTable userTable)
        {
            if (ModelState.IsValid)
            {
                db.UserTables.Add(userTable);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.RoleID = new SelectList(db.RoleTables, "RoleID", "Role", userTable.RoleID);
            return View(userTable);
        }
        public ActionResult Create()
        {
            ViewBag.RoleID = new SelectList(db.RoleTables, "RoleID", "Role");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserID,Name,Email,Phone,IsActive,AddOn,Signup,RoleID,Password")] UserTable userTable)
        {
            if (ModelState.IsValid)
            {
                db.UserTables.Add(userTable);
                db.SaveChanges();
                return RedirectToAction("Login");
            }

            ViewBag.RoleID = new SelectList(db.RoleTables, "RoleID", "Role", userTable.RoleID);
            return View(userTable);
        }
        public ActionResult Logout()
        {
            Session["UserID"] = string.Empty;
            Session["RoleID"] = string.Empty;
            Session["Name"] = string.Empty;
            Session["Email"] = string.Empty;
            Session["Password"] = string.Empty;
            Session["Phone"] = string.Empty;
            Session["AddOn"] = string.Empty;
            Session["Signup"] = string.Empty;

            return RedirectToAction("Login");
        }




        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}