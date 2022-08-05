using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using Klickit_Task.Models;


namespace Klickit_Task.Controllers
{
    public class AdminController : Controller
    {
        //Check Product Before Create New 
        public ActionResult CheckProduct(string Prod_Name, int? Prod_ID)
        {
            //Create Case
            if (Prod_ID == null)
            {
                HttpResponseMessage message = GlobalVariables.WebAPIcLinet.GetAsync("api/Product").Result;

                if (message.IsSuccessStatusCode)
                {
                    List<Product> products = message.Content.ReadAsAsync<List<Product>>().Result;

                    Product product = products.Where(n => n.Prod_Name == Prod_Name).FirstOrDefault();

                    if (product == null)
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
                HttpResponseMessage message = GlobalVariables.WebAPIcLinet.GetAsync("api/Product").Result;

                if (message.IsSuccessStatusCode)
                {
                    List<Product> products = message.Content.ReadAsAsync<List<Product>>().Result;

                    Product product = products.Where(n => n.Prod_Name == Prod_Name && n.Prod_ID != Prod_ID).FirstOrDefault();

                    if (product == null)
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

        public ActionResult Dashboard()
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
                        return RedirectToAction("Home","Home");
                    }
                    else if (user1.User_Role == "Admin")
                    {
                        HttpResponseMessage message = GlobalVariables.WebAPIcLinet.GetAsync("api/User").Result;

                        HttpResponseMessage message1 = GlobalVariables.WebAPIcLinet.GetAsync("api/Order").Result;

                        HttpResponseMessage message2 = GlobalVariables.WebAPIcLinet.GetAsync("api/Product").Result;

                        HttpResponseMessage message3 = GlobalVariables.WebAPIcLinet.GetAsync("api/Order").Result;

                        HttpResponseMessage message4 = GlobalVariables.WebAPIcLinet.GetAsync("api/User").Result;

                        HttpResponseMessage message5 = GlobalVariables.WebAPIcLinet.GetAsync("api/Order").Result;

                        HttpResponseMessage message6 = GlobalVariables.WebAPIcLinet.GetAsync("api/Order").Result;

                        HttpResponseMessage message7 = GlobalVariables.WebAPIcLinet.GetAsync("api/Order").Result;


                        if (message.IsSuccessStatusCode)
                        {
                            List<User> users1 = message.Content.ReadAsAsync<List<User>>().Result;

                            User user = users1.Where(n => n.User_ID == id).SingleOrDefault();

                            List<Order> sales = message1.Content.ReadAsAsync<List<Order>>().Result;
                            List<double> Sales = sales.Where(n=>n.Status == "Accept").Select(n => n.Total).ToList();
                            double TotalSales = 0;

                            if (Sales.Count != 0)
                            {
                                for (int i = 0; i < Sales.Count; i++)
                                {
                                    TotalSales += Sales[i];
                                }
                                ViewBag.Sales = TotalSales;
                            }
                            else
                            {
                                ViewBag.Sales = 0;
                            }

                            List<Product> products = message2.Content.ReadAsAsync<List<Product>>().Result;

                            if (products.Count != 0)
                            {
                                ViewBag.Prod = products.Count;
                            }
                            else
                            {
                                ViewBag.Prod = 0;
                            }

                            List<Order> orders = message3.Content.ReadAsAsync<List<Order>>().Result;

                            if (orders.Count != 0)
                            {
                                ViewBag.Order = orders.Count;
                            }
                            else
                            {
                                ViewBag.Order = 0;
                            }

                            List<User> customers = message4.Content.ReadAsAsync<List<User>>().Result;
                            List<User> Customers = customers.Where(n => n.User_Role == "User").ToList();

                            if (Customers.Count != 0)
                            {
                                ViewBag.Cust = Customers.Count;
                            }
                            else
                            {
                                ViewBag.Cust = 0;
                            }

                            List<Order> pending = message5.Content.ReadAsAsync<List<Order>>().Result;
                            List<Order> Pending = pending.Where(n => n.Status == "Pending").ToList();

                            if (Pending.Count != 0)
                            {
                                ViewBag.Pending = Pending.Count;
                            }
                            else
                            {
                                ViewBag.Pending = 0;
                            }

                            List<Order> accept = message6.Content.ReadAsAsync<List<Order>>().Result;
                            List<Order> Accept = accept.Where(n => n.Status == "Accept").ToList();

                            if (Accept.Count != 0)
                            {
                                ViewBag.Accept = Accept.Count;
                            }
                            else
                            {
                                ViewBag.Accept = 0;
                            }

                            List<Order> reject = message7.Content.ReadAsAsync<List<Order>>().Result;
                            List<Order> Reject = reject.Where(n => n.Status == "Reject").ToList();

                            if (Reject.Count != 0)
                            {
                                ViewBag.Reject = Reject.Count;
                            }
                            else
                            {
                                ViewBag.Reject = 0;
                            }

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

        public ActionResult Product()
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
                        return RedirectToAction("Home", "Home");
                    }
                    else if (user1.User_Role == "Admin")
                    {
                        HttpResponseMessage message = GlobalVariables.WebAPIcLinet.GetAsync("api/Product").Result;

                        if (message.IsSuccessStatusCode)
                        {
                            List<Product> products = message.Content.ReadAsAsync<List<Product>>().Result;
                            ViewBag.Prod = products.ToList();
                        }

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

        public ActionResult AddProduct()
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
                        return RedirectToAction("Home", "Home");
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
        public ActionResult AddProduct(Product product , string submit)
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
                        return RedirectToAction("Home", "Home");
                    }
                    else if (user1.User_Role == "Admin")
                    {
                        HttpResponseMessage message = GlobalVariables.WebAPIcLinet.PostAsJsonAsync("api/Product", product).Result;

                        if (message.IsSuccessStatusCode)
                        {

                            ViewBag.Add = "The Add Process is Completed Successfully";
                            return RedirectToAction("Product", "Admin");

                        }
                    }
                }
            }
            else
            {
                return RedirectToAction("Login");
            }
            return RedirectToAction("Product", "Admin");
        }

        public ActionResult DeleteProduct(int id)
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
                        return RedirectToAction("Home", "Home");
                    }
                    else if (user1.User_Role == "Admin")
                    {
                        HttpResponseMessage message = GlobalVariables.WebAPIcLinet.DeleteAsync($"api/Product/{id}").Result;

                        if (message.IsSuccessStatusCode)
                        {

                            return RedirectToAction("Product", "Admin");

                        }
                    }
                }
            }
            else
            {
                return RedirectToAction("Login");
            }
            return RedirectToAction("Product", "Admin");
        }

        public ActionResult UpdateProduct(int id)
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
                        return RedirectToAction("Home", "Home");
                    }
                    else if (user1.User_Role == "Admin")
                    {
                        HttpResponseMessage message = GlobalVariables.WebAPIcLinet.GetAsync("api/Product").Result;

                        if (message.IsSuccessStatusCode)
                        {
                            List<Product> products = message.Content.ReadAsAsync<List<Product>>().Result;
                            Product product = products.Where(n => n.Prod_ID == id).FirstOrDefault();
                            return View(product);
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
        public ActionResult UpdateProduct(Product product, string submit)
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
                        return RedirectToAction("Home", "Home");
                    }
                    else if (user1.User_Role == "Admin")
                    {
                        HttpResponseMessage message = GlobalVariables.WebAPIcLinet.GetAsync("api/Product").Result;

                        if (message.IsSuccessStatusCode)
                        {
                            HttpResponseMessage message1 = GlobalVariables.WebAPIcLinet.PutAsJsonAsync($"api/Product/{product.Prod_ID}", product).Result;

                            if (message1.IsSuccessStatusCode)
                            {
                                ViewBag.Message = "The Update Product Process is Completed";

                                return RedirectToAction("Product", "Admin");
                            }
                        }
                    }
                }
            }
            else
            {
                return RedirectToAction("Login");
            }
            return RedirectToAction("Product", "Admin");
        }

        public ActionResult Order()
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
                        return RedirectToAction("Home", "Home");
                    }
                    else if (user1.User_Role == "Admin")
                    {
                        HttpResponseMessage message = GlobalVariables.WebAPIcLinet.GetAsync("api/Order").Result;

                        if (message.IsSuccessStatusCode)
                        {
                            List<Order> orders = message.Content.ReadAsAsync<List<Order>>().Result;
                            ViewBag.Order = orders.ToList();
                        }
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

        public ActionResult Pending()
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
                        return RedirectToAction("Home", "Home");
                    }
                    else if (user1.User_Role == "Admin")
                    {
                        HttpResponseMessage message = GlobalVariables.WebAPIcLinet.GetAsync("api/Order").Result;

                        if (message.IsSuccessStatusCode)
                        {
                            List<Order> orders = message.Content.ReadAsAsync<List<Order>>().Result;
                            ViewBag.Order = orders.Where(n => n.Status == "Pending").ToList();
                        }

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

        public ActionResult Accept()
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
                        return RedirectToAction("Home", "Home");
                    }
                    else if (user1.User_Role == "Admin")
                    {
                        HttpResponseMessage message = GlobalVariables.WebAPIcLinet.GetAsync("api/Order").Result;

                        if (message.IsSuccessStatusCode)
                        {
                            List<Order> orders = message.Content.ReadAsAsync<List<Order>>().Result;
                            ViewBag.Order = orders.Where(n => n.Status == "Accept").ToList();
                        }

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

        public ActionResult Reject()
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
                        return RedirectToAction("Home", "Home");
                    }
                    else if (user1.User_Role == "Admin")
                    {
                        HttpResponseMessage message = GlobalVariables.WebAPIcLinet.GetAsync("api/Order").Result;

                        if (message.IsSuccessStatusCode)
                        {
                            List<Order> orders = message.Content.ReadAsAsync<List<Order>>().Result;
                            ViewBag.Order = orders.Where(n => n.Status == "Reject").ToList();
                        }

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

        public ActionResult AcceptOrder(int id)
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
                        return RedirectToAction("Home", "Home");
                    }
                    else if (user1.User_Role == "Admin")
                    {
                        HttpResponseMessage message = GlobalVariables.WebAPIcLinet.GetAsync("api/Order").Result;

                        if (message.IsSuccessStatusCode)
                        {
                            List<Order> orders = message.Content.ReadAsAsync<List<Order>>().Result;
                            Order order = orders.Where(n => n.Order_ID == id).SingleOrDefault();

                            return View(order);
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
        public ActionResult AcceptOrder(Order order , string submit)
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
                        return RedirectToAction("Home", "Home");
                    }
                    else if (user1.User_Role == "Admin")
                    {
                        Delete((int)order.Order_ID);

                        HttpResponseMessage message = GlobalVariables.WebAPIcLinet.PostAsJsonAsync($"api/Order/{order.Order_ID}", order).Result;

                        if (message.IsSuccessStatusCode)
                        {
                            ViewBag.Message = "The Accept Order Process is Completed";

                            return RedirectToAction("Order", "Admin");
                        }
                    }
                }
            }
            else
            {
                return RedirectToAction("Login");
            }
            return RedirectToAction("Order", "Admin");
        }

        public ActionResult RefuseOrder(int id)
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
                        return RedirectToAction("Home", "Home");
                    }
                    else if (user1.User_Role == "Admin")
                    {
                        HttpResponseMessage message = GlobalVariables.WebAPIcLinet.GetAsync("api/Order").Result;

                        if (message.IsSuccessStatusCode)
                        {
                            List<Order> orders = message.Content.ReadAsAsync<List<Order>>().Result;
                            Order order = orders.Where(n => n.Order_ID == id).SingleOrDefault();

                            return View(order);
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
        public ActionResult RefuseOrder(Order order, string submit)
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
                        return RedirectToAction("Home", "Home");
                    }
                    else if (user1.User_Role == "Admin")
                    {
                        Delete((int)order.Order_ID);

                        HttpResponseMessage message = GlobalVariables.WebAPIcLinet.PostAsJsonAsync($"api/Order/{order.Order_ID}", order).Result;

                        if (message.IsSuccessStatusCode)
                        {
                            ViewBag.Message = "The Refuse Order Process is Completed";

                            return RedirectToAction("Order", "Admin");
                        }
                    }
                }
            }
            else
            {
                return RedirectToAction("Login");
            }
            return RedirectToAction("Order", "Admin");
        }

        public ActionResult Delete(int id)
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
                        return RedirectToAction("Home", "Home");
                    }
                    else if (user1.User_Role == "Admin")
                    {
                        HttpResponseMessage message = GlobalVariables.WebAPIcLinet.DeleteAsync($"api/Order/{id}").Result;

                        if (message.IsSuccessStatusCode)
                        {

                            return RedirectToAction("Order", "Admin");

                        }
                    }
                }
            }
            else
            {
                return RedirectToAction("Login");
            }
            return RedirectToAction("Order", "Admin");
        }

        public ActionResult Account()
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
                        return RedirectToAction("Home", "Home");
                    }
                    else if (user1.User_Role == "Admin")
                    {
                        ViewBag.Acc = users.ToList();
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

        public ActionResult DeleteAccount(int id)
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
                        return RedirectToAction("Home", "Home");
                    }
                    else if (user1.User_Role == "Admin")
                    {
                        HttpResponseMessage message = GlobalVariables.WebAPIcLinet.DeleteAsync($"api/User/{id}").Result;

                        if (message.IsSuccessStatusCode)
                        {
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