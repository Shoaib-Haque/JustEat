using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JustEat.Interface;
using JustEat.Models;
using JustEat.Repository;
using System.Web.Routing;

namespace JustEat.Controllers
{
    public class CustomerFoodController : CustomerAccessController
    {
        IRepository<Food> repo = new FoodRepository(new JustEatEntities());
        IRepository<Comment> comrepo = new CommentRepository(new JustEatEntities());
        IRepository<Rating> ratingrepo = new RatingsRepository(new JustEatEntities());
        IRepository<Restaurent> resrepo = new RestaurentRepository(new JustEatEntities());
        // GET: /FoodForCustomer/
        public ActionResult Index()
        {
            return View(repo.GetAll().Where(f=>f.Restaurent.AreaId == (int)Session["customerarea"]).ToList());
        }

        public ActionResult Details(int? id)
        {
            int i = 0;
            var cat = comrepo.GetAll().Where(c => c.FoodId == (int)id).OrderByDescending(c => c.Id);
            int count = comrepo.GetAll().Where(c => c.FoodId == (int)id).Count();
            Session["countcomment"] = count;
            foreach (var item in cat)
            {
                i++;
                Session["comment" + i.ToString()] = item.Comment1;
                Session["c" + i.ToString()] = item.CustomerName;
            }
            int? totalrat = ratingrepo.GetAll().Where(c => c.FoodId == (int)id).Sum(c => c.Rating1);
            int ratcount = ratingrepo.GetAll().Where(c => c.FoodId == (int)id).Count();
            float rat = (float)totalrat / ratcount;
            if (rat > 0)
            {
                Session["rat"] = rat.ToString() + "*";
            }
            else
            {
                Session["rat"] = "0*";
            }

            return View(repo.GetById((int)id));
        }


        public ViewResult SearchFood(string searchName)
        {
            var data = repo.GetAll().Where(p => p.Title.Contains(searchName) || p.Title.StartsWith(searchName.ToUpper())
                                              || p.Title.StartsWith(searchName.ToLower()) || p.Title.Contains(searchName.ToLower())
                                              || p.Title.Contains(searchName.ToUpper()) || p.Title.StartsWith(searchName)
                                              || p.Title.StartsWith(searchName.ToLower()) || p.Title.StartsWith(searchName.ToUpper())
                                               && p.RestaurentId == (int)Session["customerarea"])
                                              .ToList();
            return View(data);
        }

        public ActionResult SearchRestaurent(string searchName)
        {
            var data = resrepo.GetAll().Where(p => p.Name.Contains(searchName) || p.Name.StartsWith(searchName.ToUpper())
                                              || p.Name.StartsWith(searchName.ToLower()) || p.Name.Contains(searchName.ToLower())
                                              || p.Name.Contains(searchName.ToUpper()) 
                                              && p.Id == (int)Session["customerarea"]).ToList();
            return View(data);
        }

        //public ViewResult SearchRestaurentByArea(string searchName)
        //{
        //    int id = arearepo.GetAll().Where(p => p.AreaName.Contains(searchName) || p.AreaName.StartsWith(searchName.ToUpper())
        //                                      || p.AreaName.StartsWith(searchName.ToLower()) || p.AreaName.Contains(searchName.ToLower())
        //                                      || p.AreaName.Contains(searchName.ToUpper())).Select(p => p.Id).FirstOrDefault();
        //    var data = resrepo.GetAll().Where(p => p.AreaId == (int)id).ToList();
        //    return View(data);
        //}

        public ViewResult NoResult()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Searching(string searchName)
        {
            var data1 = resrepo.GetAll().Where(p => p.Name.StartsWith(searchName) || p.Name.StartsWith(searchName.ToLower()) || p.Name.StartsWith(searchName.ToUpper()) && p.Id == (int)Session["customerarea"]).FirstOrDefault();
            var data2 = repo.GetAll().Where(p => p.Title.StartsWith(searchName) || p.Title.StartsWith(searchName.ToLower()) || p.Title.StartsWith(searchName.ToUpper()) && p.RestaurentId == (int)Session["customerarea"]).FirstOrDefault();
            //var data3 = arearepo.GetAll().Where(p => p.AreaName.StartsWith(searchName) || p.AreaName.StartsWith(searchName.ToLower()) || p.AreaName.StartsWith(searchName.ToUpper())).FirstOrDefault();
            if (data1 != null)
            {
                return RedirectToAction("SearchRestaurent", new RouteValueDictionary(
                       new { controller = "CustomerFood", action = "SearchRestaurent", searchName = searchName }));
            }
            else if (data2 != null)
            {
                return RedirectToAction("SearchFood", new RouteValueDictionary(
                       new { controller = "CustomerFood", action = "SearchFood", searchName = searchName }));
            }
            //else if (data3 != null)
            //{
            //    return RedirectToAction("SearchRestaurentByArea", new RouteValueDictionary(
            //           new { controller = "Home", action = "SearchRestaurentByArea", searchName = searchName }));
            //}
            else
            {
                return RedirectToAction("NoResult");
            }
        }

        public ActionResult SearchAuto(string term)
        {
            List<string> reslist = new List<string>();
            var res1 = repo.GetAll().Where(p => (p.Title.Contains(term) || p.Title.StartsWith(term.ToUpper())
                                              || p.Title.StartsWith(term.ToLower()) || p.Title.Contains(term.ToLower())
                                              || p.Title.Contains(term.ToUpper())) && p.Restaurent.AreaId == (int)Session["customerarea"])
                                              .Select(p => p.Title).ToList();
            var res2 = resrepo.GetAll().Where(p => (p.Name.Contains(term) || p.Name.StartsWith(term.ToUpper())
                                              || p.Name.StartsWith(term.ToLower()) || p.Name.Contains(term.ToLower())
                                              || p.Name.Contains(term.ToUpper())) && p.AreaId == (int)Session["customerarea"])
                                              .Select(p => p.Name).ToList();
            //var res3 = arearepo.GetAll().Where(p => p.AreaName.Contains(term) || p.AreaName.StartsWith(term.ToUpper())
            //                                  || p.AreaName.StartsWith(term.ToLower()) || p.AreaName.Contains(term.ToLower())
            //                                  || p.AreaName.Contains(term.ToUpper()))
            //                                  .Select(p => p.AreaName).ToList();
            foreach (var item in res1)
            {
                reslist.Add(item);
            }
            foreach (var item in res2)
            {
                reslist.Add(item);
            }
            //foreach (var item in res3)
            //{
            //    reslist.Add(item);
            //}
            return Json(reslist, JsonRequestBehavior.AllowGet);
        }

        public ViewResult FoodByRestaurent(int id)
        {
            var data = repo.GetAll().Where(p => p.RestaurentId == (int)id).ToList();
            return View(data);
        }
	}
}