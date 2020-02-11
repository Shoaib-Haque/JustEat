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
    public class FoodController : RestaurentAccessController
    {
        IRepository<Food> repo = new FoodRepository(new JustEatEntities());
        IRepository<Comment> comrepo = new CommentRepository(new JustEatEntities());
        public ActionResult Index()
        {
            return View(repo.GetAll().Where(f=>f.RestaurentId == (int)Session["restaurentid"]).ToList());
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost, ActionName("Create")]
        public ActionResult FinalCreate(FormCollection form, HttpPostedFileBase Image, Food pr)
        {
            if (ModelState.IsValid)
            {
                float unitpricel = (float)pr.UnitPrice;
                string pic = System.IO.Path.GetFileName(Image.FileName);
                string path = System.IO.Path.Combine(Server.MapPath("~/Image"), pic);
                Image.SaveAs(path);
                repo.Insert(new Food
                {
                    Title = pr.Title.ToString(),
                    Details = pr.Details.ToString(),
                    Unit = (int)1,
                    UnitPrice = (float)unitpricel,
                    Image = pic.ToString(),
                    RestaurentId = (int)Session["restaurentid"],
                    DeliveryFeeId = (int)1,

                });
                return RedirectToAction("Index");
            }
            else
            {
                return View("Create");
            }

        }

        public ActionResult Details(int id)
        {
            int i = 0;
            var cat = comrepo.GetAll().Where(c => c.FoodId == (int)id).OrderByDescending(c => c.Id);
            int count = comrepo.GetAll().Where(c => c.FoodId == (int)id).Count();
            Session["countcomment"] = count;
            foreach (var item in cat)
            {                
                Session["commentid" + i.ToString()] = item.Id;
                Session["commentst" + i.ToString()] = item.Status;
                Session["comment" + i.ToString()] = item.Comment1;
                Session["c" + i.ToString()] = item.CustomerName;
                i++;
            }
            Food p = repo.GetById(id);
            return View(p);
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            Food p = repo.GetById((int)id);
            return View(p);
        }

        [HttpPost, ActionName("Edit")]
        public ActionResult FinalEdit(FormCollection form, HttpPostedFileBase Image, int id, int? Unit, int UnitPrice)
        {
            if (ModelState.IsValid)
            {
                if (Image == null)
                {
                    Food p = repo.GetById(id);
                    float oldprice = (float)p.UnitPrice;
                    p.Title = form["Title"];
                    p.UnitPrice = UnitPrice;
                    p.Details = form["Details"];
                    if (oldprice > (float)UnitPrice)
                    {
                        p.LastPrice = oldprice;
                    }
                    else if (oldprice < (float)UnitPrice)
                    {
                        p.LastPrice = null;
                    }
                    repo.Update(p);
                    return RedirectToAction("Index");
                }

                else
                {
                    string pic = System.IO.Path.GetFileName(Image.FileName);
                    string path = System.IO.Path.Combine(Server.MapPath("~/Image"), pic);
                    Image.SaveAs(path);

                    Food p = repo.GetById(id);
                    float oldprice = (float)p.UnitPrice;
                    p.Title = form["Title"];
                    p.UnitPrice = UnitPrice;
                    p.Details = form["Details"];
                    p.Image = pic.ToString();
                    if (oldprice > (float)UnitPrice)
                    {
                        p.LastPrice = oldprice;
                    }
                    else if (oldprice < (float)UnitPrice)
                    {
                        p.LastPrice = null;
                    }
                    repo.Update(p);
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return View("Edit");
            }

            
        }

        [HttpGet]
        public ActionResult FreeDelivery(int? id)
        {
            Food p = repo.GetById((int)id);
            return View(p);
        }

        [HttpGet]
        public ActionResult AllowFreeDelivery(int? id)
        {
            Food p = repo.GetById((int)id);
            p.DeliveryFeeId = (int)2;
            repo.Update(p);
            return View();
        }

        [HttpGet]
        public ActionResult CancelFreeDelivery(int? id)
        {
            Food p = repo.GetById((int)id);
            return View(p);
        }

        [HttpGet]
        public ActionResult FinalCancelFreeDelivery(int? id)
        {
            Food p = repo.GetById((int)id);
            p.DeliveryFeeId = (int)1;
            repo.Update(p);
            return View();
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            Food p = repo.GetById((int)id);
            return View(p);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult FinalDelete(int? id)
        {
            repo.Delete((int)id);
            return RedirectToAction("Index");
        }
	}
}