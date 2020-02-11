using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JustEat.Interface;
using JustEat.Models;
using JustEat.Repository;

namespace JustEat.Controllers
{
    public class CustomerLoginController : Controller
    {
        IRepository<Customer> repo = new CustomerRepository(new JustEatEntities());

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost, ActionName("Login")]
        public ActionResult Signin(int? CustomerInput, string Password, FormCollection form)
        {
            if (repo.GetAll().Where(c => c.Contact == CustomerInput && c.Password == form["Password"] || c.Email == form["CustomerInput"].ToString() && c.Password == form["Password"].ToString()).FirstOrDefault() != null)
            {
                var cus = repo.GetAll().Where(c => c.Contact == CustomerInput && c.Password == form["Password"] || c.Email == form["CustomerInput"].ToString() && c.Password == form["Password"].ToString()).FirstOrDefault(); 
                Session["customerid"] = cus.Id;
                Session["customername"] = cus.Name;
                Session["customerarea"] = cus.AreaId;
                return RedirectToAction("Index", "CustomerFood");
            }
            else
            {
                ViewData["invalidlogin"] = "Invalid Login!Try Again";
                return View("Login");
            }
        }
	}
	
}