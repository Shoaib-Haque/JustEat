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
    public class CustomerOrderController : CustomerAccessController
    {
        IRepository<Order> repo = new OrderRepository(new JustEatEntities());
        IRepository<Cart> cartrepo = new CartRepository(new JustEatEntities());
        IRepository<Food> foodrepo = new FoodRepository(new JustEatEntities());
        IRepository<Customer> cusrepo = new CustomerRepository(new JustEatEntities());
        IRepository<Notification> notrepo = new NotificationRepository(new JustEatEntities());
        public ActionResult Index()
        {
            return View(repo.GetAll().Where(o => o.CustomerId == (int)Session["customerid"]).OrderByDescending(c=>c.PlacedTime));
        }

        [HttpGet]
        public ActionResult ThankYou()
        {
            return View();
        }

        [HttpPost, ActionName("PlaceOrder")]
        public ActionResult OrderPlace(FormCollection form, DateTime? DeliveryDay)
        {
            if (ModelState.IsValid)
            {
                double fammount = Convert.ToDouble(Request.Params["FinalAmmount"]);
                double tammount = Convert.ToDouble(Request.Params["TotalAmmount"]);
                double dammount = Convert.ToDouble(Request.Params["Discount"]);
                double dfee = Convert.ToDouble(Request.Params["DeliveryFee"]);
                int resid = Convert.ToInt32(Request.Params["resid"].ToString());
                repo.Insert(new Order { PlacedTime = DateTime.Now, Ammount = (double)tammount, 
                                        Discount = (double)dammount, FinalAmmount = (double)fammount,
                                        CustomerId = (int)Session["customerid"], Status = "Pending", 
                                        DeliveryFee = (int)dfee, RestaurentId = (int)resid });
                var lastinsertedId = repo.GetAll().Select(o => o.Id).Last();
                var c = cartrepo.GetAll().Where(u => u.CustomerId == (int)Session["customerid"] && u.OrderId == null);
                foreach (var item in c)
                {
                    item.OrderId = lastinsertedId;
                    cartrepo.Update(item);
                }
                notrepo.Insert(new Notification { Notice = "Your Order Has Been Placed!",  CustomerId = (int)Session["customerid"], DateTime = DateTime.Now, OrderId = Convert.ToInt32(lastinsertedId) });
                return RedirectToAction("ThankYou");
            }
            else
            {
                return View("PlaceOrder");
            }

        }

        public ActionResult Details(int? id)
        {
            if (id != null)
            {
                var cart = cartrepo.GetAll().Where(c => c.OrderId == (int)id);
                return View(cart);
            }
            else
            {
                return RedirectToAction("Index", "ProductForCustomer");
            }
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id != null)
            {
                var order = repo.GetById((int)id);
                return View();
            }
            else
            {
                return RedirectToAction("Index", "ProductForCustomer");
            }

        }

        [HttpPost, ActionName("Delete")]

        public ActionResult FinalDelete(int? id)
        {
            if (id != null)
            {
                var cartid = cartrepo.GetAll().Where(c => c.OrderId == (int)id).Select(c => c.Id);
                var nid = notrepo.GetAll().Where(n => n.OrderId == (int)id).Select(n => n.Id);
                foreach (var itemid in nid)
                {
                    notrepo.Delete((int)itemid);
                }
                
                foreach (var cid in cartid)
                {
                    cartrepo.Delete((int)cid);
                }

                
                repo.Delete((int)id);

                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index", "ProductForCustomer");
            }
        }
	}
}