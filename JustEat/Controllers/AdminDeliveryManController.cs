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
    public class AdminDeliveryManController : AdminAccessController
    {
        IRepository<DeliveryMan> repo = new DeliveryManRepository(new JustEatEntities());
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Registration()
        {
            return View(repo.GetAll().Where(r => r.Status == null));
        }

        public ActionResult Approve(int? id)
        {
            DeliveryMan o = repo.GetById((int)id);
            o.Status = "Active";
            repo.Update(o);
            return RedirectToAction("Registration");
        }
	}
}