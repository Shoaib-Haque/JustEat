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
    public class DeliveryManLoginController : Controller
    {
        IRepository<DeliveryMan> repo = new DeliveryManRepository(new JustEatEntities());
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost, ActionName("Login")]
        public ActionResult Signin(int? DeliveryManInput, string Password, FormCollection form)
        {
            if (repo.GetAll().Where(c => c.ContactNo == DeliveryManInput && c.Password == form["Password"] || c.Email == form["DeliveryManInput"].ToString() && c.Password == form["Password"].ToString() && c.Status == "Active").FirstOrDefault() != null)
            {
                var adm = repo.GetAll().Where(c => c.ContactNo == DeliveryManInput && c.Password == form["Password"] || c.Email == form["DeliveryManInput"].ToString() && c.Password == form["Password"].ToString() && c.Status == "Active").FirstOrDefault();
                Session["deliverymanid"] = adm.Id;
                Session["deliverymanname"] = adm.Name;
                
                return RedirectToAction("DashBoard", "DeliveryManOrder");
            }
            else
            {
                ViewData["invalidlogin"] = "Invalid Login!Try Again";
                return View("Index");
            }
        }
	}
}