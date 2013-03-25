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
                var courses = courseQuery.Select(a => new {a.Subject, a.CourseNumb, a.Name}).Distinct();
                return new JsonNetResult(courses);
            }
        }

        // GET: /Course/Complete?term=201301&subject=MAT&courseNumbers=021A&courseNumbers=021B&key=1234
        public ActionResult Complete(string term, string subject, string[] courseNumbers)
        {
            if (string.IsNullOrWhiteSpace(subject) || courseNumbers == null || !courseNumbers.Any()|| string.IsNullOrWhiteSpace(term))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Neither Subject, Course Nubmers nor Term can be empty");
            }

            using (var db = new DbManager())
            {
                var cm = string.Join(",", courseNumbers.Select(a => "'" + a + "'"));
                var cq = string.Format(QueryResources.CoursesSubjectQuery, cm);
                var rq = string.Format(QueryResources.RostersSubjectQuery, cm);

                var courseQuery = db.Connection.Query(cq,
                    new { Term = term, Subject = subject });

                var rosterQuery = db.Connection.QueryMultiple(rq,
                    new { Term = term, Subject = subject });

                var students = rosterQuery.Read().ToList();
                var instructors = rosterQuery.Read().ToList();

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
                                                  Instructors = instructors.Where(a => a.Crn == y.First().Crn).Select(i => new Person(i)).ToArray()
                                              }
                                          })
                                  };

                return new JsonNetResult(courses);
            }
        }

    }
}
