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

            HttpResponseMessage message = GlobalVariables.WebAPIcLinet.GetAsync("api/User").Result;

            HttpResponseMessage message1 = GlobalVariables.WebAPIcLinet.GetAsync("api/Product").Result;

            HttpResponseMessage message2 = GlobalVariables.WebAPIcLinet.GetAsync("api/Product").Result;

            HttpResponseMessage message3 = GlobalVariables.WebAPIcLinet.GetAsync("api/Order").Result;

            HttpResponseMessage message4 = GlobalVariables.WebAPIcLinet.GetAsync("api/User").Result;

            HttpResponseMessage message5 = GlobalVariables.WebAPIcLinet.GetAsync("api/Order").Result;

            HttpResponseMessage message6 = GlobalVariables.WebAPIcLinet.GetAsync("api/Order").Result;


            if (message.IsSuccessStatusCode)
            {
                List<User> users = message.Content.ReadAsAsync<List<User>>().Result;

                User user = users.Where(n => n.User_ID == id).SingleOrDefault();

                List<Product> sales = message1.Content.ReadAsAsync<List<Product>>().Result;
                List<double> Sales = sales.Select(n => n.Prod_Price).ToList();
                double TotalSales = 0;

                if (Sales.Count != 0)
                {
                    for(int i=0; i<Sales.Count; i++)
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

                return View(user);
            }

            return View();
        }

        public ActionResult Product()
        {
            HttpResponseMessage message = GlobalVariables.WebAPIcLinet.GetAsync("api/Product").Result;

            if (message.IsSuccessStatusCode)
            {
                List<Product> products = message.Content.ReadAsAsync<List<Product>>().Result;
                ViewBag.Prod = products.ToList();
            }

            return View();
        }

        public ActionResult AddProduct()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddProduct(Product product , string submit)
        {
            HttpResponseMessage message = GlobalVariables.WebAPIcLinet.PostAsJsonAsync("api/Product", product).Result;

            if (message.IsSuccessStatusCode)
            {

                ViewBag.Add = "The Add Process is Completed Successfully";
                return RedirectToAction("Product", "Admin");

            }

            return RedirectToAction("Product", "Admin");
        }

        public ActionResult DeleteProduct(int id)
        {

            HttpResponseMessage message = GlobalVariables.WebAPIcLinet.DeleteAsync($"api/Product/{id}").Result;

            if (message.IsSuccessStatusCode)
            {

                return RedirectToAction("Product", "Admin");

            }

            return RedirectToAction("Product", "Admin");
        }

        public ActionResult UpdateProduct(int id)
        {
            HttpResponseMessage message = GlobalVariables.WebAPIcLinet.GetAsync("api/Product").Result;

            if (message.IsSuccessStatusCode)
            {
                List<Product> products = message.Content.ReadAsAsync<List<Product>>().Result;
                Product product = products.Where(n => n.Prod_ID == id).FirstOrDefault();
                return View(product);
            }

            return View();
        }

        [HttpPost]
        public ActionResult UpdateProduct(Product product, string submit)
        {
            HttpResponseMessage message = GlobalVariables.WebAPIcLinet.PutAsJsonAsync($"api/Product/{product.Prod_ID}", product).Result;

            if (message.IsSuccessStatusCode)
            {
                ViewBag.Message = "The Update Product Process is Completed";

                return RedirectToAction("Product", "Admin");
            }

            return RedirectToAction("Product", "Admin");
        }

        public ActionResult Order()
        {
            HttpResponseMessage message = GlobalVariables.WebAPIcLinet.GetAsync("api/Order").Result;

            if (message.IsSuccessStatusCode)
            {
                List<Order> orders = message.Content.ReadAsAsync<List<Order>>().Result;
                ViewBag.Order = orders.ToList();
            }

            return View();
        }

        public ActionResult Pending()
        {
            HttpResponseMessage message = GlobalVariables.WebAPIcLinet.GetAsync("api/Order").Result;

            if (message.IsSuccessStatusCode)
            {
                List<Order> orders = message.Content.ReadAsAsync<List<Order>>().Result;
                ViewBag.Order = orders.Where(n => n.Status == "Pending").ToList();
            }

            return View();
        }

        public ActionResult Accept()
        {
            HttpResponseMessage message = GlobalVariables.WebAPIcLinet.GetAsync("api/Order").Result;

            if (message.IsSuccessStatusCode)
            {
                List<Order> orders = message.Content.ReadAsAsync<List<Order>>().Result;
                ViewBag.Order = orders.Where(n => n.Status == "Accept").ToList();
            }

            return View();
        }

        public ActionResult AcceptOrder(int id)
        {
            HttpResponseMessage message = GlobalVariables.WebAPIcLinet.GetAsync("api/Order").Result;

            if (message.IsSuccessStatusCode)
            {
                List<Order> orders = message.Content.ReadAsAsync<List<Order>>().Result;
                Order order = orders.Where(n => n.Order_ID == id).SingleOrDefault();

                return View(order);
            }
            return View();
        }

        [HttpPost]
        public ActionResult AcceptOrder(Order order , string submit)
        {
            RefuseOrder((int)order.Order_ID);

            HttpResponseMessage message = GlobalVariables.WebAPIcLinet.PostAsJsonAsync($"api/Order/{order.Order_ID}", order).Result;

            if (message.IsSuccessStatusCode)
            {
                ViewBag.Message = "The Accept Order Process is Completed";

                return RedirectToAction("Order", "Admin");
            }

            return RedirectToAction("Order", "Admin");
        }

        public ActionResult RefuseOrder(int id)
        {
            HttpResponseMessage message = GlobalVariables.WebAPIcLinet.DeleteAsync($"api/Order/{id}").Result;

            if (message.IsSuccessStatusCode)
            {

                return RedirectToAction("Order", "Admin");

            }

            return RedirectToAction("Order", "Admin");
        }

        public ActionResult Account()
        {
            HttpResponseMessage message = GlobalVariables.WebAPIcLinet.GetAsync("api/User").Result;

            if (message.IsSuccessStatusCode)
            {
                List<User> users = message.Content.ReadAsAsync<List<User>>().Result;
                ViewBag.Acc = users.ToList();
            }

            return View();
        }

        public ActionResult DeleteAccount(int id)
        {
            HttpResponseMessage message = GlobalVariables.WebAPIcLinet.DeleteAsync($"api/User/{id}").Result;

            if (message.IsSuccessStatusCode)
            {

                return RedirectToAction("Account", "Admin");

            }

            return RedirectToAction("Account", "Admin");
        }
    }
}