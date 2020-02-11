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
    public class ReportController : AdminAccessController
    {
        IRepository<Transaction> tranrepo = new TransactionRepository(new JustEatEntities());
        IRepository<Trending> trendrepo = new TrendingRepository(new JustEatEntities());
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TotalProfit()
        {

            int total = tranrepo.GetAll().Sum(t => t.JEProfit);
            TempData["totalprofit"] = total.ToString();
            return View(tranrepo.GetAll().ToList().OrderByDescending(t=>t.DateTime));
        }

        public ActionResult TodayProfit()
        {
            int total = tranrepo.GetAll().Where(p =>  p.Date == DateTime.Now.Date).Sum(t => t.JEProfit);
            TempData["todayprofit"] = total.ToString();
            return View(tranrepo.GetAll().Where(p => p.Date == DateTime.Now.Date).OrderByDescending(t => t.DateTime).ToList());
        }

        public ActionResult ByDate()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ReportByDate(DateTime Date)
        {
            int total = tranrepo.GetAll().Where(t => t.Date == Date.Date).Sum(t => t.JEProfit);
            TempData["totalprofitdate"] = total.ToString();
            return View(tranrepo.GetAll().Where(t => t.Date == Date.Date).OrderByDescending(t => t.DateTime));
        }

        public ActionResult SevenDayProfit()
        {
            int total = tranrepo.GetAll().Where(p => p.Date >= DateTime.Now.Date.AddDays(-7) && p.Date <= DateTime.Now.Date).Sum(t => t.JEProfit);
            TempData["totalsevenprofit"] = total.ToString();
            return View(tranrepo.GetAll().Where(p => p.Date >= DateTime.Now.Date.AddDays(-7) && p.Date <= DateTime.Now.Date).OrderByDescending(t => t.DateTime).ToList());
        }

       
	}
}