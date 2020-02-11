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
    public class RestaurentReportController : RestaurentAccessController
    {
        IRepository<Transaction> tranrepo = new TransactionRepository(new JustEatEntities());
        IRepository<Food> repo = new FoodRepository(new JustEatEntities());
        IRepository<Trending> trendrepo = new TrendingRepository(new JustEatEntities());
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TotalProfit()
        {
            
            int total = tranrepo.GetAll().Where(t => t.RestaurentId == (int)Session["restaurentid"]).Sum(t=>t.RestaurentProfit);
            TempData["totalprofit"] = total.ToString();
            return View(tranrepo.GetAll().Where(p=>p.RestaurentId == (int)Session["restaurentid"]).OrderByDescending(t=>t.DateTime).ToList());
        }

        public ActionResult TodayProfit()
        {
            int total = tranrepo.GetAll().Where(p => p.RestaurentId == (int)Session["restaurentid"] && p.Date == DateTime.Now.Date).Sum(t => t.RestaurentProfit);
            TempData["todayprofit"] = total.ToString();
            return View(tranrepo.GetAll().Where(p => p.RestaurentId == (int)Session["restaurentid"] && p.Date == DateTime.Now.Date).OrderByDescending(t => t.DateTime).ToList());
        }

        public ActionResult SevenDayProfit()
        {
            int total = tranrepo.GetAll().Where(p => p.RestaurentId == (int)Session["restaurentid"] && p.Date >= DateTime.Now.Date.AddDays(-7) && p.Date <= DateTime.Now.Date).Sum(t => t.RestaurentProfit);
            TempData["totalsevenprofit"] = total.ToString();
            return View(tranrepo.GetAll().Where(p => p.RestaurentId == (int)Session["restaurentid"] && p.Date >= DateTime.Now.Date.AddDays(-7) && p.Date <= DateTime.Now.Date).OrderByDescending(t => t.DateTime).ToList());
        }

        public ActionResult BestSelling()
        {
            return View(trendrepo.GetAll().Where(t=>t.RestaurentId == (int)Session["restaurentid"]).OrderByDescending(t=>t.Count).ToList());
        }

        public ActionResult ByDate()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ReportByDate(DateTime Date)
        {
            int total = tranrepo.GetAll().Where(t => t.RestaurentId == (int)Session["restaurentid"] && t.Date == Date.Date).Sum(t => t.RestaurentProfit);
            TempData["totalprofitdate"] = total.ToString();
            return View(tranrepo.GetAll().Where(t => t.RestaurentId == (int)Session["restaurentid"] && t.Date == Date.Date).OrderByDescending(t=>t.DateTime));
        }



	}
}