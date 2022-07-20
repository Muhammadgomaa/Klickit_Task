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

            if (message.IsSuccessStatusCode)
            {
                List<User> users = message.Content.ReadAsAsync<List<User>>().Result;

                User user = users.Where(n => n.User_ID == id).SingleOrDefault();

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