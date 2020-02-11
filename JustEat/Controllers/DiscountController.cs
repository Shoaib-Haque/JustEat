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
    public class DiscountController : AdminAccessController
    {
        IRepository<Discount> repo = new DiscountRepository(new JustEatEntities());
        IRepository<Customer> cusrepo = new CustomerRepository(new JustEatEntities());
        public ActionResult Index()
        {
            return View(repo.GetAll());
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost, ActionName("Create")]
        public ActionResult FinalCreate(Discount d)
        {
            if (ModelState.IsValid)
            {
                repo.Insert(d);
                return RedirectToAction("Index");
            }
            else
            {
                return View("Create");
            }
            
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            Discount d = repo.GetById((int)id);
            return View(d);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult FinalDelete(int? id)
        {
            var customer = cusrepo.GetAll().Where(c => c.DiscountId == (int)id);

            foreach (var item in customer)
            {
                
                item.DiscountId = null;
                cusrepo.Update(item);
            }

            repo.Delete((int)id);
            return RedirectToAction("Index");
        }
	}
}