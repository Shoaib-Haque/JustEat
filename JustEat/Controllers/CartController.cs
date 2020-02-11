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
    public class CartController : CustomerAccessController
    {
        IRepository<Cart> repo = new CartRepository(new JustEatEntities());
        IRepository<Food> foodrepo = new FoodRepository(new JustEatEntities());
        IRepository<Customer> cusrepo = new CustomerRepository(new JustEatEntities());

        public ActionResult Index()
        {
            var cartl = repo.GetAll().Where(c => c.CustomerId == (int)Session["customerid"] && c.OrderId == null);
            Cart cat = repo.GetAll().Where(c => c.CustomerId == (int)Session["customerid"] && c.OrderId == null).FirstOrDefault();
            if(cat != null)
            {
                int? resid = (int)cat.RestaurentId;
                TempData["resid"] = resid.ToString();
            }
            
            int deliveryfee = 0;
            foreach(var item in cartl)
            {
                if(item.Food.DeliveryFeeId == 1)
                {
                    deliveryfee = 60;
                    TempData["deliveryfee"] = deliveryfee.ToString();
                    break;
                }
                else
                {
                    deliveryfee = 0;
                    TempData["deliveryfee"] = deliveryfee.ToString();
                }
            }
            Double? total = repo.GetAll().Where(c => c.CustomerId == (int)Session["customerid"] && c.OrderId == null).Sum(c => c.TotalPrice);
            TempData["totalammount"] = total;
            var customer = cusrepo.GetAll().Where(c => c.Id == (int)Session["customerid"]).Select(c => c.Discount);
            foreach (var cus in customer)
            {
                if (cus != null)
                {
                    Double? discountrate = (Double?)cus.Rate;
                    TempData["discountrate"] = discountrate;
                    float? discount = ((float)total / 100) * (float)discountrate;
                    TempData["discount"] = discount;
                    float finalammount = (float)total - (float)discount + deliveryfee ;
                    TempData["finalammount"] = finalammount;
                }
                else
                {
                    TempData["discountrate"] = 0;
                    TempData["discount"] = 0;
                    TempData["finalammount"] = (float)total  + deliveryfee;
                }
            }
            return View(repo.GetAll().Where(c => c.CustomerId == (int)Session["customerid"] && c.OrderId == null));
        }

        public ActionResult Increment(int? id, int? pid, int? rid, Double? unitprice, string title, string ur)
        {
            if (id != null)
            {
                if (repo.GetAll().Where(c => c.CustomerId == (int)Session["customerid"] && c.OrderId == null && c.RestaurentId == (int)rid).FirstOrDefault() != null)
                {
                    if (repo.GetAll().Where(c => c.CustomerId == (int)Session["customerid"] && c.FoodId == pid && c.OrderId == null).FirstOrDefault() != null)
                    {
                        var cart = repo.GetAll().Where(c => c.CustomerId == (int)Session["customerid"] && c.FoodId == pid && c.OrderId == null).FirstOrDefault();
                        cart.Quantity = cart.Quantity + 1;
                        cart.TotalPrice = cart.TotalPrice + (Double)unitprice;
                        repo.Update(cart);

                        var Food = foodrepo.GetById((int)pid);
                        return Redirect(ur);
                    }
                    else
                    {
                        repo.Insert(new Cart { FoodName = title.ToString(), Quantity = 1, FoodId = (int)pid, CustomerId = (int)Session["customerid"], TotalPrice = (Double)unitprice, UnitPrice = (Double)unitprice, RestaurentId = (int)rid });

                        var Food = foodrepo.GetById((int)pid);
                        return Redirect(ur);
                    }
                }

                else
                {
                    if (repo.GetAll().Where(c => c.CustomerId == (int)Session["customerid"] && c.OrderId == null) != null)
                    {
                        var cartcount = repo.GetAll().Where(c => c.CustomerId == (int)Session["customerid"] && c.OrderId == null);
                        foreach (var item in cartcount)
                        {
                            repo.Delete(item.Id);
                        }
                        repo.Insert(new Cart { FoodName = title.ToString(), Quantity = 1, FoodId = (int)pid, CustomerId = (int)Session["customerid"], TotalPrice = (Double)unitprice, UnitPrice = (Double)unitprice, RestaurentId = (int)rid });
                        return Redirect(ur);
                    }

                    else
                    {
                        repo.Insert(new Cart { FoodName = title.ToString(), Quantity = 1, FoodId = (int)pid, CustomerId = (int)Session["customerid"], TotalPrice = (Double)unitprice, UnitPrice = (Double)unitprice, RestaurentId = (int)rid });
                        return Redirect(ur);
                    }

                }

            }
            else
            {
                return Redirect(ur);
            }
        }

        public ActionResult Decrement(int? id, int? pid, Double? unitprice, string ur)
        {
            if (id != null)
            {
                if (repo.GetAll().Where(c => c.CustomerId == (int)Session["customerid"] && c.FoodId == pid && c.OrderId == null).FirstOrDefault() != null)
                {
                    var cart = repo.GetAll().Where(c => c.CustomerId == (int)Session["customerid"] && c.FoodId == pid && c.OrderId == null).FirstOrDefault();
                    cart.Quantity = cart.Quantity - 1;
                    cart.TotalPrice = cart.TotalPrice - (Double)unitprice;
                    repo.Update(cart);
                    if (repo.GetAll().Where(c => c.CustomerId == (int)Session["customerid"] && c.FoodId == pid && c.OrderId == null).FirstOrDefault().Quantity == 0)
                    {
                        repo.Delete((int)id);
                    }
                    var Food = foodrepo.GetById((int)pid);
                    
                    return Redirect(ur);
                }
                else
                {
                    return Redirect(ur);
                }
            }
            else
            {
                return Redirect(ur);
            }
        }


        public ActionResult Delete(int? id, int? pid, Double? quantity, string ur)
        {
            if (id != null)
            {
                if (repo.GetAll().Where(c => c.CustomerId == (int)Session["customerid"] && c.FoodId == pid && c.OrderId == null).FirstOrDefault() != null)
                {
                    Double? cartid = repo.GetAll().Where(c => c.CustomerId == (int)Session["customerid"] && c.FoodId == pid && c.OrderId == null).FirstOrDefault().Id;
                    repo.Delete((int)cartid);
                    var Food = foodrepo.GetById((int)pid);
                    
                    return Redirect(ur);
                }
                else
                {
                    return Redirect(ur);
                }
            }
            else
            {
                return Redirect(ur);
            }
        }
	}
}