using System.Linq;
using System.Net;
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
        public ActionResult Find(string subject)
        {
            if (string.IsNullOrWhiteSpace(subject))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Subject cannot be empty");
            }

            using (var db = new DbManager())
            {
                var departments = db.Connection.Query(QueryResources.DeparmentFindQuery, new {subject});
                return new JsonNetResult(departments.FirstOrDefault());
            }
        }

        public ActionResult Crn(string crn)
        {
            if (string.IsNullOrWhiteSpace(crn))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Crn cannot be empty");
            }

            using (var db = new DbManager())
            {
                var departments = db.Connection.Query(QueryResources.DepartmentCrnQuery, new {crn});
                return new JsonNetResult(departments.FirstOrDefault());
            }
        }
    }
}
