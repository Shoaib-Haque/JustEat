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
    public class CustomerDiscountController : CustomerAccessController
    {
        IRepository<Discount> repo = new DiscountRepository(new JustEatEntities());
        IRepository<Customer> cusrepo = new CustomerRepository(new JustEatEntities());
        public ActionResult Nodiscount()
        {
            return View();
        }
        public ActionResult Index()
        {
            int? disid = cusrepo.GetById((int)Session["customerid"]).DiscountId;
            if (disid != null)
            {
                var disitem = repo.GetAll().Where(d => d.Id == (int)disid);
                foreach (var item in disitem)
                {
                    int? discountrate = (int)item.Rate;
                    TempData["customerdiscountrate"] = discountrate;
                }
                return View();
            }
            else
            {
                return RedirectToAction("Nodiscount");
            }
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost, ActionName("Add")]
        public ActionResult FinalAdd(FormCollection form, string ur)
        {
            if (repo.GetAll().Where(d => d.Code == form["Code"].ToString()).FirstOrDefault() != null)
            {
                var customer = cusrepo.GetAll().Where(c => c.Id == Convert.ToInt32(Session["customerid"]) || c.Name == Session["customername"].ToString()).FirstOrDefault();
                int disc = repo.GetAll().Where(d => d.Code == form["Code"].ToString()).Select(d => d.Id).FirstOrDefault();
                customer.DiscountId = (int)disc;
                cusrepo.Update(customer);
                return RedirectToAction("Index");
            }
            else
            {
                ViewData["nodiscount"] = "This Code Doesn't Have Any Discount!";
                return View("Add");
            }
        }
	}
}