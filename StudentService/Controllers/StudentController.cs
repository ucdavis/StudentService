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
    public class StudentController : Controller
    {
        // GET: /Student/Courses?department=wxyz&key=1234
        public ActionResult Courses(string department)
        {
            if (string.IsNullOrWhiteSpace(department))
            {
                return Json(new {error = true, errorString = "Department cannot be empty"});
            }

            using (var db = new DbManager())
            {
                
            }

            return new JsonNetResult();
        }
    }
}
