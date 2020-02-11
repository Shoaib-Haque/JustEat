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
    public class AreaController : AdminAccessController
    {
        IRepository<Area> repo = new AreaRepository(new JustEatEntities());
        public ActionResult Index()
        {
            return View(repo.GetAll().OrderBy(a=>a.AreaName));
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost,ActionName("Create")]
        public ActionResult FinalCreate(Area a)
        {
            if (ModelState.IsValid)
            {
                repo.Insert(a);
                return RedirectToAction("Index");
            }
            else
            {
                return View("Create");
            }
            
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            return View(repo.GetById(id));
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            return View(repo.GetById(id));
        }

        [HttpPost, ActionName("Edit")]
        public ActionResult FinalEdit(Area a)
        {
            if (ModelState.IsValid)
            {
                repo.Update(a);
                return RedirectToAction("Index");
            }
            else
            {
                return View("Edit");
            }
            
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            return View(repo.GetById(id));
        }

        [HttpPost,ActionName("Delete")]
        public ActionResult FinalDelete(int id)
        {
            repo.Delete(id);
            return RedirectToAction("Index");
        }
	}
}