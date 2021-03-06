﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RedisCacheSessionStore.Models;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace RedisCacheSessionStore.Controllers
{
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index()
        {
            //Load data from distributed data store asynchronously
            await HttpContext.Session.LoadAsync();
            //Get value from session
            string storedValue = HttpContext.Session.GetString("TestValue");
            if (storedValue == null)
            {
                //No value stored, set one
                storedValue = "Testing session in Redis. Time of storage: " + DateTime.Now.ToString("s");
                HttpContext.Session.SetString("TestValue", storedValue);
                //Store session data asynchronously
                await HttpContext.Session.CommitAsync();
            }
            ViewData["Message"] = "Value in session: " + storedValue;
            ViewData["SessionId"] = HttpContext.Session.Id;
            ViewData["Keys"] = HttpContext.Session.Keys;

            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
