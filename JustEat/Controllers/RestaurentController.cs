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
    public class RestaurentController : Controller
    {
        IRepository<Restaurent> repo = new RestaurentRepository(new JustEatEntities());
        IRepository<Area> arearepo = new AreaRepository(new JustEatEntities());
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            int i = 0;
            var ar = arearepo.GetAll().OrderBy(c => c.AreaName);
            int count = arearepo.GetAll().Count();
            Session["countarea"] = count;
            foreach (var item in ar)
            {
                Session["id" + i.ToString()] = item.Id;
                Session["areaname" + i.ToString()] = item.AreaName;
                i++;
            }
            return View();
        }

        public ActionResult SignedUp()
        {
            return View();
        }


        [HttpPost, ActionName("Create")]
        public ActionResult Signup(FormCollection form, HttpPostedFileBase Image, Restaurent a)
        {

            if (ModelState.IsValid)
            {
                if (repo.GetAll().Where(c => c.Email == a.Email.ToString()).FirstOrDefault() != null)
                {
                    ViewData["invalidaddmsg"] = "This Email Got An Id";
                    return View("Create");
                }
                if (repo.GetAll().Where(c => c.ContactNo == (int)a.ContactNo).FirstOrDefault() != null)
                {
                    ViewData["invalidaddmsg"] = "This ContactNo Number Got An Id";
                    return View("Create");
                }
                else
                {
                    string name = a.Name.ToString();
                    string address = a.AddressBook.ToString();
                    string email = a.Email.ToString();
                    int contactno = (int)a.ContactNo;
                    string password = a.Password.ToString();
                    int aid = (int)a.AreaId;

                    string pic = System.IO.Path.GetFileName(Image.FileName);
                    string path = System.IO.Path.Combine(Server.MapPath("~/Image"), pic);
                    Image.SaveAs(path);
                    TempData["Image"] = pic.ToString();

                    repo.Insert(new Restaurent
                    {
                        Name = name,
                        AddressBook = address,
                        AreaId = aid,
                        Email = email,
                        ContactNo = contactno,
                        Password = password,
                        Image = TempData["Image"].ToString(),
                    });
                    //repo.Insert(a);
                    return RedirectToAction("SignedUp");
                }

            }
            else
            {
                return View("Create");
            }
        }
	}
}