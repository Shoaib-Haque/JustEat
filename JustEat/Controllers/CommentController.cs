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
    public class CommentController : Controller
    {
        IRepository<Comment> repo = new CommentRepository(new JustEatEntities());
        IRepository<Food> prorepo = new FoodRepository(new JustEatEntities());

        public ActionResult Post(string comment, int? pid, string ur)
        {
            
            repo.Insert(new Comment { Comment1 = comment, FoodId = (int)pid, CustomerId = (int)Session["customerid"], CustomerName = Session["customername"].ToString() });
            return Redirect(ur);
        }

        public ActionResult Index()
        {
            return View(repo.GetAll().Where(c=>c.Status == "Reported"));
        }


        public ActionResult Delete(int? id,string url)
        {
            if(id.HasValue == true)
            {
                int? cid = id;
                repo.Delete((int)cid);
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public ActionResult Deny(int? id, string url)
        {
            //return Content(id.ToString());
            if (id.HasValue == true)
            {
                int? cid = id;
                Comment c = repo.GetById((int)cid);
                c.Status = null;
                repo.Update(c);
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public ActionResult Report(int? id, string ur)
        {
            Comment c = repo.GetById((int)id);
            c.Status = "Reported";
            repo.Update(c);
            return Redirect(ur);
        }
	}
}