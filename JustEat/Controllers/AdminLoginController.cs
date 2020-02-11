using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JustEat.Repository;
using JustEat.Models;
using JustEat.Interface;

namespace JustEat.Controllers
{
    public class AdminLoginController : Controller
    {
        IRepository<Admin> repo = new AdminRepository(new JustEatEntities());

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost, ActionName("Login")]
        public ActionResult Signin(int? AdminInput, string Password, FormCollection form)
        {
            if (repo.GetAll().Where(c => c.Phone == AdminInput && c.Password == form["Password"] || c.Email == form["AdminInput"].ToString() && c.Password == form["Password"].ToString()).FirstOrDefault() != null)
            {
                var adm = repo.GetAll().Where(c => c.Phone == AdminInput && c.Password == form["Password"] || c.Email == form["AdminInput"].ToString() && c.Password == form["Password"].ToString()).FirstOrDefault();
                Session["adminid"] = adm.Id;
                Session["adminname"] = adm.Name;
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                ViewData["invalidlogin"] = "Invalid Login!Try Again";
                return View("Login");
            }
        }

    }
}