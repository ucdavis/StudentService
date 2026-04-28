using System.Linq;
using System.Net;
using System.Web.Mvc;
using Dapper;
using StudentService.Helpers;
using StudentService.Models;
using UCDArch.Web.ActionResults;

namespace StudentService.Controllers
{
    [AllowOrigin]
    [ValidateKey]
    public class CourseController : Controller
    {
        // GET: /Course/List?department=wxyz&term=201301&key=1234
        public ActionResult List(string term, string department)
        {
            if (string.IsNullOrWhiteSpace(department) || string.IsNullOrWhiteSpace(term))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Neither Department nor Term can be empty");
            }

            using (var db = new DbManager())
            {
                var courseQuery = db.Connection.Query(QueryResources.CourseSectionQuery, new { Term = term, Department = department });
                var courses = courseQuery.Select(a => new { a.Subject, a.CourseNumb, a.Name }).Distinct();
                return new JsonNetResult(courses);
            }
        }

        // GET: /Course/Complete?term=201301&subject=MAT&courseNumbers=021A&courseNumbers=021B&key=1234
        public ActionResult Complete(string term, string subject, string[] courseNumbers)
        {
            var normalizedCourseNumbers = courseNumbers == null
                ? new string[0]
                : courseNumbers
                    .Where(a => !string.IsNullOrWhiteSpace(a))
                    .Select(a => a.Trim())
                    .Distinct()
                    .ToArray();

            if (string.IsNullOrWhiteSpace(subject) || !normalizedCourseNumbers.Any() || string.IsNullOrWhiteSpace(term))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Neither Subject, Course Nubmers nor Term can be empty");
            }

            using (var db = new DbManager())
            {
                var queryParameters = new { Term = term, Subject = subject, CourseNumbers = normalizedCourseNumbers };

                var courseQuery = db.Connection.Query(QueryResources.CoursesSubjectQuery,
                    queryParameters);

                var rosterQuery = db.Connection.QueryMultiple(QueryResources.RostersSubjectQuery,
                    queryParameters);

                var students = rosterQuery.Read().ToList();
                var instructors = rosterQuery.Read().ToList();
                var teachingAssistants = rosterQuery.Read().ToList();

                var courses = from c in courseQuery
                              group c by new { c.Subject, c.CourseNumb }
                                  into x
                              select new Course(x.First())
                              {
                                  Sections = x.GroupBy(y => y.Sequence)
                                      .Select(y => new Section(y.First())
                                      {
                                          Classtimes = y.Select(z => new Classtime(z)),
                                          CourseRoster = new CourseRoster()
                                          {
                                              Students = students.Where(a => a.Crn == y.First().Crn).Select(s => new Person(s)).ToArray(),
                                              Instructors = instructors.Where(a => a.Crn == y.First().Crn).Select(i => new Person(i)).ToArray(),
                                              TeachingAssistants = teachingAssistants.Where(a => a.Crn == y.First().Crn).Select(t => new Person(t)).ToArray()
                                          }
                                      })
                              };

                return new JsonNetResult(courses);
            }
        }

        // GET: /Course/GradesRemaining?term=201306&crns=60152&crns=60170&key=1234
        public ActionResult GradesRemaining(string term, string[] crns)
        {
            if (crns == null || !crns.Any() || string.IsNullOrWhiteSpace(term))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Neither Course Nubmers nor Term can be empty");
            }

            using (var db = new DbManager())
            {
                var courses = db.Connection.Query("SELECT TermCode, Crn, GradesOutstandingCount, Gradable FROM Courses WHERE termCode = @term AND crn in @crns", new { term, crns });

                return new JsonNetResult(courses);
            }
        }
    }
}
