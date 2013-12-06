using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dapper;
using StudentService.Helpers;
using UCDArch.Web.ActionResults;
using System.Web.Configuration;
using UCDArch.Core.Utils;

namespace StudentService.Controllers
{
    [AllowOrigin]
    public class MonitorController : Controller
    {
        // GET: /Monitor/
        public ActionResult Index(string key)
        {
            var monitorKey = WebConfigurationManager.AppSettings["monitor"];

            Check.Require(key == monitorKey, "Not a valid monitor key");

            using (var db = new DbManager())
            {
                var term = db.Connection.Query("select top 1 Id from terms").Single();
                return new JsonNetResult(term);
            }
        }

    }
}
