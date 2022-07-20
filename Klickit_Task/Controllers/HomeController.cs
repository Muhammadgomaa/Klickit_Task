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

                    User user1 = users.Where(n => n.User_Email == User_Email).FirstOrDefault();

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

                    User user1 = users.Where(n => n.User_Email == User_Email && n.User_ID != User_ID).FirstOrDefault();

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

            return View();
        }

        [HttpPost]
        public ActionResult Login(User user, string rememberme)
        {
            HttpResponseMessage message = GlobalVariables.WebAPIcLinet.GetAsync("api/User").Result;

            if (message.IsSuccessStatusCode)
            {
                List<User> users = message.Content.ReadAsAsync<List<User>>().Result;

                User user1 = users.Where(n => n.User_Email == user.User_Email && n.User_Password == user.User_Password).SingleOrDefault();

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
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult Shop()
        {

            HttpResponseMessage message = GlobalVariables.WebAPIcLinet.GetAsync("api/Product").Result;

            if (message.IsSuccessStatusCode)
            {
                List<Product> products= message.Content.ReadAsAsync<List<Product>>().Result;
                ViewBag.Prod = products.ToList();

            }

            HttpResponseMessage message1 = GlobalVariables.WebAPIcLinet.GetAsync("api/Order").Result;

            int id = int.Parse(Session["userid"].ToString());

            if (message1.IsSuccessStatusCode)
            {
                List<Order> orders= message1.Content.ReadAsAsync<List<Order>>().Result;
                ViewBag.ProductsOrder = orders.Where(n=>n.Status == "Pending" && n.User_ID == id).Select(n=>n.Prod_ID).ToList();
            }

            return View();  
        }

        public ActionResult RequestOrder(int id)
        {
            HttpResponseMessage message = GlobalVariables.WebAPIcLinet.GetAsync("api/Product").Result;

            if (message.IsSuccessStatusCode)
            {
                List<Product> products = message.Content.ReadAsAsync<List<Product>>().Result;
                ViewBag.Prod = products.Where(n=>n.Prod_ID == id).SingleOrDefault();
            }
                return View();
        }

        public ActionResult ConfirmOrder(Order order)
        {
            HttpResponseMessage message = GlobalVariables.WebAPIcLinet.PostAsJsonAsync("api/Order", order).Result;

            if (message.IsSuccessStatusCode)
            {

                ViewBag.Message = "The Order Confirm Process is Completed";
                return RedirectToAction("Home");

            }

            return RedirectToAction("Home");
        }

        public ActionResult Orders()
        {
            int id = int.Parse(Session["userid"].ToString());

            HttpResponseMessage message = GlobalVariables.WebAPIcLinet.GetAsync("api/Order").Result;

            if (message.IsSuccessStatusCode)
            {
                List<Order> orders= message.Content.ReadAsAsync<List<Order>>().Result;
                ViewBag.Orders = orders.Where(n => n.User_ID == id).ToList();
            }

            return View();
        }

        public ActionResult AddAccount()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddAccount(User user, string submit)
        {
            HttpResponseMessage message = GlobalVariables.WebAPIcLinet.PostAsJsonAsync("api/User", user).Result;

            if (message.IsSuccessStatusCode)
            {

                ViewBag.Message = "The Add Admin Account Process is Completed";
                return RedirectToAction("Account", "Admin");

            }

            return RedirectToAction("Account", "Admin");
        }

        public ActionResult UpdateAccount(int id)
        {
            HttpResponseMessage message = GlobalVariables.WebAPIcLinet.GetAsync("api/User").Result;

            if (message.IsSuccessStatusCode)
            {
                List<User> users = message.Content.ReadAsAsync<List<User>>().Result;
                User user = users.Where(n => n.User_ID == id).FirstOrDefault();
                return View(user);
            }

            return View();
        }

        [HttpPost]
        public ActionResult UpdateAccount(User user, string submit)
        {
            HttpResponseMessage message = GlobalVariables.WebAPIcLinet.PutAsJsonAsync($"api/User/{user.User_ID}", user).Result;

            if (message.IsSuccessStatusCode)
            {
                ViewBag.Message = "The Update User Account Process is Completed";

                return RedirectToAction("Account", "Admin");
            }

            return RedirectToAction("Account", "Admin");
        }

    }
}