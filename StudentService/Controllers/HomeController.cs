using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UCDArch.Web.Attributes;

namespace StudentService.Controllers
{
    [HandleTransactionsManually] //No ORM data access needed in this application
    public class HomeController : ApplicationController
    {
        public ActionResult Index()
        {
            return View();
        }

    }
}
