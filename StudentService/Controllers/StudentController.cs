using System.Linq;
using System.Net;
using System.Web.Mvc;
using Dapper;
using StudentService.Helpers;
using UCDArch.Web.ActionResults;
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
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Neither Department nor Term can be empty");
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

        // GET: /Student/RosterBySubject?term=201301&subject=ENL&courseNumber=262&key=1234
        public ActionResult RosterBySubject(string term, string subject, string courseNumber)
        {
            if (string.IsNullOrWhiteSpace(term) || string.IsNullOrWhiteSpace(subject) || string.IsNullOrWhiteSpace(courseNumber))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Term, Subject and CourseNumber need to be provided");
            }

            using (var db = new DbManager())
            {
                var rosterQuery = db.Connection.QueryMultiple(QueryResources.CourseSubjectQuery, 
                    new { Term = term, Subject = subject, CourseNumb = courseNumber });
                
                var students = rosterQuery.Read().ToList();
                var instructors = rosterQuery.Read().ToList();

                var courses = from c in students
                              group c by c.Crn into uniqueCourses
                              orderby uniqueCourses.Key
                              select new CourseRoster
                              {
                                  Crn = uniqueCourses.Key,
                                  Students = students.Where(s => s.Crn == uniqueCourses.Key).Select(s => new Person(s)).ToArray(),
                                  Instructors = instructors.Where(i => i.Crn == uniqueCourses.Key).Select(i => new Person(i) { Mi = i.Mi }).ToArray()
                              };
                
                return new JsonNetResult(courses);
            }
        }

        // GET: /Student/Roster?term=201301&crn=52960&key=1234
        public ActionResult Roster(string term, string crn){
            if (string.IsNullOrWhiteSpace(term) || string.IsNullOrWhiteSpace(crn))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Neither Term nor Crn can be empty");
            }
                        
            using (var db = new DbManager())
            {
                var rosterQuery = db.Connection.QueryMultiple(QueryResources.RosterQuery, new { @Term = term, @Crn = crn });

                var students = rosterQuery.Read().ToList();
                var instructors = rosterQuery.Read().ToList();

                return new JsonNetResult(new { students, instructors });
            }
        }
    }    
}
