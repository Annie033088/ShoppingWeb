using Pashamao.Filters;
using System.Web.Mvc;

namespace Pashamao.Controllers
{
    [RequestLoggerFilter]
    public class MainOrderController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}