using LaptopShopUI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace LaptopShopUI.Controllers
{
    public class HomeController : Controller
    {
        string Baseurl = "http://ec2-18-191-150-168.us-east-2.compute.amazonaws.com/api/";
        //string Baseurl = "https://localhost:44357/";
        // GET: Home
        public async Task<ActionResult> Index()
        {
            //Hosted web API REST Service base url 
            
            List<Laptop> ProdInfo = new List<Laptop>();
            using (var client = new HttpClient())
            {
                //Passing service base url 
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                //Define request data format 
                client.DefaultRequestHeaders.Accept.Add(new
               MediaTypeWithQualityHeaderValue("application/json"));
                //Sending request to find web api REST service resource GetAllEmployees using HttpClient
                HttpResponseMessage Res = await client.GetAsync("Laptop");
                //Checking the response is successful or not which is sent using HttpClient
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api 
                    var PrResponse = Res.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the Product list
                    ProdInfo = JsonConvert.DeserializeObject<List<Laptop>>(PrResponse);
                }
                //returning the Product list to view 
                return View(ProdInfo);
            }
        }

        // GET: Home/Details/5
        public async Task<ActionResult> Details(int id)
        {
            Console.WriteLine("ID IS: " + id);
            Laptop laptop = null;
            using (var client = new HttpClient())
            {
                await SetHeader(client);
                //Sending request to find web api REST service resource GetAllEmployees using HttpClient
                HttpResponseMessage Res = await client.GetAsync("Laptop/" + id);
                //Checking the response is successful or not which is sent using HttpClient
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api 
                    var PrResponse = Res.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the Product list
                    laptop = JsonConvert.DeserializeObject<Laptop>(PrResponse);
                    Console.WriteLine(laptop.Name);                }
                //returning the Product list to view 
                return View(laptop);
            }
        }

        // GET: Product/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            Laptop laptop = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                HttpResponseMessage Res = await client.GetAsync("Laptop/" + id);
                //Checking the response is successful or not which is sent using HttpClient
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api 
                    var PrResponse = Res.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the Product list
                    laptop = JsonConvert.DeserializeObject<Laptop>(PrResponse);
                }
                else
                    ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            }

            return View(laptop);
        }
        // POST: Product/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(int id, Laptop laptop)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);
                    client.PutAsJsonAsync("Laptop/" + id, laptop).Wait();
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Laptop laptop)
        {
            try
            {
                using(HttpClient client = new())
                {
                    client.BaseAddress = new Uri(Baseurl);
                    client.PostAsJsonAsync("Laptop", laptop).Wait();
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                return View();
            }
        }

        public async Task<ActionResult> Delete(Laptop laptop, int id)
        {
            Laptop laptopToDelete = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                HttpResponseMessage Res = await client.GetAsync("Laptop/" + id);
                //Checking the response is successful or not which is sent using HttpClient
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api 
                    var PrResponse = Res.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the Product list
                    laptopToDelete = JsonConvert.DeserializeObject<Laptop>(PrResponse);
                }
                else
                    ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            }
            return View(laptopToDelete);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Laptop laptop)
        {
            try
            {
                using (HttpClient client = new())
                {
                    client.BaseAddress = new Uri(Baseurl);
                    client.DeleteAsync("Laptop/" + id).Wait();
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                return View();
            }
        }

        public async Task SetHeader(HttpClient client)
        {
            //Passing service base url 
            client.BaseAddress = new Uri(Baseurl);
            client.DefaultRequestHeaders.Clear();
            //Define request data format 
            client.DefaultRequestHeaders.Accept.Add(new
           MediaTypeWithQualityHeaderValue("application/json"));
        }

    }
}
