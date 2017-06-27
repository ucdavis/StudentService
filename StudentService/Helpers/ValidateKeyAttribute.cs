using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace StudentService.Helpers
{
    public class ValidateKeyAttribute : AuthorizeAttribute
    {

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var key = httpContext.Request.QueryString["key"];
            string[] tokens = WebConfigurationManager.AppSettings["secret"].Split(',');
            return tokens.Contains(key);
        }
    }
}