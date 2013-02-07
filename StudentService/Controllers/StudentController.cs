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
                return Json(new { error = true, errorString = "Department cannot be empty" });
            }

            const string query =
@"SELECT     c.TermCode
	, c.Crn
	, c.Subject
	, c.CourseNumb
	, c.Sequence
	, c.Name
	, s.SectionType
	, s.StartDate
	, s.EndDate
	, s.StartTime
	, s.EndTime
	, s.DaysOfWeek
FROM         
	Courses c 
	INNER JOIN Sections s
        ON c.TermCode = s.TermCode 
            AND c.Crn = s.Crn
WHERE 
	c.TermCode = '201301' 
	AND c.DepartmentId = 'ENL'
    AND c.Crn = '73160'
";

            using (var db = new DbManager())
            {
                var courseQuery = db.Connection.Query(query);

                var courses = from c in courseQuery
                              group c by c.Crn into uniqueCourses
                              orderby uniqueCourses.Key
                              select new
                              {
                                  course = new Course
                                  {
                                      Crn = uniqueCourses.First().Crn,
                                      Name = uniqueCourses.First().Name,
                                      Sections = uniqueCourses.Select(
                                        x => new Section { Type = x.SectionType, DaysOfWeek = x.DaysOfWeek }
                                      )
                                  }
                              };
                
                return new JsonNetResult(courses);
            }
        }
    }

    public class Course
    {
        public int Crn { get; set; }
        public string Name { get; set; }
        public IEnumerable<Section> Sections { get; set; }
    }

    public class Section
    {
        public string Type { get; set; }
        public string DaysOfWeek { get; set; }
    }
}
