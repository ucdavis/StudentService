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
            if (string.IsNullOrWhiteSpace(department) || string.IsNullOrWhiteSpace(term))
            {
                return Json(new { error = true, errorString = "Neither Department nor Term can be empty" });
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

        public ActionResult Roster(string term, string crn){
            if (string.IsNullOrWhiteSpace(term) || string.IsNullOrWhiteSpace(crn))
            {
                return Json(new { error = true, errorString = "Neither Term nor Crn can be empty" });
            }

            const string query = @"select s.Pidm
,s.FirstName
,s.LastName
,s.LoginId
,s.Email
from CourseRoster r
	inner join students s on r.Pidm = s.Pidm
where 
	r.Termcode = '201301' 
	AND r.Crn = '52960'	
select ci.InstructorId
,i.FirstName
,i.Mi
,i.LastName
,i.LoginId
,i.Email
from CourseInstructors ci
	inner join Instructors i on ci.InstructorId = i.Id
where
	ci.TermCode = '201301'
	AND ci.Crn = '52960'";

            using (var db = new DbManager())
            {
                var rosterQuery = db.Connection.QueryMultiple(query);

                var students = rosterQuery.Read().ToList();
                var instructors = rosterQuery.Read().ToList();

                return new JsonNetResult();
            }
        }
    }    
}
