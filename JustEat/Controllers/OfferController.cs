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
    public class OfferController : RestaurentAccessController
    {
        IRepository<Food> repo = new FoodRepository(new JustEatEntities());
        public ActionResult FreeDeliveryAllMenu()
        {
            return View();
        }

        public ActionResult AllowFreeDeliveryAllMenu()
        {
            var f = repo.GetAll().Where(fa=>fa.RestaurentId == (int)Session["restaurentid"]).ToList();
            foreach(var item in f)
            {
                item.DeliveryFeeId = (int)2;
                repo.Update(item);
            }
            return View();
        }


        public ActionResult CancelFreeDeliveryAllMenu()
        {
            return View();
        }

        public ActionResult FinalCancelFreeDeliveryAllMenu()
        {
            var f = repo.GetAll().Where(fa => fa.RestaurentId == (int)Session["restaurentid"]).ToList();
            foreach (var item in f)
            {
                item.DeliveryFeeId = (int)1;
                repo.Update(item);
            }
            return View();
        }

        public ActionResult DiscountAllMenu()
        {
            return View();
        }

        public ActionResult AllowedDiscountAllMenu()
        {
            return View();
        }

        [HttpPost, ActionName("AllowDiscountAllMenu")]
        public ActionResult FinalAllowDiscountAllMenu(int? Rate)
        {
            var f = repo.GetAll().Where(fa => fa.RestaurentId == (int)Session["restaurentid"]).ToList();
            foreach (var item in f)
            {
                float oldprice = (float)item.UnitPrice;
                float newprice = (float)(oldprice - (oldprice/100)*Rate);
                
                item.UnitPrice = (int)newprice;
                item.LastPrice = oldprice;
                repo.Update(item);
            }
            return RedirectToAction("AllowedDiscountAllMenu");
        }
	}
}