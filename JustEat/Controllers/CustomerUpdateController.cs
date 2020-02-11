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
    public class CustomerUpdateController : CustomerAccessController
    {
        IRepository<Customer> repo = new CustomerRepository(new JustEatEntities());
        IRepository<Area> arearepo = new AreaRepository(new JustEatEntities());
        public ActionResult AreaUpdate()
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
            return View(repo.GetById((int)Session["customerid"]));
            
        }

        public ActionResult AreaUpdated()
        {
            return View();
        }

        [HttpPost,ActionName("AreaUpdate")]
        public ActionResult FinalAreaUpdate(int Areaid, int Id)
        {
            Customer c = repo.GetById(Id);
            c.AreaId = Areaid;
            repo.Update(c);
            Session["customerarea"] = Areaid;
            return RedirectToAction("AreaUpdated");
        }

        public ActionResult AdressUpdated()
        {
            return View();
        }
        
        public ActionResult AddressUpdate()
        {
            return View(repo.GetById((int)Session["customerid"]));
        }

        [HttpPost, ActionName("AddressUpdate")]
        public ActionResult FinalAddressUpdate(string Address, int Id)
        {
            Customer c = repo.GetById(Id);
            c.Address = Address;
            repo.Update(c);
            return RedirectToAction("AdressUpdated");
        }
	}
}