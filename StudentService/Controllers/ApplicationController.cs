using UCDArch.Web.Controller;
using UCDArch.Web.Attributes;

namespace StudentService.Controllers
{
    [Version(MajorVersion = 3)]
    //[ServiceMessage("StudentService", ViewDataKey = "ServiceMessages", MessageServiceAppSettingsKey = "MessageService")]
    [HandleTransactionsManually] //No ORM data access needed in this application
    public abstract class ApplicationController : SuperController
    {
    }
}