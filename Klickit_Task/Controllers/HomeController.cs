using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using Klickit_Task.Models;

namespace Klickit_Task.Controllers
{
    public class HomeController : Controller
    {

        //Check User Before Create New 
        public ActionResult CheckUser(string User_Email , int? User_ID)
        {
            //Create Case
            if (User_ID == null)
            {
                HttpResponseMessage message = GlobalVariables.WebAPIcLinet.GetAsync("api/User").Result;

                if (message.IsSuccessStatusCode)
                {
                    List<User> users = message.Content.ReadAsAsync<List<User>>().Result;

                    User user1 = users.Where(n => string.Equals(n.User_Email, User_Email, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

                    if (user1 == null)
                    {
                        return Json(true, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(false, JsonRequestBehavior.AllowGet);
                    }

                }
            }

            //Update Case
            else
            {
                HttpResponseMessage message = GlobalVariables.WebAPIcLinet.GetAsync("api/User").Result;

                if (message.IsSuccessStatusCode)
                {
                    List<User> users = message.Content.ReadAsAsync<List<User>>().Result;

                    User user1 = users.Where(n => string.Equals(n.User_Email, User_Email, StringComparison.CurrentCultureIgnoreCase) && n.User_ID != User_ID).FirstOrDefault();

                    if (user1 == null)
                    {
                        return Json(true, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(false, JsonRequestBehavior.AllowGet);
                    }

                }
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Login()
        {
           
            //if coockies founded in pc User
            if (Request.Cookies["coockie"] != null)
            {
                Session["userid"] = Request.Cookies["coockie"].Values["id"];
                int id = int.Parse(Session["userid"].ToString());

                if(Session["userid"] != null)
                {
                    HttpResponseMessage message = GlobalVariables.WebAPIcLinet.GetAsync("api/User").Result;

                    if (message.IsSuccessStatusCode)
                    {
                        List<User> users = message.Content.ReadAsAsync<List<User>>().Result;

                        User user1 = users.Where(n => n.User_ID == id).SingleOrDefault();

                        if (user1.User_Role == "User")
                        {
                            return RedirectToAction("Home");
                        }
                        else if (user1.User_Role == "Admin")
                        {
                            return RedirectToAction("Dashboard", "Admin");
                        }
                    }
                }
                else
                {
                    return RedirectToAction("Login");
                }

            }

            return View();
        }

        [HttpPost]
        public ActionResult Login(User user, string rememberme)
        {
            HttpResponseMessage message = GlobalVariables.WebAPIcLinet.GetAsync("api/User").Result;

            if (message.IsSuccessStatusCode)
            {
                List<User> users = message.Content.ReadAsAsync<List<User>>().Result;

                User user1 = users.Where(n => string.Equals(n.User_Email, user.User_Email, StringComparison.CurrentCultureIgnoreCase) && n.User_Password == user.User_Password).SingleOrDefault();

                if (user1 != null && user1.User_Role == "User")
                {
                    Session.Add("userid", user1.User_ID);

                    //cookie 
                    //if checkbox is checked
                    if (rememberme == "true")
                    {
                        HttpCookie cookie = new HttpCookie("coockie"); //create file
                        cookie.Values.Add("id", user1.User_ID.ToString()); //save data
                        cookie.Expires = DateTime.Now.AddDays(90); //expire date
                        Response.Cookies.Add(cookie);
                    }

                    return RedirectToAction("Home");
                }
                else if (user1 != null && user1.User_Role == "Admin")
                {
                    Session.Add("userid", user1.User_ID);

                    //cookie 
                    //if checkbox is checked
                    if (rememberme == "true")
                    {
                        HttpCookie cookie = new HttpCookie("coockie"); //create file
                        cookie.Values.Add("id", user1.User_ID.ToString()); //save data
                        cookie.Expires = DateTime.Now.AddDays(90); //expire date
                        Response.Cookies.Add(cookie);
                    }

                    return RedirectToAction("Dashboard", "Admin");
                }
                else
                {
                    ViewBag.status = "Invalid Username or Password";
                    return View();
                }
            }
            return View();
        }
        
        public ActionResult Logout()
        {
            Session["userid"] = null;
            Session["cart"] = null;
            HttpCookie cookie = new HttpCookie("coockie"); //create file
            cookie.Expires = DateTime.Now.AddDays(-15); //expire date (to delete coockie)
            Response.Cookies.Add(cookie);
            return RedirectToAction("Login");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(User user, string submit)
        {
            HttpResponseMessage message = GlobalVariables.WebAPIcLinet.PostAsJsonAsync("api/User", user).Result;

            if (message.IsSuccessStatusCode)
            {

                ViewBag.Message = "The Register Process is Completed";
                return RedirectToAction("Login");

            }

            return RedirectToAction("Login");

        }

        public ActionResult Home()
        {
            if (Session["userid"] != null)
            {
                return View();
            }
            else
                return RedirectToAction("Login");
        }

        public ActionResult About()
        {
            if (Session["userid"] != null)
            {
                return View();
            }
            else
                return RedirectToAction("Login");
        }

        public ActionResult Contact()
        {
            if (Session["userid"] != null)
            {
                return View();
            }
            else
                return RedirectToAction("Login");
        }

        public ActionResult Shop()
        {
            if (Session["userid"] != null)
            {
                HttpResponseMessage message = GlobalVariables.WebAPIcLinet.GetAsync("api/Product").Result;

                if (message.IsSuccessStatusCode)
                {
                    List<Product> products = message.Content.ReadAsAsync<List<Product>>().Result;
                    ViewBag.Prod = products.ToList();

                    if(Session["cart"] == null)
                    {
                        ViewBag.List = new List<Items>();
                    }
                    else
                    {
                        ViewBag.List = (List<Items>)Session["cart"];
                    }

                    return View();
                }
                return View();
            }

            else
                return RedirectToAction("Login");
        }

        public ActionResult AddCart(int id)
        {
            if (Session["userid"] != null) {

                //First Item Added
                if (Session["cart"] == null)
                {
                    List<Items> Cart = new List<Items>();

                    HttpResponseMessage message = GlobalVariables.WebAPIcLinet.GetAsync("api/Product").Result;

                    if (message.IsSuccessStatusCode)
                    {
                        List<Product> products = message.Content.ReadAsAsync<List<Product>>().Result;
                        Product product = products.Where(n => n.Prod_ID == id).SingleOrDefault();

                        Cart.Add(new Items()
                        {
                            Product = product,
                            Quantity = 1
                        });

                        Session["cart"] = Cart;

                    }
                }

                else
                {
                    List<Items> Cart = (List<Items>)Session["cart"];

                    HttpResponseMessage message = GlobalVariables.WebAPIcLinet.GetAsync("api/Product").Result;

                    if (message.IsSuccessStatusCode)
                    {
                        List<Product> products = message.Content.ReadAsAsync<List<Product>>().Result;
                        Product product = products.Where(n => n.Prod_ID == id).SingleOrDefault();

                        Cart.Add(new Items()
                        {
                            Product = product,
                            Quantity = 1
                        });

                        Session["cart"] = Cart;

                    }
                }

                return RedirectToAction("Shop");

            }
            else
                return RedirectToAction("Login");
        }

        public ActionResult RemoveCart(int id)
        {
            if (Session["userid"] != null)
            {
                List<Items> Cart = (List<Items>)Session["cart"];

                for (int i = 0; i < Cart.Count; i++)
                {
                    if (Cart[i].Product.Prod_ID == id)
                    {
                        Cart.Remove(Cart[i]);
                        break;
                    }
                }
                if (Cart.Count == 0)
                {
                    Session["cart"] = null;
                }
                else
                {
                    Session["cart"] = Cart;
                }

                return RedirectToAction("Shop");
            }
            else
                return RedirectToAction("Login");
        }

        public ActionResult Checkout()
        {
            if (Session["userid"] != null)
            {
                return View();
            }
            else
                return RedirectToAction("Login");
        }

        public ActionResult IncreaseQuantity(int id)
        {
            if (Session["userid"] != null)
            {
                if (Session["cart"] != null)
                {
                    List<Items> Cart = (List<Items>)Session["cart"];

                    HttpResponseMessage message = GlobalVariables.WebAPIcLinet.GetAsync("api/Product").Result;

                    if (message.IsSuccessStatusCode)
                    {
                        List<Product> products = message.Content.ReadAsAsync<List<Product>>().Result;
                        Product product = products.Where(n => n.Prod_ID == id).SingleOrDefault();

                        foreach (var items in Cart)
                        {
                            if (items.Product.Prod_ID == id)
                            {
                                int prevQty = items.Quantity;
                                Cart.Remove(items);
                                Cart.Add(new Items()
                                {
                                    Product = product,
                                    Quantity = prevQty + 1
                                });
                                break;
                            }
                        }

                        Session["cart"] = Cart;
                    }
                }
                return RedirectToAction("Checkout");
            }
            else
                return RedirectToAction("Login");
        }

        public ActionResult DecreaseQuantity(int id)
        {
            if (Session["userid"] != null)
            {
                if (Session["cart"] != null)
                {
                    List<Items> Cart = (List<Items>)Session["cart"];

                    HttpResponseMessage message = GlobalVariables.WebAPIcLinet.GetAsync("api/Product").Result;

                    if (message.IsSuccessStatusCode)
                    {
                        List<Product> products = message.Content.ReadAsAsync<List<Product>>().Result;
                        Product product = products.Where(n => n.Prod_ID == id).SingleOrDefault();

                        foreach (var items in Cart)
                        {
                            if (items.Product.Prod_ID == id)
                            {
                                int prevQty = items.Quantity;
                                Cart.Remove(items);
                                Cart.Add(new Items()
                                {
                                    Product = product,
                                    Quantity = prevQty - 1
                                });
                                break;
                            }
                        }

                        Session["cart"] = Cart;
                    }
                }
                return RedirectToAction("Checkout");
            }
            else
                return RedirectToAction("Login");
        }

        public ActionResult CheckoutDetails()
        {
            if (Session["userid"] != null)
            {
                int id = int.Parse(Session["userid"].ToString());

                HttpResponseMessage message = GlobalVariables.WebAPIcLinet.GetAsync("api/User").Result;

                HttpResponseMessage message1 = GlobalVariables.WebAPIcLinet.GetAsync("api/Order").Result;

                if (message.IsSuccessStatusCode)
                {

                    List<User> users = message.Content.ReadAsAsync<List<User>>().Result;
                    User user = users.Where(n=>n.User_ID == id ).SingleOrDefault();

                    List<Order> orders = message1.Content.ReadAsAsync<List<Order>>().Result;
                    long LastOrder = orders.Select(n => n.Order_ID).Last() +1;

                    if (orders.Count != 0)
                    {
                        ViewBag.Last = LastOrder;
                    }
                    else
                    {
                        ViewBag.Last = 1;
                    }

                    return View(user);

                }
                return View();
            }
            else
                return RedirectToAction("Login");
        }

        public ActionResult ConfirmOrder(Order order , List<Invoice> invoices , string submit)
        {
            Session["cart"] = null;

            if (Session["userid"] != null)
            {
                HttpResponseMessage message = GlobalVariables.WebAPIcLinet.PostAsJsonAsync("api/Order", order).Result;
                
                HttpResponseMessage message1 = GlobalVariables.WebAPIcLinet.PostAsJsonAsync("api/Invoice", invoices).Result;

                if (message.IsSuccessStatusCode && message1.IsSuccessStatusCode)
                {
                    ViewBag.Message = "The Order Confirm Process is Completed";
                    return RedirectToAction("Home");
                }
                return RedirectToAction("Home");
            }
            else
                return RedirectToAction("Login");
        }

        public ActionResult Orders()
        {
            if (Session["userid"] != null)
            {
                int id = int.Parse(Session["userid"].ToString());

                HttpResponseMessage message = GlobalVariables.WebAPIcLinet.GetAsync("api/Order").Result;

                if (message.IsSuccessStatusCode)
                {
                    List<Order> orders = message.Content.ReadAsAsync<List<Order>>().Result;
                    ViewBag.Orders = orders.Where(n => n.User_ID == id).ToList();
                }

                return View();
            }
            else
                return RedirectToAction("Login");
        }

        public ActionResult AddAccount()
        {
            int id = int.Parse(Session["userid"].ToString());

            if (Session["userid"] != null)
            {
                HttpResponseMessage message0 = GlobalVariables.WebAPIcLinet.GetAsync("api/User").Result;

                if (message0.IsSuccessStatusCode)
                {
                    List<User> users = message0.Content.ReadAsAsync<List<User>>().Result;

                    User user1 = users.Where(n => n.User_ID == id).SingleOrDefault();

                    if (user1.User_Role == "User")
                    {
                        return RedirectToAction("Home");
                    }
                    else if (user1.User_Role == "Admin")
                    {
                        return View();
                    }
                }
            }
            else
            {
                return RedirectToAction("Login");
            }
            return View();
        }

        [HttpPost]
        public ActionResult AddAccount(User user, string submit)
        {
            int id = int.Parse(Session["userid"].ToString());

            if (Session["userid"] != null)
            {
                HttpResponseMessage message0 = GlobalVariables.WebAPIcLinet.GetAsync("api/User").Result;

                if (message0.IsSuccessStatusCode)
                {
                    List<User> users = message0.Content.ReadAsAsync<List<User>>().Result;

                    User user1 = users.Where(n => n.User_ID == id).SingleOrDefault();

                    if (user1.User_Role == "User")
                    {
                        return RedirectToAction("Home");
                    }
                    else if (user1.User_Role == "Admin")
                    {
                        HttpResponseMessage message = GlobalVariables.WebAPIcLinet.PostAsJsonAsync("api/User", user).Result;

                        if (message.IsSuccessStatusCode)
                        {
                            ViewBag.Message = "The Add Admin Account Process is Completed";
                            return RedirectToAction("Account", "Admin");
                        }
                    }
                }
            }
            else
            {
                return RedirectToAction("Login");
            }
            return RedirectToAction("Account", "Admin");
        }

        public ActionResult UpdateAccount(int id)
        {
            int id1 = int.Parse(Session["userid"].ToString());

            if (Session["userid"] != null)
            {
                HttpResponseMessage message0 = GlobalVariables.WebAPIcLinet.GetAsync("api/User").Result;

                if (message0.IsSuccessStatusCode)
                {
                    List<User> users = message0.Content.ReadAsAsync<List<User>>().Result;

                    User user1 = users.Where(n => n.User_ID == id1).SingleOrDefault();

                    if (user1.User_Role == "User")
                    {
                        return RedirectToAction("Home");
                    }
                    else if (user1.User_Role == "Admin")
                    {
                        HttpResponseMessage message = GlobalVariables.WebAPIcLinet.GetAsync("api/User").Result;

                        if (message.IsSuccessStatusCode)
                        {
                            List<User> users1 = message.Content.ReadAsAsync<List<User>>().Result;
                            User user = users1.Where(n => n.User_ID == id).FirstOrDefault();
                            return View(user);
                        }
                    }
                }
            }
            else
            {
                return RedirectToAction("Login");
            }
            return View();
        }

        [HttpPost]
        public ActionResult UpdateAccount(User user, string submit)
        {
            int id = int.Parse(Session["userid"].ToString());

            if (Session["userid"] != null)
            {
                HttpResponseMessage message0 = GlobalVariables.WebAPIcLinet.GetAsync("api/User").Result;

                if (message0.IsSuccessStatusCode)
                {
                    List<User> users = message0.Content.ReadAsAsync<List<User>>().Result;

                    User user1 = users.Where(n => n.User_ID == id).SingleOrDefault();

                    if (user1.User_Role == "User")
                    {
                        return RedirectToAction("Home");
                    }
                    else if (user1.User_Role == "Admin")
                    {
                        HttpResponseMessage message = GlobalVariables.WebAPIcLinet.PutAsJsonAsync($"api/User/{user.User_ID}", user).Result;

                        if (message.IsSuccessStatusCode)
                        {
                            ViewBag.Message = "The Update User Account Process is Completed";

                            return RedirectToAction("Account", "Admin");
                        }
                    }
                }
            }
            else
            {
                return RedirectToAction("Login");
            }
            return RedirectToAction("Account", "Admin");            
        }

    }
}