using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JustEat.Controllers
{
    public class LogoutController : Controller
    {
        public ActionResult Index()
        {
            Session["customerid"] = "";
            Session["customername"] = "";
            Session["adminid"] = "";
            Session["adminname"] = "";
            Session["restaurentid"] = "";
            Session["restaurentname"] = "";
            Session["deliverymanid"] = "";
            Session["delivermanname"] = "";
            return RedirectToAction("Index", "Home");
        }
	}
}