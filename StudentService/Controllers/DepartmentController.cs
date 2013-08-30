using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Dapper;
using StudentService.Helpers;
using UCDArch.Web.ActionResults;

namespace StudentService.Controllers
{
    [AllowOrigin]
    [ValidateKey]
    public class DepartmentController : Controller
    {
        // GET: /Course/List?department=wxyz&term=201301&key=1234
        public ActionResult Find(string subject)
        {
            if (string.IsNullOrWhiteSpace(subject))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Subject cannot be empty");
            }

            using (var db = new DbManager())
            {
                var departments = db.Connection.Query(QueryResources.DeparmentFindQuery, new { id = subject });
                return new JsonNetResult(departments.FirstOrDefault());
            }
        }


    }
}
