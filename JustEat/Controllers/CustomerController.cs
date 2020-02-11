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
    public class CustomerController : Controller
    {
        IRepository<Customer> repo = new CustomerRepository(new JustEatEntities());
        IRepository<Area> arearepo = new AreaRepository(new JustEatEntities());
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Registration()
        {
            int i = 0;
            var ar = arearepo.GetAll().OrderBy(c => c.AreaName);
            int count = arearepo.GetAll().Count();
            Session["countarea"] = count;
            foreach (var item in ar)
            {     
                Session["id" + i.ToString()] = item.Id;
                Session["areaname" + i.ToString()] = item.AreaName;
                i++;
            }
            return View();
        }

        public ActionResult SignedUp()
        {
            return View();
        }

        [HttpPost, ActionName("Registration")]
        public ActionResult SignUp(Customer p)
        {
            if (ModelState.IsValid)
            {
                if (repo.GetAll().Where(c => c.Email == p.Email.ToString()).FirstOrDefault() != null)
                {
                    ViewData["invalidregmsg"] = "This Email Got An Id";
                    return View("Registration");
                }
                if (repo.GetAll().Where(c => c.Contact == (int)p.Contact).FirstOrDefault() != null)
                {
                    ViewData["invalidregmsg"] = "This Phone Number Got An Id";
                    return View("Registration");
                }
                else
                {
                    repo.Insert(p);
                    return RedirectToAction("SignedUp");
                }
            }
            else
            {
                return View("Registration");
            }
        }
	}
}