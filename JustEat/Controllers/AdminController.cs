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
    public class AdminController : Controller
    {
        IRepository<Admin> repo = new AdminRepository(new JustEatEntities());
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Create()
        {
            return View();
        }

        public ActionResult SignedUp()
        {
            return View();
        }


        [HttpPost, ActionName("Create")]
        public ActionResult Signup(Admin a)
        {

            if (ModelState.IsValid)
            {
                if (repo.GetAll().Where(c => c.Email == a.Email.ToString()).FirstOrDefault() != null)
                {
                    ViewData["invalidaddmsg"] = "This Email Got An Id";
                    return View("Create");
                }
                if (repo.GetAll().Where(c => c.Phone == (int)a.Phone).FirstOrDefault() != null)
                {
                    ViewData["invalidaddmsg"] = "This Phone Number Got An Id";
                    return View("Create");
                }
                else
                {
                    repo.Insert(a);
                    return RedirectToAction("SignedUp");
                }

            }
            else
            {
                return View("Create");
            }
        }
	}
}