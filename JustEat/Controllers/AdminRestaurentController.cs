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
    public class AdminRestaurentController : AdminAccessController
    {
        

        IRepository<Restaurent> repo = new RestaurentRepository(new JustEatEntities());
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Registration()
        {
            return View(repo.GetAll().Where(r => r.Status == null));
        }

        [HttpGet]
        public ActionResult Approve(int? id)
        {
            Restaurent o = repo.GetById((int)id);
            o.Status = "Active";
            repo.Update(o);
            return RedirectToAction("Registration");
        }
	}
}