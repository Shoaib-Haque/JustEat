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
    public class OrderController : RestaurentAccessController
    {
        IRepository<Order> repo = new OrderRepository(new JustEatEntities());
        IRepository<Cart> catrepo = new CartRepository(new JustEatEntities());

        IRepository<Notification> notrepo = new NotificationRepository(new JustEatEntities());
        IRepository<Customer> cusrepo = new CustomerRepository(new JustEatEntities());
        IRepository<Transaction> tranrepo = new TransactionRepository(new JustEatEntities());
        IRepository<Trending> trendrepo = new TrendingRepository(new JustEatEntities());
        public ActionResult Index()
        {
            return View(repo.GetAll().Where(o=>o.RestaurentId == (int)Session["restaurentid"]).OrderByDescending(c=>c.PlacedTime));
        }

        public ActionResult DashBoard()
        {
            return View(repo.GetAll().Where(o => o.RestaurentId == (int)Session["restaurentid"] && o.Status == "Pending").OrderByDescending(c=>c.PlacedTime).ToList());
        }

        [HttpGet]
        public ActionResult Details(int? id, int? cid)
        {
            int i = 0;
            var cat = catrepo.GetAll().Where(c => c.OrderId == (int)id);
            int count = cat.Count();
            Session["countcart"] = count;
            foreach (var item in cat)
            {
                i++;
                Session["ocpid" + i.ToString()] = item.FoodId;
                Session["ocpt" + i.ToString()] = item.FoodName;
                Session["ocup" + i.ToString()] = item.UnitPrice;
                Session["ocq" + i.ToString()] = item.Quantity;
                Session["octp" + i.ToString()] = item.TotalPrice;
            }

            Customer customer = cusrepo.GetById((int)cid);
            Session["customerid"] = customer.Id;
            Session["customername"] = customer.Name;
            Session["customercontact"] = customer.Contact;
            Session["customeraddressbook"] = customer.Address;

            return View(repo.GetById((int)id));
        }

        public ActionResult Cancel(int? id)
        {
            var cartid = catrepo.GetAll().Where(c => c.OrderId == (int)id).Select(c => c.Id);
            var nid = notrepo.GetAll().Where(n => n.OrderId == (int)id).Select(n => n.Id);
            foreach (var itemid in nid)
            {
                notrepo.Delete((int)itemid);
            }
            foreach (var cid in cartid)
            {
                catrepo.Delete((int)cid);
            }

            Order o = repo.GetById((int)id);
            int customerid = (int)o.CustomerId;
            repo.Delete((int)id);
            //notrepo.Insert(new Notification { CustomerId = customerid, DateTime = DateTime.Now, OrderId = (int)id, Notice = "Your Order Has Been Canceled Due To unavoidable circumstances..!" });
            return RedirectToAction("Index");
        }



        [HttpGet]
        public ActionResult OnProcess(int? id)
        {
            Order o = repo.GetById((int)id);
            int customerid = (int)o.CustomerId;
            o.Status = "OnProcess";
            repo.Update(o);
            notrepo.Insert(new Notification { CustomerId = customerid, DateTime = DateTime.Now, OrderId = (int)id, Notice = "Your Order on Process..!" });
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult LookForRider(int? id)
        {
            Order o = repo.GetById((int)id);
            o.Status = "Looking For Rider";
            repo.Update(o);
            return RedirectToAction("DashBoard");
        }

        [HttpGet]
        public ActionResult Ready(int? id)
        {
            Order o = repo.GetById((int)id);
            o.Status = "Ready";
            repo.Update(o);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Delivered(int? id)
        {
            Order o = repo.GetById((int)id);
            int customerid = (int)o.CustomerId;
            o.Status = "Delivered";
            o.DeliveredTime = DateTime.Now;
            repo.Update(o);
            notrepo.Insert(new Notification { CustomerId = customerid, DateTime = DateTime.Now, OrderId = (int)id, Notice = "Your Order Has Been Delivered..!" });
            Order ot = repo.GetById((int)id);
            DateTime dt = DateTime.Now;
            string time = dt.ToString("hh:mm:ss tt");
            string t = DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":"+ DateTime.Now.Second.ToString();
            int respro = 0;
            int jepro = 0;

            if (o.Discount == 0 && o.DeliveryFee == 60)
            {
                respro = (int)((o.Ammount / 100) * 80);
                jepro = (int)(((o.Ammount / 100) * 20) + 10);
            }

            else if (o.Discount == 0 && o.DeliveryFee == 0)
            {
                respro = (int)((((o.Ammount ) / 100) * 80) - 60);
                jepro = (int)(((o.Ammount / 100) * 20) + 10);
            }

            else if (o.Discount > 0 && o.DeliveryFee == 60)
            {
                respro = (int)(((o.Ammount / 100) * 80));
                jepro = (int)((((o.Ammount  ) / 100) * 20) - (int)o.Discount + 10);
            }

            else if (o.Discount > 0 && o.DeliveryFee == 0)
            {
                respro = (int)((((o.Ammount ) / 100) * 80) - 60 );
                jepro = (int)(((((o.Ammount ) / 100) * 20) - (int)o.Discount + 10));
            }


            tranrepo.Insert(new Transaction
            {
                DateTime = DateTime.Now,
                Date = DateTime.Now.Date,
                Time = DateTime.Now.TimeOfDay,
                RestaurentProfit = (int)respro,
                JEProfit = (int)jepro,
                DeliveryManProfit = (int)50,
                CustomerId = (int)o.CustomerId,
                RestaurentId = (int)o.RestaurentId,
                OrderId = (int)o.Id,
                DeliveryManId = (int)o.DeliveryManId
            });

            var carts = catrepo.GetAll().Where(c => c.OrderId == (int)id);
            foreach(var item in carts)
            {
                if(trendrepo.GetAll().Where(tc=>tc.FoodId == item.FoodId).FirstOrDefault() != null)
                {
                    var items = trendrepo.GetAll().Where(tc => tc.FoodId == item.FoodId).FirstOrDefault();
                    items.Count = items.Count + (int)item.Quantity;
                    
                    trendrepo.Update(items);
                }
                else
                {
                    trendrepo.Insert(new Trending
                    {
                        FoodId = item.FoodId,
                        RestaurentId = (int)item.RestaurentId,
                        Count = item.Quantity,
                        Date = DateTime.Now.Date,
                    });
                }
                
            }
            
            return RedirectToAction("Index");
        }
	}
}