using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Dapper;
using StudentService.Helpers;
using UCDArch.Web.ActionResults;
using System;
using StudentService.Models;

namespace StudentService.Controllers
{
    [ValidateKey]
    public class StudentController : Controller
    {
        // GET: /Student/Courses?department=wxyz&term=201301&key=1234
        public ActionResult Courses(string department, string term)
        {
            if (string.IsNullOrWhiteSpace(department))
            {
                return Json(new { error = true, errorString = "Department cannot be empty" });
            }

            using (var db = new DbManager())
            {
                var courseQuery = db.Connection.Query(QueryResources.CourseSectionQuery, new { Term = term, Department = department });

                var courses = from c in courseQuery
                              group c by c.Crn into uniqueCourses
                              orderby uniqueCourses.Key
                              select new
                              {
                                  course = new Course(uniqueCourses.First())
                                  {
                                      Sections = uniqueCourses.Select(x => new Section(x))
                                  }
                              };
                
                return new JsonNetResult(courses);
            }
        }
    }    
}
