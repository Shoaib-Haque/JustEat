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
    public class RestaurentLoginController : Controller
    {
        IRepository<Restaurent> repo = new RestaurentRepository(new JustEatEntities());
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost, ActionName("Login")]
        public ActionResult Signin(int? RestaurentInput, string Password, FormCollection form)
        {
            if (repo.GetAll().Where(c => c.ContactNo == RestaurentInput && c.Password == form["Password"] || c.Email == form["RestaurentInput"].ToString() && c.Password == form["Password"].ToString() && c.Status == "Active").FirstOrDefault() != null)
            {
                var adm = repo.GetAll().Where(c => c.ContactNo == RestaurentInput && c.Password == form["Password"] || c.Email == form["RestaurentInput"].ToString() && c.Password == form["Password"].ToString() && c.Status == "Active").FirstOrDefault();
                Session["restaurentid"] = adm.Id;
                Session["restaurentname"] = adm.Name;
                return RedirectToAction("DashBoard", "Order");
            }
            else
            {
                ViewData["invalidlogin"] = "Invalid Login!Try Again";
                return View("Index");
            }
        }
	}
}