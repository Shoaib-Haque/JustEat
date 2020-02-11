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
    public class DeliveryManController : Controller
    {
        IRepository<DeliveryMan> repo = new DeliveryManRepository(new JustEatEntities());
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult SignedUp()
        {
            return View();
        }


        [HttpPost, ActionName("Create")]
        public ActionResult Signup(FormCollection form, HttpPostedFileBase Image, DeliveryMan a)
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
                    string address = a.Address.ToString();
                    string email = a.Email.ToString();
                    int contactno = (int)a.ContactNo;
                    string password = a.Password.ToString();

                    string pic = System.IO.Path.GetFileName(Image.FileName);
                    string path = System.IO.Path.Combine(Server.MapPath("~/Image"), pic);
                    Image.SaveAs(path);
                    TempData["Image"] = pic.ToString();

                    repo.Insert(new DeliveryMan
                    {
                        Name = name,
                        Address = address,
                        Email = email,
                        ContactNo = contactno,
                        Password = password,
                        Image = TempData["Image"].ToString(),
                        Status = null,
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