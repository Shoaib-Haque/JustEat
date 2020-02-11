using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JustEat.Controllers
{
    public class RestaurentAccessController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string id = Session["restaurentname"] as string;
            if (string.IsNullOrEmpty(id))
            {
                Response.Redirect("/Home");
            }
        }
	}
}