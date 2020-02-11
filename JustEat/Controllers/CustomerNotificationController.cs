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
    public class CustomerNotificationController : CustomerAccessController
    {
        IRepository<Notification> repo = new NotificationRepository(new JustEatEntities());
        public ActionResult Index()
        {
            return View(repo.GetAll().Where(n => n.CustomerId == (int)Session["customerid"]).OrderByDescending(c=>c.DateTime).ToList());
        }

        [HttpGet]
        public void OrderPlaceNotice(int oid)
        {
            repo.Insert(new Notification { Notice = "Your Order Has Been Placed!", CustomerId = (int)Session["customerid"], DateTime = DateTime.Now, OrderId = Convert.ToInt32(oid) });
        }
    }
}