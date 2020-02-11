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
    public class DeliveryManOrderController : DeliveryManAccessController
    {
        IRepository<Order> repo = new OrderRepository(new JustEatEntities());
        IRepository<Customer> cusrepo = new CustomerRepository(new JustEatEntities());
        IRepository<Notification> notrepo = new NotificationRepository(new JustEatEntities());
        IRepository<Transaction> tranrepo = new TransactionRepository(new JustEatEntities());
        public ActionResult Index()
        {
            return View(repo.GetAll().Where(o=>o.DeliveryManId == (int)Session["deliverymanid"]).OrderByDescending(p=>p.PlacedTime));
        }

        public ActionResult DashBoard()
        {
            return View(repo.GetAll().Where(o => o.Status == "Looking For Rider").OrderByDescending(p=>p.PlacedTime));
        }

        public ActionResult Income()
        {
            int total = tranrepo.GetAll().Where(t => t.DeliveryManId == (int)Session["deliverymanid"]).Sum(t => t.DeliveryManProfit);
            TempData["totalprofit"] = total.ToString();
            return View(tranrepo.GetAll().Where(o => o.DeliveryManId == (int)Session["deliverymanid"]).OrderByDescending(p => p.DateTime));
        }

        [HttpGet]
        public ActionResult Details(int? id, int? cid)
        {
            Customer customer = cusrepo.GetById((int)cid);
            Session["customerid"] = customer.Id;
            Session["customername"] = customer.Name;
            Session["customercontact"] = customer.Contact;
            Session["customeraddressbook"] = customer.Address;

            return View(repo.GetById((int)id));
        }

        public ActionResult Pick(int? id)
        {
            Order o = repo.GetById((int)id);
            o.Status = "Picked";
            
            o.DeliveryManId = (int)Session["deliverymanid"];
            repo.Update(o);
            return RedirectToAction("Index");
        }

        public ActionResult OnRide(int? id)
        {
            Order o = repo.GetById((int)id);
            int customerid = (int)o.CustomerId;
            o.Status = "OnRide";
            repo.Update(o);
            notrepo.Insert(new Notification { CustomerId = customerid, DateTime = DateTime.Now, OrderId = (int)id, Notice = "Your Food is Ready And On Ride..!" });
            return RedirectToAction("Index");
        }
	}
}