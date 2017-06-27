using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Dapper;
using StudentService.Helpers;
using UCDArch.Web.ActionResults;
using StudentService.Models;

namespace StudentService.Controllers
{
    [AllowOrigin]
    [ValidateKey]
    public class StudentController : Controller
    {
        // GET: /Student/TermCodes?key=1234
        public ActionResult TermCodes()
        {
            using (var db = new DbManager())
            {
                var terms = db.Connection.Query(QueryResources.TermCodeQuery);

                return new JsonNetResult(terms);
            }
        }

        // GET: /Student/Course?term=201303&subject

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
                              group c by new {c.Subject, c.CourseNumb}
                              into x
                              select new Course(x.First())
                                  {
                                      Sections =
                                          x.GroupBy(y => y.Sequence)
                                           .Select(y => new Section(y.First()) {Classtimes = y.Select(z => new Classtime(z))})
                                  };

                return new JsonNetResult(courses);
            }
        }

        // GET: /Student/Course?term=201301&subject=ENL&courseNumber=262&key=1234
        public ActionResult Course(string term, string subject, string courseNumber)
        {
            if (string.IsNullOrWhiteSpace(term) || string.IsNullOrWhiteSpace(subject) || string.IsNullOrWhiteSpace(courseNumber))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Term, Subject and CourseNumber need to be provided");
            }

            using (var db = new DbManager())
            {
                var courseQuery = db.Connection.Query(QueryResources.CourseSubjectQuery, 
                    new { Term = term, Subject = subject, CourseNumb = courseNumber });

                var courses = from c in courseQuery
                              group c by new { c.Subject, c.CourseNumb }
                                  into x
                                  select new Course(x.First())
                                  {
                                      Sections =
                                          x.GroupBy(y => y.Sequence)
                                           .Select(y => new Section(y.First()) { Classtimes = y.Select(z => new Classtime(z)) })
                                  };

                return new JsonNetResult(courses.FirstOrDefault());
            }
        }

        // GET: /Student/CourseRoster?term=201301&subject=ENL&courseNumber=262&key=1234
        public ActionResult CourseRoster(string term, string subject, string courseNumber)
        {
            if (string.IsNullOrWhiteSpace(term) || string.IsNullOrWhiteSpace(subject) || string.IsNullOrWhiteSpace(courseNumber))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Term, Subject and CourseNumber need to be provided");
            }

            using (var db = new DbManager())
            {
                var rosterQuery = db.Connection.QueryMultiple(QueryResources.RosterSubjectQuery, 
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
                                  Instructors = instructors.Where(i => i.Crn == uniqueCourses.Key).Select(i => new Person(i)).ToArray()
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

        // GET: /Student/SearchStudentByLogin?login=adam
        /// <summary>
        /// An API for use with cloud-based Commencement to search for a student
        /// by login that is not already present in the list of students.
        /// Executes Commencement.dbo.usp_SearchStudentByLogin and returns
        /// the results in a new Student object or a HTTP 404 if not found.
        /// </summary>
        /// <param name="login"></param>
        /// <returns>Student</returns>
        public ActionResult SearchStudentByLogin(string login)
        {
            if (string.IsNullOrWhiteSpace(login))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Student Login cannot be empty");
            }

            using (var db = new CommencementDbManager())
            {
                var studentQuery = db.Connection.Query<Student>(QueryResources.StudentByLoginQuery, new { @Login = login },
                    transaction: null, buffered: true, commandTimeout: 120);

                var result = studentQuery.FirstOrDefault();

                if (result == null)
                    return HttpNotFound(string.Format("Student with login \"{0}\" not found", login));
                    
                var student = result;

                return new JsonNetResult(student);
            }
        }

        // GET: /Student/SearchStudent?studentid=012345678
        /// <summary>
        /// An API for use with cloud-based Commencement to search for a student
        /// by Student ID that is not already present in the list of students.
        /// Executes Commencement.dbo.usp_SearchStudent and returns
        /// the results in a new Student object or a HTTP 404 if not found.
        /// </summary>
        /// <param name="studentid"></param>
        /// <returns>List of Students</returns>
        public ActionResult SearchStudent(string studentid)
        {
            if (string.IsNullOrWhiteSpace(studentid))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "StudentId cannot be empty");
            }

            using (var db = new CommencementDbManager())
            {
                var studentQuery = db.Connection.Query<Student>(QueryResources.StudentByIdQuery, 
                    new { @StudentId = studentid },
                    transaction: null, buffered: true, commandTimeout: 120);
               
                var student = studentQuery.FirstOrDefault();

                // For this we would really want back just an empty list, so we're not returning 404.
                //if (!students.Any())
                //    return HttpNotFound(string.Format("Student with StudentId \"{0}\" not found", studentid));

                return new JsonNetResult(student);
            }
        }
    }    
}
