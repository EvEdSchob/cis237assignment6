﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace cis237Assignment6.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "A bit about our company.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Please contact us with any questions about our products or services.";

            return View();
        }
    }
}